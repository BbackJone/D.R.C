using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour{

    public ObjType m_Type { get; set; }

    private float m_Speed;     
    private bool m_Penetration;
    private float m_StayTime;  

    private int m_BodyDamage;
    private int m_HeadDamage;

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
    }

    void Start()
    {
        m_Speed = 3000f;
        m_StayTime = 0.5f;
        if (gameObject.name == "Sniper_Bullet")
            m_Penetration = true;

        ObjListAdd();
    }

    void OnEnable()
    {
        m_PrevPos = transform.position;
        Invoke("Remove", m_StayTime);   //Remove this after "m_StayTime"
        StartCoroutine("CoCollisionCheck");
    }

    //proceed and Check Collision
    private void FixedUpdate()
    {
        m_PrevPos = transform.position;

        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);
    }

    public IEnumerator CoCollisionCheck()
    {
        while (true)
        {
            //We have to wait for unity to render a bullet trail before it destroyed.
            yield return new WaitForEndOfFrame();

            CollisionCheck();
        }
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
            else if (hit[i].transform.CompareTag("Floor"))
            {
                Vector3 CollsionPoint = hit[i].point;
                ObjectPoolMgr.instance.MakeParticle("WFXMR_BImpact Concrete + Hole Unlit", CollsionPoint, this.transform.rotation);   //Make particle at attack point
                CancelInvoke();
                Remove();
            }
            else if (hit[i].transform.CompareTag("Sand"))
            {
                Vector3 CollsionPoint = hit[i].point;
                ObjectPoolMgr.instance.MakeParticle("WFXMR_BImpact Dirt + Hole", CollsionPoint, this.transform.rotation);   //Make particle at attack point
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

        hit = Physics.RaycastAll(ray, direction.magnitude).OrderBy(h=>h.distance).ToArray();
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform.CompareTag("Enemy"))
            {
                Vector3 CollsionPoint = hit[i].point;
                int[] DamageSet = new int[2] { m_HeadDamage, m_BodyDamage };
                hit[i].transform.gameObject.SendMessage("GetDamage", DamageSet);
                Debug.Log("Head Damage : " + DamageSet[0] + "Name : " + hit[i].transform.name);
                ObjectPoolMgr.instance.CreatePooledObject("FX_BloodSplatter_Bullet", CollsionPoint, this.transform.rotation);   //Make particle at attack point

                //Minus 50 Damage per every penetration
                m_HeadDamage = Mathf.Max(m_HeadDamage-30, 0);
                m_BodyDamage = Mathf.Max(m_BodyDamage-30, 0);

                if (m_HeadDamage + m_BodyDamage == 0)
                {
                    CancelInvoke();
                    Remove();
                }
            }
            else if (hit[i].transform.CompareTag("Floor"))
            {
                Vector3 CollsionPoint = hit[i].point;
                ObjectPoolMgr.instance.MakeParticle("WFXMR_BImpact Concrete + Hole Unlit", CollsionPoint, this.transform.rotation);   //Make particle at attack point
                CancelInvoke();
                Remove();
            }
            else if (hit[i].transform.CompareTag("Sand"))
            {
                Vector3 CollsionPoint = hit[i].point;
                ObjectPoolMgr.instance.MakeParticle("WFXMR_BImpact Dirt + Hole", CollsionPoint, this.transform.rotation);   //Make particle at attack point
                CancelInvoke();
                Remove();
            }
        }
    }
}
