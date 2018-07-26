using System.Collections;
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

    public GameObject m_GameOver;       //gameover picture
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
            m_BulletNum.text = m_Player.m_WeaponInhand.m_AmmoBulletNum.ToString() + "/" + m_Player.m_WeaponInhand.m_MaxBulletNum.ToString();
            m_Hp.text = m_Player.m_Hp.ToString() + "/" + m_Player.m_MaxHp;
            yield return null;
        }
    }

    //Activate "Game over"UI when player die.
    public void ActiveUI(string _str)
    {
        m_GameOver.SetActive(true);
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

        //Set the alpha of image 0
        Color alpha = _imageSeconds.Image.color;
        alpha.a = 0f;
        _imageSeconds.Image.color = alpha;

        while (true)
        {
            m_RedAimTimer += _imageSeconds.Seconds / 10f;

            if (m_RedAimTimer <= _imageSeconds.Seconds / 2.0f)
            {
                alpha.a += 0.2f;
                _imageSeconds.Image.color = alpha;
            }
            else
            {
                alpha.a -= 0.2f;
                _imageSeconds.Image.color = alpha;
            }
        
            if(m_RedAimTimer > _imageSeconds.Seconds)
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