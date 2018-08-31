using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When creating particle, if no more pooled particle trash old particle and make the old particle to be a new particle
public class ParticleOrderList
{
    public class node
    {
        public int index;
        public node nextNode;

        public node(int _index)
        {
            index = _index;
        }
    }

    public void PushBack(node _node)
    {
        if(lastNode != null)
        {
            lastNode.nextNode = _node;
            lastNode = _node;
            lastNode.nextNode = null;
        }
        else
        {
            firstNode = _node;
            lastNode = _node;
            lastNode.nextNode = null;
        }
    }
    public void PushFront(node _node)
    {
        if(firstNode != null)
        {
            _node.nextNode = firstNode;
            firstNode = _node;
        }
        else
        {
            firstNode = _node;
            lastNode = _node;
        }
    }

    public node PopFront()
    {
        node originFirst = new node(firstNode.index);
        firstNode = firstNode.nextNode;
        return originFirst;
    }

    public node firstNode;
    public node lastNode;
}

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

    public Dictionary<string, ParticleOrderList> m_ParticleOrderList = new Dictionary<string, ParticleOrderList>();

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

      
        //AddObjectToPool-------------------------------------------
        //Zomebie(Businessman)
        AddObjectToPool(0, 50, ObjType.OBJ_ENEMY, false);

        //Particle(FX_BloodSplatter_Katana)
        AddObjectToPool(1, 20, ObjType.OBJ_ETC, true);

        //Bullet
        AddObjectToPool(2, 20, ObjType.OBJ_BULLET, true);

        //Particle(FX_BloodSplatter_Bullet)
        AddObjectToPool(3, 50, ObjType.OBJ_ETC, true);

        //Zomebie(SA_Zombie_RoadWorker)            
        AddObjectToPool(4, 50, ObjType.OBJ_ENEMY, false);

        //DropHeart
        AddObjectToPool(5, 20, ObjType.OBJ_ETC, true);

        //DropAmmo
        AddObjectToPool(6, 10, ObjType.OBJ_ETC, true);

        //Flame
        AddObjectToPool(7, 10, ObjType.OBJ_ETC, true);

        //ExplosionParticle
        AddObjectToPool(8, 10, ObjType.OBJ_ETC, true);

        //Devil Zombie
        AddObjectToPool(9, 10, ObjType.OBJ_ENEMY, false);

        //Rugby Zombie
        AddObjectToPool(10, 10, ObjType.OBJ_ENEMY, false);

        //PrisonerZombie   
        AddObjectToPool(11, 10, ObjType.OBJ_ENEMY, false);

        //SoldierZombie
        AddObjectToPool(12, 10, ObjType.OBJ_ENEMY, false);

        //Zomebie(Normal3)            
        AddObjectToPool(13, 50, ObjType.OBJ_ENEMY, false);

        //Zomebie(Normal4)            
        AddObjectToPool(14, 50, ObjType.OBJ_ENEMY, false);

        //Zomebie(Normal5)            
        AddObjectToPool(15, 50, ObjType.OBJ_ENEMY, false);

        //Rocket
        AddObjectToPool(16, 3, ObjType.OBJ_BULLET, true);

        //SniperBullet
        AddObjectToPool(17, 5, ObjType.OBJ_BULLET, true);

        //Bullet Impact_Concrete
        AddObjectToPool(18, 100, ObjType.OBJ_ETC, false);
        m_ParticleOrderList.Add("WFXMR_BImpact Concrete + Hole Unlit", new ParticleOrderList());

        //Bullet Impact_Dirt
        AddObjectToPool(19, 100, ObjType.OBJ_ETC, false);
        m_ParticleOrderList.Add("WFXMR_BImpact Dirt + Hole", new ParticleOrderList());

        //Soldier Zombie Bullet
        AddObjectToPool(20, 10, ObjType.OBJ_BULLET, true);
    }

    public void AddObjectToPool(int _childindex, int _amountToPool, ObjType _objtype, bool _shouldexpand)
    {
        Transform Directory_ObjectToPool = transform.GetChild(0);

        ObjectToPool ObjectToPool = new ObjectToPool();
        ObjectToPool.Obj = Directory_ObjectToPool.GetChild(_childindex).gameObject;
        ObjectToPool.AmountToPool = _amountToPool;

        //parsing number.name => number[0] / name[1]
        string objname = ObjectToPool.Obj.name;
        string[] sp = objname.Split(new char[] { '.' }, 2);
        if (sp.Length <= 1)
        {
            ObjectToPool.ObjName = ObjectToPool.Obj.name;
        }
        else
        {
            ObjectToPool.ObjName = sp[1];
        }

        ObjectToPool.objType = _objtype;
        ObjectToPool.ShouldExpand = _shouldexpand;

        m_ObjectToPool.Add(ObjectToPool.ObjName, ObjectToPool);
        GameObject Directory = new GameObject();
        Directory.name = ObjectToPool.ObjName;
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
            MakePool(_objname);
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
                    if (string.Equals(m_Directory_PooledObject.GetChild(h).name, Obj.name))
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

    public void MakePool(string objToExpand)
    {
        //Pooling 을 하는 작업이다.
        foreach (KeyValuePair<string, ObjectToPool> iter in m_ObjectToPool)
        {
            if (iter.Key != objToExpand) continue;
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
                    if (string.Equals(m_Directory_PooledObject.GetChild(h).name, Obj.name))
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
            break;
        }
    }

    public GameObject MakeParticle(string _objname, Vector3 _pos, Quaternion _rot)
    {
        List<GameObject> objlist = m_PooledObject[_objname];
        for (int i = 0; i < objlist.Count; i++)
        {
            if (!objlist[i].activeInHierarchy)
            {
                objlist[i].transform.position = _pos;
                objlist[i].transform.rotation = _rot;
                objlist[i].SetActive(true);
                m_ParticleOrderList[_objname].PushBack(new ParticleOrderList.node(i));
                return objlist[i];
            }
        }

        //set inactive oldest particle
        int firstNodeIndex = m_ParticleOrderList[_objname].firstNode.index;
        objlist[firstNodeIndex].SetActive(false);

        //make the oldest particle to be a new particle
        objlist[firstNodeIndex].transform.position = _pos;
        objlist[firstNodeIndex].transform.rotation = _rot;
        objlist[firstNodeIndex].SetActive(true);

        //pick a first node and push that node to a back of a list
        ParticleOrderList.node firstNode = m_ParticleOrderList[_objname].PopFront();
        m_ParticleOrderList[_objname].PushBack(firstNode);

        return null;
    }
}