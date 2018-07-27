using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameMovement : MonoBehaviour
{
    public float m_Speed { get; set; }
    public int m_Damage;
    private float m_StayTime;

    //Raycast Parameters
    public Vector3 m_PrevPos;

    // Use this for initialization
    void Awake()
    {
        m_StayTime = 2f;
    }

    void Start()
    {
        m_Speed = 30f;
    }

    void OnEnable()
    {
        m_PrevPos = transform.position;
        Invoke("Remove", m_StayTime);   //Remove this after "m_StayTime"
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

        if (Physics.Raycast(ray, out hit, direction.magnitude))
        {
            if (hit.transform.CompareTag("Floor"))
            {
                Vector3 CollsionPoint = hit.point;
                ObjectPoolMgr.instance.CreatePooledObject("ExplosionParticle", CollsionPoint, Quaternion.LookRotation(Vector3.up));   //Make particle at attack point
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
