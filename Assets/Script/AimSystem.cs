using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class defines a target that is hit by raycast from camera.
public class AimSystem : MonoBehaviour {

    private Camera m_Camera;
    private PlayerData m_PlayerData;
    private PlayerInput m_PlayerInput;

    public float m_TargetMinDistance = 0f;
    public Transform m_RayTarget;    //if Raycast is true, this is position of RaycastHit.
                                        //but not, this is position of target moderate forward at camera.
    private Vector3 m_RayStartPos;
    private RaycastHit m_RaycastHit;
    private int m_RaycastLayermask;       //Layer for raycast to ignore
    public Vector3 m_LookTarget;       //Target Player Look at.

    public Image[] m_CrossHairImages;   //RIght Left Up Down

    private Vector3[] DirectionSet = { new Vector3(1,0,0), new Vector3(-1,0,0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    public float m_CrossHairOffSetPos = 10f;
    public float UpRecoilMultiplyer = 0.5f;

    private float CrosshairOffsetResolutionMultiplier = 1f;

    //Test 
    public float LookDistance = 100f;

    private void Awake()
    {
        m_Camera = Camera.main;
        m_PlayerData = GetComponent<PlayerData>();
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    // Use this for initialization
    void Start () {
        m_RaycastLayermask = ~((1 << 2) | (1 << 8)); //ignore second and eighth layer
    }
	
	// Update is called once per frame
	void Update () {
        SetRayTargetPos();

        CrosshairOffsetResolutionMultiplier = Screen.width / 720f;

        //Handle Recoil(When moving, spread cross hair)
        m_PlayerData.m_WeaponInhand.m_StackedRecoil = Mathf.Max(m_PlayerData.m_Move.magnitude,
            m_PlayerData.m_WeaponInhand.m_StackedRecoil);
        UpdateCrossHair();
    }

    public void SetRayTargetPos()
    {
        m_RayStartPos = m_Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));   //middle point of screen
        if (Physics.Raycast(m_RayStartPos, m_Camera.transform.forward, out m_RaycastHit, 100f, m_RaycastLayermask))    //raycast forward
        {
            m_RayTarget.position = m_RaycastHit.point;

            if(Vector3.Distance(m_RayStartPos, m_RayTarget.position) < m_TargetMinDistance)
            {
                m_RayTarget.position = m_RayStartPos + m_Camera.transform.forward * m_TargetMinDistance;
            }
        }
        else    //if there is no point where the ray hit, set destination point as moderate forward at camera.
        {
            m_RayTarget.position = m_RayStartPos + m_Camera.transform.forward * 100f;
        }

        m_LookTarget = m_RayStartPos + m_Camera.transform.forward * LookDistance;
    }

    public void UpdateCrossHair()
    {
        float recoil = m_PlayerData.m_WeaponInhand.m_StackedRecoil;

        //Set crossHair position according to magnitude of recoil
        for (int i = 0; i < 4; i++)
        {
            Vector3 CrossHairPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0) 
                + (DirectionSet[i] * m_CrossHairOffSetPos * CrosshairOffsetResolutionMultiplier) + 
                (DirectionSet[i] * recoil * 5.5f * CrosshairOffsetResolutionMultiplier);
            m_CrossHairImages[i].transform.position = CrossHairPos;
        }
    }

    public void RecoilUpward(float _weaponrecoil)
    {
        StopCoroutine("CoRecoilUpward");
        StartCoroutine("CoRecoilUpward", _weaponrecoil);
    }

    public IEnumerator CoRecoilUpward(float _weaponrecoil)
    {
        int count = 0;

        while(true)
        {
            m_PlayerInput.m_Mouse_Y -= _weaponrecoil * UpRecoilMultiplyer * Mathf.Pow(0.5f, count+2);
            if (count >= 10)
                StopCoroutine("CoRecoilUpward");

            count++;
            yield return null;
        }
    }
}
