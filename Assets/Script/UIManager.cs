﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct ImageSeconds      //This is used at "ShowImageForseconds" Coroutine
{
    public Image Image;
    public float Seconds;
}

public class UIManager : MonoBehaviour {

    public static UIManager m_Instance;

    private float m_SceneTimer;

    public PlayerData m_Player;
    private Canvas m_Canvas;        //the place system draws user interfaces

    public Image m_RedAim;         //Redaim picture
    public Text m_BulletNum;
    public Text m_Hp;

    private float m_RedAimTimer = 0f;

	// Use this for initialization
    void Awake()
    {
        m_Instance = this;
        m_Canvas = FindObjectOfType<Canvas>();
    }

	void Start () {
        m_SceneTimer = 0f;
        StartCoroutine("ShowPlayerData");
    }

    //Get player's bullet remained and HP.
    IEnumerator ShowPlayerData()
    {
        while(true)
        {
            m_BulletNum.text = string.Format("{0} / {1}", m_Player.m_WeaponInhand.m_AmmoBulletNum, m_Player.m_WeaponInhand.m_MaxBulletNum);
            m_Hp.text = string.Format("{0} / {1}", m_Player.m_Hp, m_Player.m_MaxHp);
            yield return null;
        }
    }

    //change scene
    public void NextScene(string _Str)
    {
        SceneManager.LoadScene(_Str);
    }

    //change scene with delay
    public IEnumerator NextScene(string _Str, float _delay)
    {
        while (true)
        {
            if (m_SceneTimer < _delay)
            {
                m_SceneTimer += Time.deltaTime;
            }
            else
            {
                m_SceneTimer = 0f;
                SceneManager.LoadScene(_Str);
                StopCoroutine("NextScene");
            }

            yield return null;
        }
    }

    public IEnumerator CoShowImageforSeconds(ImageSeconds _imageSeconds)
    {
        _imageSeconds.Image.gameObject.SetActive(true);
        yield return new WaitForSeconds(_imageSeconds.Seconds);
        _imageSeconds.Image.gameObject.SetActive(false);
    }

    //It is used to call Coroutine in other script
    public void ShowImageForSeconds(ImageSeconds _imageSeconds)
    {
        StartCoroutine("CoShowImageforSeconds", _imageSeconds);
    }


    public IEnumerator CoShowImageforSeconds_OpacityChange(ImageSeconds _imageSeconds)
    {
        _imageSeconds.Image.gameObject.SetActive(true);
        m_RedAimTimer = 0;

        //Set the alpha of image 1
        Color alpha = _imageSeconds.Image.color;
        alpha.a = 1f;
        _imageSeconds.Image.color = alpha;

        while (true)
        {
            m_RedAimTimer += _imageSeconds.Seconds / 10f;

            alpha.a -= 0.1f;
            _imageSeconds.Image.color = alpha;

            if (m_RedAimTimer > _imageSeconds.Seconds)
            {
                _imageSeconds.Image.gameObject.SetActive(false);
                m_RedAimTimer = 0;
                StopCoroutine("CoShowImageforSeconds_OpacityChange");
            }
        
            yield return new WaitForSeconds(_imageSeconds.Seconds / 10f);
        }
    }

    //It is used to call Coroutine in other script
    public void ShowImageForSeconds_OpacityChange(ImageSeconds _imageSeconds)
    {
        StartCoroutine("ShowImageForSeconds_OpacityChange", _imageSeconds);
    }

    public void ShowRedAim()
    {
        ImageSeconds Temp = new ImageSeconds();
        Temp.Image = m_RedAim;
        Temp.Seconds = 0.1f;
        StopCoroutine("CoShowImageforSeconds_OpacityChange");
        StartCoroutine("CoShowImageforSeconds_OpacityChange", Temp);
    }
}