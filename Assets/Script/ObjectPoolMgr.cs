using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr : MonoBehaviour {

    public static ObjectPoolMgr instance;

    private struct ObjectToPool
    {
        public GameObject Obj;
        public string ObjName;
        public ObjType objType;
        public int AmountToPool;
        public bool ShouldExpand;
    }
    private Dictionary<string, ObjectToPool> m_ObjectToPool = new Dictionary<string, ObjectToPool>();
    public Dictionary<string, List<GameObject>> m_PooledObject { get; set; }

    public Vector3[] m_PoolingPos;
    private Transform m_Directory_PooledObject;     //Forder that contains pooledObjects.

    void Awake()
    {
        instance = this;

        m_PooledObject = new Dictionary<string, List<GameObject>>();
        m_Directory_PooledObject = transform.GetChild(2);

        //Initializing Position (포지션을 할당해 놓는다.)
        Transform Directory_PoolingPos = transform.GetChild(1);
        m_PoolingPos = new Vector3[Directory_PoolingPos.childCount];
        for (int i = 0; i < m_PoolingPos.Length; i++ )
        {
            m_PoolingPos[i] = Directory_PoolingPos.GetChild(i).position;
        }

        //Initializing ObjectToPool
        Transform Directory_ObjectToPool = transform.GetChild(0);

        //Zomebie(Businessman)
        ObjectToPool ZomebieToPool = new ObjectToPool();
        ZomebieToPool.Obj = Directory_ObjectToPool.GetChild(0).gameObject;
        ZomebieToPool.AmountToPool = 50;
        ZomebieToPool.ObjName = ZomebieToPool.Obj.name;
        ZomebieToPool.objType = ObjType.OBJ_ENEMY;
        ZomebieToPool.ShouldExpand = false;

        m_ObjectToPool.Add(ZomebieToPool.ObjName ,ZomebieToPool);
        GameObject Directory = new GameObject();                                           
        Directory.name = ZomebieToPool.ObjName;                                            
        Directory.transform.SetParent(m_Directory_PooledObject);                           
                                                                                           
        //Zomebie(SA_Zombie_RoadWorker)                                                    
        ObjectToPool ZomebieToPool2 = new ObjectToPool();                                  
        ZomebieToPool2.Obj = Directory_ObjectToPool.GetChild(4).gameObject;                
        ZomebieToPool2.AmountToPool = 50;                                                   
        ZomebieToPool2.ObjName = ZomebieToPool2.Obj.name;                                   
        ZomebieToPool2.objType = ObjType.OBJ_ENEMY;                                        
        ZomebieToPool2.ShouldExpand = false;

        m_ObjectToPool.Add(ZomebieToPool2.ObjName, ZomebieToPool2);
        Directory = new GameObject();
        Directory.name = ZomebieToPool2.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);

        //Particle(FX_BloodSplatter_Katana)
        ObjectToPool ParticleToPool = new ObjectToPool();
        ParticleToPool.Obj = Directory_ObjectToPool.GetChild(1).gameObject;
        ParticleToPool.AmountToPool = 10;
        ParticleToPool.ObjName = ParticleToPool.Obj.name;
        ParticleToPool.objType = ObjType.OBJ_ETC;
        ParticleToPool.ShouldExpand = true;

        m_ObjectToPool.Add(ParticleToPool.ObjName ,ParticleToPool);
        Directory = new GameObject();
        Directory.name = ParticleToPool.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);

        //Particle(FX_BloodSplatter_Bullet)
        ObjectToPool ParticleToPool2 = new ObjectToPool();
        ParticleToPool2.Obj = Directory_ObjectToPool.GetChild(3).gameObject;
        ParticleToPool2.AmountToPool = 10;
        ParticleToPool2.ObjName = ParticleToPool2.Obj.name;
        ParticleToPool2.objType = ObjType.OBJ_ETC;
        ParticleToPool2.ShouldExpand = true;

        m_ObjectToPool.Add(ParticleToPool2.ObjName, ParticleToPool2);
        Directory = new GameObject();
        Directory.name = ParticleToPool2.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);


        //Bullet
        ObjectToPool BulletToPool = new ObjectToPool();
        BulletToPool.Obj = Directory_ObjectToPool.GetChild(2).gameObject;
        BulletToPool.AmountToPool = 10;
        BulletToPool.ObjName = BulletToPool.Obj.name;
        BulletToPool.objType = ObjType.OBJ_BULLET;
        BulletToPool.ShouldExpand = true;

        m_ObjectToPool.Add(BulletToPool.ObjName, BulletToPool);
        Directory = new GameObject();
        Directory.name = BulletToPool.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);

        //Flame
        ObjectToPool FlameToPool = new ObjectToPool();
        FlameToPool.Obj = Directory_ObjectToPool.GetChild(7).gameObject;
        FlameToPool.AmountToPool = 10;
        FlameToPool.ObjName = FlameToPool.Obj.name;
        FlameToPool.objType = ObjType.OBJ_ETC;
        FlameToPool.ShouldExpand = true;

        m_ObjectToPool.Add(FlameToPool.ObjName, FlameToPool);
        Directory = new GameObject();
        Directory.name = FlameToPool.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);

        //ExplosionParticle
        ObjectToPool ExplosionToPool = new ObjectToPool();
        ExplosionToPool.Obj = Directory_ObjectToPool.GetChild(8).gameObject;
        ExplosionToPool.AmountToPool = 10;
        ExplosionToPool.ObjName = ExplosionToPool.Obj.name;
        ExplosionToPool.objType = ObjType.OBJ_ETC;
        ExplosionToPool.ShouldExpand = true;

        m_ObjectToPool.Add(ExplosionToPool.ObjName, ExplosionToPool);
        Directory = new GameObject();
        Directory.name = ExplosionToPool.ObjName;
        Directory.transform.SetParent(m_Directory_PooledObject);
    }

    void Start()
    {
        MakePool();
    }

    //Pooling 되있는 오브젝트를 활성화 시키는 함수이다.
    public GameObject CreatePooledObject(string _objname, Vector3 _pos, Quaternion _rot)
    {
        List<GameObject> objlist = m_PooledObject[_objname];
        for (int i = 0; i < objlist.Count; i++ )
        {
            if (!objlist[i].activeInHierarchy)
            {
                objlist[i].transform.position = _pos;
                objlist[i].transform.rotation = _rot;
                objlist[i].SetActive(true);
                return objlist[i];
            }
        }

        //if the number of pooled object is required to expand, it expand that
        if (m_ObjectToPool[_objname].ShouldExpand)
        {
            MakePool();
            CreatePooledObject(_objname, _pos, _rot);
        }

        return null;
    }


    //Pooling을 하는 함수.
    public void MakePool()
    {
        //Pooling 을 하는 작업이다.
        foreach(KeyValuePair<string, ObjectToPool> iter in m_ObjectToPool)
        {
            ObjectToPool itemToPool = iter.Value;
            for (int j = 0; j < itemToPool.AmountToPool; j++)
            {
                GameObject Obj = Instantiate(itemToPool.Obj);
                Obj.name = itemToPool.ObjName;
                int PosIndex = Random.Range(0, m_PoolingPos.Length);
                Obj.transform.position = m_PoolingPos[PosIndex];
                Transform Parent_Directory = m_Directory_PooledObject;
                for (int h = 0; h < m_Directory_PooledObject.childCount; h++)
                {
                    if (m_Directory_PooledObject.GetChild(h).name == itemToPool.Obj.name)
                    {
                        Parent_Directory = m_Directory_PooledObject.GetChild(h);
                        break;
                    }
                }
                Obj.transform.SetParent(Parent_Directory);
                Obj.SetActive(false);

                if (!m_PooledObject.ContainsKey(itemToPool.ObjName))
                {
                    m_PooledObject.Add(itemToPool.ObjName, new List<GameObject>());
                }
                m_PooledObject[itemToPool.ObjName].Add(Obj);
            }
        }
    }

}