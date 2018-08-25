using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

   public AudioClip bgmClip;
   public AudioSource bgmSource;

    private void Start()
    {
        StartCoroutine("Bgmplay");
    }

    

    IEnumerator Bgmplay()
    {
        
        while (true)
        {
           
            if (!bgmSource.isPlaying)
            {

                bgmSource.PlayOneShot(bgmClip,0.0f);
            }
            yield return new WaitForSeconds(2);
        }
    }
}

