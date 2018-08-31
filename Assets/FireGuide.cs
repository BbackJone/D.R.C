using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireGuide : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Image image;
    private Color originalColor;
    private float alpha;

    public bool guideCounter = false;

    private PlayerData playerData;
    public FireButtonScript fireButton;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        alpha = originalColor.a;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("GuidedCount", 0) < 3) StartCoroutine("FadeGuide");
        else image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        playerData = ObjectManager.m_Inst.m_Player.GetComponent<PlayerData>();
    }

    IEnumerator FadeGuide() {
        yield return new WaitForSeconds(1f);

        while (alpha > 0)
        {
            alpha -= 0.005f;
            if (alpha < 0) alpha = 0;
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return new WaitForFixedUpdate();
        }

        if (guideCounter)
        {
            PlayerPrefs.SetInt("GuidedCount", PlayerPrefs.GetInt("GuidedCount", 0) + 1);
            PlayerPrefs.Save();
        }
        StopCoroutine("FadeGuide");
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerData.m_WeaponInhand.m_Autoshot)
        {
            playerData.m_isShooting = true;
        }
        else
        {
            StartCoroutine(SingleShot());
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if(!fireButton.autofireByFireButton)
            playerData.m_isShooting = false;
    }

    IEnumerator SingleShot()
    {
        playerData.m_isShooting = true;
        yield return new WaitForEndOfFrame();
        playerData.m_isShooting = false;
    }
}
