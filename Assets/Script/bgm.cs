using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

   public AudioClip bgmClip;
   public AudioSource bgmSource;

    private float musicvol;
    private void Start()
    {
        UpdateBgmVolume();
        StartCoroutine("Bgmplay");
    }
    private void Update()
    {
        if (Time.timeScale == 0f) UpdateBgmVolume();
    }

    void UpdateBgmVolume()
    {
        musicvol = VolumeHolderScript.instance.musicVol;
        bgmSource.volume = musicvol;
    }

    IEnumerator Bgmplay()
    {
        while (true)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.PlayOneShot(bgmClip);
            }
            yield return new WaitForSeconds(2);
        }
    }
}

