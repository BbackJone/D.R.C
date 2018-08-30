using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
    private GameObject currentTarget;
    private const float fireTimeConst = 0.5f;
    private float fireTime = fireTimeConst;

	void OnEnable() {
        StartCoroutine(RunTurret());
	}
    
    IEnumerator RunTurret() {
        while (true) {
            Debug.Log("TurretScript: RunTurret loop started");

            // find target
            List<Transform> activeEnemies = new List<Transform>();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject o in enemies) {
                if (o.activeSelf && o.GetComponent<ZombieData>() != null) activeEnemies.Add(o.transform);
            }
            if (activeEnemies.Count == 0) {
                Debug.Log("TurretScript: couldn't found attackable target, will repeat after 1s");
                yield return new WaitForSeconds(1f);
                continue;
            }
            Transform closestEnemy = GetClosestEnemy(activeEnemies.ToArray());
            ZombieData zombieData = closestEnemy.GetComponent<ZombieData>();
            Debug.Log("TurretScript: Target found! " + closestEnemy.gameObject.name);

            // attack target until it dies
            while (zombieData.m_Hp != 0) {
                Debug.Log("TurretScript: Targeted Zombie HP: " + zombieData.m_Hp);
                transform.LookAt(new Vector3(closestEnemy.position.x, Mathf.Max(closestEnemy.position.y, 1), closestEnemy.position.z));

                fireTime -= Time.deltaTime;

                if (fireTime < 0f) {
                    GameObject bullet = ObjectPoolMgr.instance.CreatePooledObject("Handgun_Bullet", transform.position, transform.rotation);
                    bullet.SendMessage("SetBodyDamage", 20);
                    bullet.SendMessage("SetHeadDamage", 40);

                    fireTime += fireTimeConst;
                }

                yield return new WaitForEndOfFrame();
            }

            Debug.Log("TurretScript: RunTurret loop finished");

            yield return new WaitForSeconds(1f);
        }
    }

    Transform GetClosestEnemy(Transform[] enemies) {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies) {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist) {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
}
