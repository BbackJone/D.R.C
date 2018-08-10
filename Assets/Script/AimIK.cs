using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimSystem))]
public class AimIK : MonoBehaviour {

    private Animator m_Ani;
    private AimSystem m_Aimsystem;
    private Transform m_AimTarget;
    private Weapon m_WeaponInhand;

    public Transform m_LeftHandPosition;
    public Transform m_RightHandPosition;

    private Transform m_ChestTransform;

    // Use this for initialization
    void Awake () {
        m_Ani = GetComponent<Animator>();
        m_Aimsystem = GetComponent<AimSystem>();
        m_AimTarget = m_Aimsystem.m_RayTarget;
        m_WeaponInhand = GetComponent<PlayerData>().m_WeaponInhand;
	}

    private void Start()
    {
        m_ChestTransform = m_Ani.GetBoneTransform(HumanBodyBones.Chest);
    }

    private void Update()
    {
        //Vector3 DestDirection = m_AimTarget.position - m_ChestTransform.position;
        //m_WeaponInhand.transform.rotation = Quaternion.FromToRotation(m_WeaponInhand.transform.forward, DestDirection);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        m_Ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        m_Ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

        m_Ani.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandPosition.position);
        m_Ani.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandPosition.position);
    }
}
