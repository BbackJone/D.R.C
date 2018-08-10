using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkByfootcollider : MonoBehaviour {

    private SoundPlayer m_SoundPlayer;

        public AudioClip SandSound;

        public AudioClip WalkSound;

    private void Awake()
    {
        m_SoundPlayer = transform.root.GetComponent<SoundPlayer>();
    }

    void OnTriggerEnter(Collider col)
        {
        if (col.gameObject.CompareTag("Sand"))
        {
            m_SoundPlayer.PlaySound(SOUNDCLIP.SANDWALK);
            Debug.Log("Sand");
        }
        if (col.gameObject.CompareTag("Floor"))
        {
            m_SoundPlayer.PlaySound(SOUNDCLIP.ASPHALTWALK);
            Debug.Log("Floor");
        }


        }

}

 
