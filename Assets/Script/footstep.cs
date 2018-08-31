using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstep : MonoBehaviour {
    public AudioSource FstepSourse;

    private float sevol;
    public AudioClip[] FstepClip;

    private Transform leftFootTrans;
    private Transform rightFootTrans;

    private void Awake()
    {
        
        leftFootTrans = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFootTrans = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightFoot);
    }
    private void Update()
    {
        sevol = PlayerPrefs.GetFloat("sevol");
    }

    void LeftFootstep()
    {
        AudioClip clip = null;
        float maxvol = 1.0f;
        RaycastHit hit;
        if(Physics.Raycast(leftFootTrans.position,-Vector3.up,out hit, 1))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                clip = FstepClip[0];
                maxvol = UnityEngine.Random.Range(0.4f*sevol, 0.9f*sevol);
            }
            else if (hit.collider.CompareTag("Sand"))
            {
                clip = FstepClip[1];
                maxvol = UnityEngine.Random.Range(0.4f*sevol, 0.9f*sevol);
            }
        }
        if (clip != null)
        {
            FstepSourse.PlayOneShot(clip, maxvol);
        }
    }

    void RightFootstep()
    {
        AudioClip clip = null;
        float maxvol = 1.0f;
        RaycastHit hit;
        if (Physics.Raycast(rightFootTrans.position, -Vector3.up, out hit, 1))
        {
            if (hit.collider.CompareTag("Floor"))
            {
                clip = FstepClip[0];
                maxvol = UnityEngine.Random.Range(0.4f*sevol, 0.9f*sevol);
            }
            else if (hit.collider.CompareTag("Sand"))
            {
                clip = FstepClip[1];
                maxvol = UnityEngine.Random.Range(0.4f*sevol, 0.9f*sevol);
            }
        }
        if (clip != null)
        {
            FstepSourse.PlayOneShot(clip, maxvol);
        }
    }
}
