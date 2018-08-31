using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Katana : Weapon
{
    private string m_ParticleName;
    private Animator m_PlayerAni;

    private BoxCollider m_BoxCol;

    private Vector3[] m_CurrentPos = new Vector3[7];
    private Vector3 m_SwingingDirection;        //Direction to make blood direction
    private GameObject soundObject;
    public override void Shoot()
    {
        m_PlayerAni.SetTrigger("KatanaAttack");
        Invoke("BoxColOn", 0.75f * 0.3f);
        Invoke("BoxColOff", 0.75f * 0.9f);
        StartCoroutine("SetProceedDirection");

        Invoke("PlayKatanaSound", 0.3f);
        
    }

    // Use this for initialization
    void Awake(){
        soundObject = GameObject.Find("Santa");

        m_AimSystem = GetComponentInParent<AimSystem>();
        m_PlayerAni = GetComponentInParent<Animator>();
        m_BoxCol = GetComponent<BoxCollider>();
        for (int i = 0; i < 7; i++)
        {
            m_CurrentPos[i] = Vector3.zero;
        }
        Initialize();
    }

    private void OnEnable()
    {
        StartCoroutine("NarrowDownAim");
    }

    void Start () {
        m_ObjName = "Katana";
        m_ParticleName = "FX_BloodSplatter_Katana";
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            Vector3 CollsionPoint = col.ClosestPointOnBounds(this.transform.position);
            int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
            col.gameObject.SendMessage("GetDamage", DamageSet);
            ObjectPoolMgr.instance.CreatePooledObject(m_ParticleName, CollsionPoint, Quaternion.LookRotation(m_SwingingDirection));
        }
    }

    public void BoxColOn()
    {
        m_BoxCol.enabled = true;
    }

    public void BoxColOff()
    {
        m_BoxCol.enabled = false;
    }

    public IEnumerator SetProceedDirection()
    {
        Invoke("CancelSetProceedDirection", 0.9f);
        int LoopCount = 0;
        while (true)
        {
            m_CurrentPos[LoopCount] = transform.position;
            int PrevIndex = Mathf.Max(0, LoopCount - 1);
            m_SwingingDirection = m_CurrentPos[LoopCount] - m_CurrentPos[PrevIndex];
            LoopCount++;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void CancelSetProceedDirection()
    {
        StopCoroutine("SetProceedDirection");
        for (int i = 0; i < 7; i++)
        {
            m_CurrentPos[i] = Vector3.zero;
        }
    }

    public void PlayKatanaSound()
    {
        soundObject.SendMessage("PlaySound", 14, 0);
    }
}
