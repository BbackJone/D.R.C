using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class defines a target that is hit by raycast from camera.
public class AimSystem : MonoBehaviour {

    private Camera m_Camera;
    private PlayerData m_PlayerData;

    public Transform m_RayTarget;    //if Raycast is true, this is position of RaycastHit.
                                        //but not, this is position of target moderate forward at camera.
    private Vector3 m_RayStartPos;
    private RaycastHit m_RaycastHit;
    private int m_RaycastLayermask;       //Layer for raycast to ignore

    public Image[] m_CrossHairImages;   //RIght Left Up Down

    private Vector3[] DirectionSet = { new Vector3(1,0,0), new Vector3(-1,0,0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    private float m_CrossHairOffSetPos = 16.6f;

    private void Awake()
    {
        m_Camera = Camera.main;
        m_PlayerData = GetComponent<PlayerData>();
    }

    // Use this for initialization
    void Start () {
        m_RaycastLayermask = ~((1 << 2) | (1 << 8)); //ignore second and eighth layer
    }
	
	// Update is called once per frame
	void Update () {
        SetRayTargetPos();
        UpdateCrossHair();
    }


    public void SetRayTargetPos()
    {
        m_RayStartPos = m_Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));   //middle point of screen
        if (Physics.Raycast(m_RayStartPos, m_Camera.transform.forward, out m_RaycastHit, 100f, m_RaycastLayermask))    //raycast forward
        {
            m_RayTarget.position = m_RaycastHit.point;
        }
        else    //if there is no point where the ray hit, set destination point as moderate forward at camera.
        {
            m_RayTarget.position = m_RayStartPos + m_Camera.transform.forward * 100f;
        }
    }

    public void UpdateCrossHair()
    {
        float recoil = m_PlayerData.m_WeaponInhand.m_StackedRecoil;
        Debug.Log(m_PlayerData.m_WeaponInhand.m_ObjName + "'s Recoil = " + recoil);

        //Set crossHair position according to magnitude of recoil
        for (int i = 0; i < 4; i++)
        {
            Vector3 CrossHairPos = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0) + (DirectionSet[i] * m_CrossHairOffSetPos) + (DirectionSet[i] * recoil * 5.5f);
            m_CrossHairImages[i].transform.position = CrossHairPos;
        }
    }
}
