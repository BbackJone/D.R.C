using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlesound : MonoBehaviour {

   public AudioSource AudioSource;
   public AudioClip[] AudioClip;

   public void uisound1()
    {
        AudioSource.PlayOneShot(AudioClip[0]);
    }

    public void uisound2()
    {
        AudioSource.PlayOneShot(AudioClip[1]);
    }

    public void uisound3()
    {
        AudioSource.PlayOneShot(AudioClip[2]);
    }
    public void uisound4()
    {

        AudioSource.PlayOneShot(AudioClip[3]);
    }
    public void uisound5()
    {

   
        AudioSource.PlayOneShot(AudioClip[4]);
    }
}
