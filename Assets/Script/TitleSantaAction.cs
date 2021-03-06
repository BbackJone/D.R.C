﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSantaAction : MonoBehaviour {

    private PlayerData m_Data;
    private CameraMove m_CameraMove;
    private Animator m_Ani;

    public Transform Look_target;

	// Use this for initialization
    void Awake()
    {
        m_Data = GetComponent<PlayerData>();
        m_CameraMove = Camera.main.GetComponent<CameraMove>();
        m_Ani = GetComponent<Animator>();
    }

    void OnEnable()
    {
        StartCoroutine("Playermove");
    }
    
    IEnumerator Playermove()
    {
        while (true)
        {
            transform.Translate(Vector3.forward * m_Data.m_Move.z * Time.deltaTime * m_Data.m_Speed);
            transform.Translate(Vector3.right * m_Data.m_Move.x * Time.deltaTime * m_Data.m_Speed);

            m_Ani.SetFloat("Speed", m_Data.m_Move.z);
            yield return null;
        }
    }
}