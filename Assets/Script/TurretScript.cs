using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
    //private List<Transform> enemiesInRange = new List<Transform>();
    private const float fireTimeConst = 0.5f;
    private float fireTime = fireTimeConst;

    private Transform muzzleFlash;
    public AudioClip AudioClip;
    public AudioSource AudioSource;


    void OnEnable() {
        muzzleFlash = transform.GetChild(1);
        StartCoroutine(RunTurret());
	}

    

    IEnumerator RunTurret() {
        while (true) {
            // find target
            List<Transform> activeEnemies = new List<Transform>();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject o in enemies) {
                if (o.activeSelf && o.gameObject.name.Contains("Zombie")) activeEnemies.Add(o.transform);
            }
            if (activeEnemies.Count == 0) {
                yield return new WaitForSeconds(1f);
                continue;
            }
            Transform closestEnemy = GetClosestEnemy(activeEnemies.ToArray(), 26f);
            if (closestEnemy == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            ZombieData zombieData = closestEnemy.GetComponent<ZombieData>();
            if (zombieData == null)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // attack target until it dies
            while (zombieData.m_Hp != 0)
            {
                transform.LookAt(new Vector3(closestEnemy.position.x, Mathf.Max(1f, closestEnemy.position.y), closestEnemy.position.z));

                fireTime -= Time.deltaTime;

                if (fireTime < 0f)
                {
                    GameObject bullet = ObjectPoolMgr.instance.CreatePooledObject("Handgun_Bullet", transform.position, transform.rotation);
                    bullet.SendMessage("SetBodyDamage", 20);
                    bullet.SendMessage("SetHeadDamage", 40);
                    AudioSource.PlayOneShot(AudioClip, VolumeHolderScript.instance.seVol);

                    muzzleFlash.gameObject.SetActive(true);
                    Invoke("TurnOffMuzzleFlash", 0.05f);

                    fireTime += fireTimeConst;
                }

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    Transform GetClosestEnemy(Transform[] enemies, float maxDistance) {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies) {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist && dist < maxDistance) {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    void TurnOffMuzzleFlash()
    {
        muzzleFlash.gameObject.SetActive(false);
    }
}
