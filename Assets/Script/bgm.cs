using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

   public AudioClip bgmClip;
   public AudioSource bgmSource;

    private float musicvol;
    private void Start()
    {
        StartCoroutine("Bgmplay");
        
    }
    private void Update()
    {
        musicvol = PlayerPrefs.GetFloat("musicvol");
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

