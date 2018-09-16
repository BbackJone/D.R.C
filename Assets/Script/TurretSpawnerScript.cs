using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSpawnerScript : MonoBehaviour {
    public Text chargeRateText;
    public AudioClip AudioClip;
    public AudioSource AudioSource;

    SaveDataManager sdm;

    public List<Vector3> spawnedTurrets;

    public int chargeRate = 100;

    private void Start()
    {
        spawnedTurrets = new List<Vector3>();
        StartCoroutine(AutoCharge());
        SpawnSavedTurrets();
    }

    IEnumerator AutoCharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Charge(1);
        }
    }

    void SpawnSavedTurrets() {
        sdm = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        
        if (sdm.currentSaveData != null) {
            if (sdm.currentSaveData.turretPos != null) {
                foreach (Vector3 v in sdm.currentSaveData.turretPos) {
                    SpawnTurretAtPosition(v);
                }
            }
        }
    }

    void Update()
    {
        if (chargeRate != 100) chargeRateText.text = string.Format("{0}%", chargeRate);
        else chargeRateText.text = string.Empty;
    }

    public void Charge(int amount)
    {
        chargeRate += amount;
        if (chargeRate > 100) chargeRate = 100;
    }

    public void SpawnTurretAt(Transform position)
    {
        if (chargeRate < 100) return;
        AudioSource.PlayOneShot(AudioClip);
        chargeRate = 0;
        var newTurret = ObjectPoolMgr.instance.CreatePooledObject("TurretBody", position.position, position.rotation);
        spawnedTurrets.Add(position.position);
    }
    
    public void SpawnTurretAtPosition(Vector3 position) {
        var newTurret = ObjectPoolMgr.instance.CreatePooledObject("TurretBody", new Vector3(position.x, position.y, position.z), transform.rotation);
        spawnedTurrets.Add(position);
    }
}
