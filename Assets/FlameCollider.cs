using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCollider : MonoBehaviour {

    private float startRadius = 0.5f;
    private float endRadius = 5f;
    private float stayTime = 1.8f;
    private float speed = 10f;      //10 per sec

    private void OnEnable()
    {
        Invoke("DisableCollider", stayTime);
        StartCoroutine("ProceedCol");
        transform.localScale = new Vector3(startRadius, startRadius, startRadius);
    }

    IEnumerator ProceedCol()
    {
        float time = 0f;
        while (true)
        {
            transform.Translate(Vector3.forward * speed * 0.1f);
            float mediumScale = 2.5f * time + 0.5f;
            transform.localScale = new Vector3(mediumScale, mediumScale, mediumScale);
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DisableCollider()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            int[] DamageSet = new int[2] { 3, 3 };
            other.gameObject.SendMessage("GetDamage", DamageSet);
            Debug.Log("OnTriggerStay");
        }
    }
}
