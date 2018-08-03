using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGRocket : MonoBehaviour {
    public ParticleSystem fireTail;

    private bool IsLaunched = false;

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
                if (hit.transform.CompareTag("Floor"))
                {
                    Vector3 CollsionPoint = hit.point;
                    ObjectPoolMgr.instance.CreatePooledObject("ExplosionParticle", CollsionPoint, Quaternion.LookRotation(Vector3.up));
                    var cds = Physics.OverlapSphere(hit.point, 10);

                    try
                    {
                        foreach (Collider cd in cds)
                        {
                            var obj = cd.gameObject;
                            if (obj.tag.Equals("Enemy"))
                            {
                                if (obj.GetComponent<HitboxChecker>() != null && obj.GetComponent<HitboxChecker>().Object != null)
                                {
                                    obj.GetComponent<HitboxChecker>().Object.GetComponent<ZombieData>().GetDamage(250);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    Destroy(gameObject);
                }
            }
        }
	}

    public void SetPosRot(Vector3 pos, Quaternion rot)
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
    }

    public void Fire()
    {
        if (fireTail != null) fireTail.gameObject.SetActive(true);
        IsLaunched = true;
    }
}
