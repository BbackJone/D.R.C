using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamage : MonoBehaviour {

    private SphereCollider m_SpereCol;

    //Plase Set these variables at unity editor
    public int m_Damage;
    public float m_PushingForce;

    private GameObject soundObject;

	// Use this for initialization
	void Awake () {
        soundObject = ObjectManager.m_Inst.m_Player.gameObject;
        m_SpereCol = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        m_SpereCol.enabled = true;
        soundObject.SendMessage("PlaySound", 11);
        Invoke("DisableCollider", 0.2f);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            float RadiusOfExplode = m_SpereCol.radius;
            float DistanceWithPlayer = Vector3.Distance(other.transform.position, transform.position);

            //form of y = a(x - b)^2   =>     Damage following distance
            int Damage = (int)((m_Damage / (Mathf.Pow(RadiusOfExplode, 2))) *
                Mathf.Pow((DistanceWithPlayer - RadiusOfExplode), 2));
            other.gameObject.SendMessage("GetDamage", 1);

            //Make normalized vector that is direction of forcing player.
            Vector3 ForceDirection = other.transform.position - transform.position;
            ForceDirection += Vector3.up;
            ForceDirection = ForceDirection.normalized;

            //form of y = -ax^2 + b     =>     pushingforce following distance
            int force = (int)(-(DistanceWithPlayer / (2 * m_PushingForce))
                + m_PushingForce);
            other.gameObject.GetComponent<Rigidbody>().AddForce(ForceDirection * force, ForceMode.Impulse);
        }
    }

    public void DisableCollider()
    {
        m_SpereCol.enabled = false;
    }
}
