using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    public ObjType m_Type { get; set; }
    public ObjectManager m_ObjMgr { get; set; }
    public string m_ObjName { get; set; }

    public float m_Speed { get; set; }
    public int m_AttackDamage { get; set; }
    private float m_StayTime;
    private TrailRenderer m_Trail;

    
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
        m_AttackDamage = 10;
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
        Vector3 CollsionPoint = col.ClosestPointOnBounds(this.transform.position);

        if (col.CompareTag("Enemy"))
        {
            m_ObjMgr.DamageObj(ObjType.OBJ_ENEMY, col.transform, m_AttackDamage);
        }
        else if (col.CompareTag("Player"))
        {
            m_ObjMgr.DamageObj(ObjType.OBJ_PLAYER, col.transform, m_AttackDamage);
        }

        m_ObjMgr.MakeParticle(CollsionPoint, this.transform.rotation, "FX_BloodSplatter_Bullet");   //Make particle at attack point
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
