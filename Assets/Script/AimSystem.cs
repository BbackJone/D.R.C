using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class defines a target that is hit by raycast from camera.
public class AimSystem : MonoBehaviour {

    private Camera m_Camera;

    public Transform m_RayTarget;    //if Raycast is true, this is position of RaycastHit.
                                        //but not, this is position of target moderate forward at camera.
    private Vector3 m_RayStartPos;
    private RaycastHit m_RaycastHit;

    private int m_RaycastLayermask;       //Layer for raycast to ignore

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    // Use this for initialization
    void Start () {
        m_RaycastLayermask = ~((1 << 2) | (1 << 8)); //ignore second and eighth layer
    }
	
	// Update is called once per frame
	void Update () {
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
}
