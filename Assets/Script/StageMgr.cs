using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    public Transform m_Sun;
    public GameObject[] m_LevelImage;
    private int m_Level = 0;
    public int m_GameTime { get; set; }

    // Use this for initialization
    void Start()
    {
        m_GameTime = 0;
        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            m_GameTime++;
            yield return new WaitForSeconds(1f);
        }
    }
}

