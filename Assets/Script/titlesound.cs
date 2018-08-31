using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlesound : MonoBehaviour {

   public AudioSource AudioSource;
   public AudioClip[] AudioClip;
    private float sevol;

    private void Update()
    {
        sevol = PlayerPrefs.GetFloat("sevol");
    }
    
    

    public void uisound1()
    {
        AudioSource.PlayOneShot(AudioClip[0],sevol);
    }

    public void uisound2()
    {
        AudioSource.PlayOneShot(AudioClip[1],sevol);
    }

    public void uisound3()
    {
        AudioSource.PlayOneShot(AudioClip[2],sevol);
    }
    public void uisound4()
    {

        AudioSource.PlayOneShot(AudioClip[3],sevol);
    }
    public void uisound5()
    {

   
        AudioSource.PlayOneShot(AudioClip[4],sevol);
    }
}
