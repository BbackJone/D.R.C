using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titlesound : MonoBehaviour {

   public AudioSource AudioSource;
   public AudioClip[] AudioClip;

    public void uisound1()
    {
        AudioSource.PlayOneShot(AudioClip[0],VolumeHolderScript.instance.seVol);
    }

    public void uisound2()
    {
        AudioSource.PlayOneShot(AudioClip[1], VolumeHolderScript.instance.seVol);
    }

    public void uisound3()
    {
        AudioSource.PlayOneShot(AudioClip[2], VolumeHolderScript.instance.seVol);
    }
    public void uisound4()
    {
        AudioSource.PlayOneShot(AudioClip[3], VolumeHolderScript.instance.seVol);
    }
    public void uisound5()
    {
        AudioSource.PlayOneShot(AudioClip[4], VolumeHolderScript.instance.seVol);
    }
}
