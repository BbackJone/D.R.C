using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeHolderScript : MonoBehaviour {
    public static VolumeHolderScript instance;

    public float musicVol;
    public float seVol;

    private void Awake()
    {
        instance = this;
        UpdateVolume();
    }
    
    public void UpdateVolume()
    {
        musicVol = PlayerPrefs.GetFloat("musicvol", 1f);
        seVol = PlayerPrefs.GetFloat("sevol", 1f);
    }
}
