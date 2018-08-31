using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireGuide : MonoBehaviour {

    private Image image;
    private Color originalColor;
    private float alpha;

    public bool guideCounter = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
        alpha = originalColor.a;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("GuidedCount", 0) < 3) StartCoroutine(FadeGuide());
        else gameObject.SetActive(false);
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
        gameObject.SetActive(false);
	}
}
