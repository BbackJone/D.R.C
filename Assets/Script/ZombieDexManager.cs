using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZombieDexManager : MonoBehaviour {
    public ZombieDexCameraMover cameraMover;

    string[] zombieDexIndices = { "NormalZombies", "SoldierZombie", "RugbyZombie", "PrisonerZombie", "DevilZombie" };
    public ZombieDexSpinner[] zombieDexSpinners;

    int idx = 0;

    public Text zombieName;
    public Text zombieDesc;

    public LanguageUpdater lu;

    void Awake () {
        zombieDexSpinners = new ZombieDexSpinner[zombieDexIndices.Length];
        for (int i = 0; i < zombieDexIndices.Length; i++)
        {
            zombieDexSpinners[i] = transform.GetChild(i).GetComponent<ZombieDexSpinner>();
        }
        SetDexTarget();
    }

    public void IncreaseTargetIdx()
    {
        idx++;
        if (idx > zombieDexIndices.Length - 1) idx = 0;
    }

    public void DecreaseTargetIdx()
    {
        idx--;
        if (idx < 0) idx = zombieDexIndices.Length - 1;
    }

    public void SetDexTarget()
    {
        cameraMover.target = zombieDexSpinners[idx].camPos;
        StopCoroutine(UpdateDexText());
        StartCoroutine(UpdateDexText());
    }

    IEnumerator UpdateDexText()
    {
        while (lu == null)
        {
            yield return new WaitForEndOfFrame();
        }

        while (lu.lm == null)
        {
            yield return new WaitForEndOfFrame();
        }

        while (!lu.lm.IsLoaded)
        {
            yield return new WaitForEndOfFrame();
        }

        zombieName.text = lu.lm.Get("zombiedex_" + zombieDexIndices[idx]);
        zombieDesc.text = lu.lm.Get("zombiedex_" + zombieDexIndices[idx] + "_desc");

        yield return new WaitForEndOfFrame();
    }

    public void QuitToTitle()
    {
        SceneManager.LoadScene("Menu");
    }
}
