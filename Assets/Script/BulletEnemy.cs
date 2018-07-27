using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour{

    public ObjType m_Type { get; set; }
    public ObjectManager m_ObjMgr { get; set; }

    public float m_Speed { get; set; }
    public int m_BodyDamage;
    public int m_HeadDamage;
    private float m_StayTime;
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
        m_StayTime = 2f;
        m_ObjMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        m_Trail = transform.GetComponent<TrailRenderer>();
    }

    void Start()
    {
        m_Speed = 100f;
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
        Vector3 direction = transform.position - m_PrevPos;
        Ray ray = new Ray(m_PrevPos, direction.normalized);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit ,direction.magnitude))
        {
            if(hit.transform.CompareTag("Player"))
            {
                Vector3 CollsionPoint = hit.point;
                hit.transform.gameObject.SendMessage("GetDamage", 1);
                ObjectPoolMgr.instance.CreatePooledObject("FX_BloodSplatter_Bullet", CollsionPoint, this.transform.rotation);   //Make particle at attack point
                CancelInvoke();
                Remove();
            }
        }
    }

    void Remove()
    {
        gameObject.SetActive(false);
    }
}
