using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkByfootcollider : MonoBehaviour {

    

        public AudioClip SandSound;

        public AudioClip WalkSound;



        void OnTriggerEnter(Collider col)
        {

            if (col.gameObject.layer == LayerMask.NameToLayer("Sand"))

            gameObject.SendMessage("PlaySound", 2);


        if (col.gameObject.layer == LayerMask.NameToLayer("Floor"))

            gameObject.SendMessage("PlaySound", 3);


    }

}

 
