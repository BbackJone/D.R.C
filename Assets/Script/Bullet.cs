using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    public ObjType m_Type { get; set; }

    public float m_Speed;          //Please Set this at editor
    public bool m_Penetration;     //Please Set this at editor
    public float m_StayTime;       //Please Set this at editor

    private int m_BodyDamage;
    private int m_HeadDamage;
    private TrailRenderer m_Trail;

    //Raycast Parameters
    public Vector3 m_PrevPos;

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
        m_Trail = transform.GetComponent<TrailRenderer>();
    }

    void Start()
    {
        ObjListAdd();
    }

    void OnEnable()
    {
        m_PrevPos = transform.position;
        Invoke("Remove", m_StayTime);   //Remove this after "m_StayTime"
        m_Trail.Clear();
    }

    //proceed
    private void FixedUpdate()
    {
        m_PrevPos = transform.position;
        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
        CollisionCheck();
    }

    void CollisionCheck()
    {
        if (m_Penetration == false)
            RayCast_NonPenetration();
        else
            RayCast_Penetration();
    }

    void Remove()
    {
        gameObject.SetActive(false);
    }

    public void ObjListAdd()
    {
        ObjectManager.m_Inst.Objects.m_Bulletlist.Add(this);
    }

    public void RayCast_NonPenetration()
    {
        Vector3 direction = transform.position - m_PrevPos;
        Ray ray = new Ray(m_PrevPos, direction.normalized);
        RaycastHit[] hit;

        hit = Physics.RaycastAll(ray, direction.magnitude);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                Vector3 CollsionPoint = hit[i].point;
                int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
                hit[i].transform.gameObject.SendMessage("GetDamage", DamageSet);
                ObjectPoolMgr.instance.CreatePooledObject("FX_BloodSplatter_Bullet", CollsionPoint, this.transform.rotation);   //Make particle at attack point
                CancelInvoke();
                Remove();
            }
        }
    }

    public void RayCast_Penetration()
    {
        Vector3 direction = transform.position - m_PrevPos;
        Ray ray = new Ray(m_PrevPos, direction.normalized);
        RaycastHit[] hit;

        hit = Physics.RaycastAll(ray, 500f);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                Vector3 CollsionPoint = hit[i].point;
                int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
                hit[i].transform.gameObject.SendMessage("GetDamage", DamageSet);
                ObjectPoolMgr.instance.CreatePooledObject("FX_BloodSplatter_Bullet", CollsionPoint, this.transform.rotation);   //Make particle at attack point

                //Minus 50 Damage per every penetration
                m_HeadDamage -= 50;
                m_BodyDamage -= 50;

                if(m_HeadDamage + m_BodyDamage == 0)
                {
                    CancelInvoke();
                    Remove();
                }
            }
        }
    }
}
