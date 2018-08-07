using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimSystem))]
public class AimIK : MonoBehaviour {

    public Animator m_Ani;

    public Transform m_LeftHandPosition;
    public Transform m_RightHandPosition;

    // Use this for initialization
    void Awake () {
        m_Ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnAnimatorIK(int layerIndex)
    {
        m_Ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        m_Ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

        m_Ani.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandPosition.position);
        m_Ani.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandPosition.position);


    }
}
