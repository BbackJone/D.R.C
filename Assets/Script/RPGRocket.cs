using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGRocket : MonoBehaviour {
    public ParticleSystem fireTail;

    private bool IsLaunched = false;
    private float m_StayTime = 2f;

    public int m_BodyDamage;
    public int m_HeadDamage;

    public void Remove()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate () {
		if (IsLaunched)
        {
            var m_PrevPos = transform.position;

            transform.Translate(Vector3.forward);
            fireTail.transform.position = transform.position;

            Vector3 direction = transform.position - m_PrevPos;
            Ray ray = new Ray(m_PrevPos, direction.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, direction.magnitude))
            {
                if (hit.transform.CompareTag("Floor") || hit.transform.CompareTag("Enemy"))
                {
                    Vector3 CollsionPoint = hit.point;
                    ObjectPoolMgr.instance.CreatePooledObject("ExplosionParticle", CollsionPoint, Quaternion.LookRotation(Vector3.up));
                    var cds = Physics.OverlapSphere(hit.point, 10);

                    foreach (Collider cd in cds)
                    {
                        var obj = cd.gameObject;
                        if (obj.tag.Equals("Enemy"))
                        {
                            int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
                            obj.SendMessage("GetDamage", DamageSet);
                        }
                    }

                    CancelInvoke();
                    Remove();
                }
            }
        }
	}

    public void Fire()
    {
        if (fireTail != null) fireTail.gameObject.SetActive(true);
        IsLaunched = true;

        Invoke("Remove", m_StayTime);
    }
}
