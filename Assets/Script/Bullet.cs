using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    public ObjType m_Type { get; set; }
    public ObjectManager m_ObjMgr { get; set; }
    public string m_ObjName { get; set; }

    public float m_Speed { get; set; }
    public int m_BodyDamage;
    public int m_HeadDamage;
    private float m_StayTime;
    private TrailRenderer m_Trail;

    //these are required to split up because these are used at "sendmessage" method
    public void SetBodyDamage(int _body)
    {
        m_BodyDamage = _body;
    }
    public void SetHeadDamage(int _haed)
    {
        m_HeadDamage = _haed;
    }

    // Use this for initialization
    void Awake()
    {
        m_StayTime = 2f;
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        m_Trail = transform.GetComponent<TrailRenderer>();
    }

    void Start()
    {
        ObjListAdd();
        m_Speed = 30f;
    }

    void OnEnable()
    {
        Invoke("Remove", m_StayTime);   //Remove this after "m_StayTime"
        m_Trail.Clear();
    }

    //proceed
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
    }

    //collision check
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Vector3 CollsionPoint = col.ClosestPointOnBounds(this.transform.position);
            int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
            col.gameObject.SendMessage("GetDamage", DamageSet);
            m_ObjMgr.MakeParticle(CollsionPoint, this.transform.rotation, "FX_BloodSplatter_Bullet");   //Make particle at attack point
            CancelInvoke();
            Remove();
        }

        CancelInvoke();
        Remove();
    }

    void Remove()
    {
        gameObject.SetActive(false);
    }

    public void ObjListAdd()
    {
        m_ObjMgr.Objects.m_Bulletlist.Add(this);
    }
}
