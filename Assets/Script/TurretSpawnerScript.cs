using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSpawnerScript : MonoBehaviour {
    public Text chargeRateText;

    public int chargeRate = 100;

    private void Start()
    {
        StartCoroutine("AutoCharge");
    }

    IEnumerator AutoCharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            Charge(1);
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
        chargeRate = 0;
        var newTurret = ObjectPoolMgr.instance.CreatePooledObject("TurretBody", position.position, position.rotation);
        newTurret.SetActive(true);
    }
}
