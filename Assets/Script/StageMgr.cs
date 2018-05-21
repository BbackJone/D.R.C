using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    public Transform m_Sun;
    public GameObject[] m_LevelImage;
    private bool m_DayCheck = true;
    private int m_Level = 0;
    public int m_GameTime { get; set; }

    // Use this for initialization
    void Start()
    {
        m_GameTime = 0;
        StartCoroutine(LevelCheck());
        StartCoroutine(GameTimer());
    }

    // Update is called once per frame

    IEnumerator LevelCheck()
    {
        while (true)
        {
            if (m_Sun.position.y < 0 && m_DayCheck == true)
            {
                m_DayCheck = false;
                m_Level++;
                m_LevelImage[m_Level - 1].SetActive(true);
            }
            else if (m_Sun.position.y >= 0 && m_DayCheck == false)
            {
                m_DayCheck = true;
                m_Level++;
                m_LevelImage[m_Level - 1].SetActive(true);
            }
            yield return new WaitForSeconds(1f);
        }
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

