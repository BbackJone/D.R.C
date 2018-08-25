using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireButtonScript : MonoBehaviour, IPointerDownHandler {
    private PlayerData playerData;
    private Image image;
    private Image inImage;
    private float imageColor = 1f;
    private const float imageColorBright = 1f;
    private const float imageColorDark = 0.4f;
    private bool autofireByFireButton = false;

    void Start() {
        playerData = ObjectManager.m_Inst.Objects.m_Playerlist[0].GetComponent<PlayerData>();
        if (playerData == null)
            enabled = false;

        image = GetComponent<Image>();
        inImage = transform.Find("Image").gameObject.GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (playerData.m_WeaponInhand.m_Autoshot)
        {
            playerData.m_isShooting = !playerData.m_isShooting;
            autofireByFireButton = playerData.m_isShooting;
            imageColor = playerData.m_isShooting ? imageColorDark : imageColorBright;
            image.color = new Color(imageColor, imageColor, imageColor);
            inImage.color = new Color(imageColor, imageColor, imageColor);
        }
        else
        {
            StartCoroutine(SingleShot());
        }
    }

    IEnumerator SingleShot()
    {
        playerData.m_isShooting = true;
        yield return new WaitForEndOfFrame();
        playerData.m_isShooting = false;
    }

    public void StopAutofireByFireButton()
    {
        if (autofireByFireButton)
        {
            playerData.m_isShooting = false;
            autofireByFireButton = false;
            imageColor = imageColorBright;
            image.color = new Color(imageColor, imageColor, imageColor);
            inImage.color = new Color(imageColor, imageColor, imageColor);
        }
    }
}
