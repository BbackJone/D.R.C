using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour{

    public ObjType m_Type { get; set; }

    public float m_Speed { get; set; }
    public int m_BodyDamage;
    public int m_HeadDamage;
    private float m_StayTime;

    private LineRenderer m_LineRenderer;

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
        m_LineRenderer = transform.GetComponent<LineRenderer>();
    }

    void Start()
    {
        m_Speed = 1000f;
    }

    void OnEnable()
    {
        m_PrevPos = transform.position;
        Invoke("Remove", m_StayTime);   //Remove this after "m_StayTime"
        StartCoroutine("CoCollisionCheck");
    }

    //proceed
    private void FixedUpdate()
    {
        m_PrevPos = transform.position;
        m_LineRenderer.SetPosition(0, m_PrevPos);

        transform.Translate(Vector3.forward * Time.deltaTime * m_Speed);

        m_LineRenderer.SetPosition(1, transform.position);
    }

    public IEnumerator CoCollisionCheck()
    {
        while(true)
        {
            //We have to wait for unity to render a bullet trail before it destroyed.
            yield return new WaitForEndOfFrame();

            CollisionCheck();
        }
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
                hit.transform.SendMessage("GetDamage", m_BodyDamage);
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
