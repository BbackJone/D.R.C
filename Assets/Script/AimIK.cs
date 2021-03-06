﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimSystem))]
public class AimIK : MonoBehaviour {

    //Other component
    private Animator m_Ani;
    private AimSystem m_Aimsystem;
    private PlayerData m_PlayerData;

    private Transform m_AimTarget;
    public Transform m_AimPivot;       //This will make hands up and down according to direction player is looking.
    private Transform m_RightShoulder;

    //Grab Points
    public Transform m_LeftHandPosition;
    public Transform m_RightHandPosition;
    public float m_LeftHandIKWeight = 1f;
    public float m_RightHandIKWeight = 1f;

    public float m_LookWeight = 1;
    public float m_BodyWeight = 0.9f;
    public float m_HeadWeight = 1;
    public float m_EyesWeight = 1;
    public float m_ClampWeight = 1;

    // Use this for initialization
    void Awake () {
        m_Ani = GetComponent<Animator>();
        m_Aimsystem = GetComponent<AimSystem>();
        m_AimTarget = m_Aimsystem.m_RayTarget;
        m_RightShoulder = m_Ani.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
        m_PlayerData = GetComponent<PlayerData>();
    }

    private void OnAnimatorMove()
    {
        SetAimpivotPosition();
        SetAimpivotRotation();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        m_LeftHandIKWeight = 1 - m_Ani.GetFloat("LeftHandIKWeight");
        m_RightHandIKWeight = 1 - m_Ani.GetFloat("RightHandIKWeight");

        //Hand IK
        if (m_LeftHandPosition)
        {
            m_Ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, m_LeftHandIKWeight);
            m_Ani.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandPosition.position);
            m_Ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, m_LeftHandIKWeight);
            m_Ani.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftHandPosition.rotation);
        }
        
        if (m_RightHandPosition)
        {
            m_Ani.SetIKPositionWeight(AvatarIKGoal.RightHand, m_RightHandIKWeight);
            m_Ani.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandPosition.position);
            m_Ani.SetIKRotationWeight(AvatarIKGoal.RightHand, m_RightHandIKWeight);
            m_Ani.SetIKRotation(AvatarIKGoal.RightHand, m_RightHandPosition.rotation);
        }

        //Look IK
        m_Ani.SetLookAtPosition(m_Aimsystem.m_LookTarget);
        m_Ani.SetLookAtWeight(m_LookWeight, m_BodyWeight, m_HeadWeight, m_EyesWeight ,m_ClampWeight);
    }

    public void SetHandsIKPosition(Transform _righthand, Transform _lefthand)
    {
        m_LeftHandPosition = _lefthand;
        m_RightHandPosition = _righthand;
    }

    public void SetAimpivotPosition()
    {
        m_AimPivot.position = m_RightShoulder.position;
    }

    public void SetAimpivotRotation()
    {
        Vector3 DestDir = m_Aimsystem.m_LookTarget - m_AimPivot.position;
        Quaternion TargetRotation = Quaternion.LookRotation(DestDir);

        m_AimPivot.rotation = Quaternion.Lerp(m_AimPivot.rotation, TargetRotation, 0.4f);
    }
}
