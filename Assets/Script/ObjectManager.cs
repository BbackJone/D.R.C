using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ObjType
{
    OBJ_PLAYER,
    OBJ_ENEMY,
    OBJ_WEAPON,
    OBJ_BULLET,
    OBJ_COLLEAGUE,
    OBJ_ETC,    //파티클 등등
}


public enum STATE_ID
{
    STATE_LOGO,
    STATE_LOADING,
    STATE_STAGE,
    STATE_ENDING
}
public class ObjectManager : MonoBehaviour {

    //SingleTon
    private static ObjectManager m_Inst;

    public ObjectsWithTag Objects { get; set; }
    private UIManager m_UIMgr;
    public ObjectPoolMgr m_ObjPoolMgr { get; set; }
    public DBManager m_DBMgr { get; set; }    //NotRef
    private STATE_ID m_SceneState;

	// Use this for initialization
    void Awake()
    {
        Screen.SetResolution(720, 1280, true);
        if(m_Inst == null)
        {
            m_Inst = this;
            m_SceneState = STATE_ID.STATE_LOGO;
            DontDestroyOnLoad(m_Inst);
        }
        else
        {
            Destroy(this);
        }

        m_DBMgr = new DBManager();
        m_DBMgr.Initialize();
        Objects = new ObjectsWithTag();

        m_UIMgr = GetComponent<UIManager>();
    }


    //오브젝트에게 데미지를 가하는 함수입니다.
    public void DamageObj(ObjType _type, Transform _obj, int _strength)
    {
        if (_type == ObjType.OBJ_ENEMY)
        {
            List<ZombieInteraction> Templist = Objects.m_Enemylist;
            for (int i = 0; i < Templist.Count; i++ )
            {
                if (Templist[i].transform == _obj)
                {
                    Templist[i].gameObject.SendMessage("GetDamage", _strength);
                }
            }
        }
        else if (_type == ObjType.OBJ_PLAYER)
        {
            List<PlayerInteraction> Templist = Objects.m_Playerlist;
            for (int i = 0; i < Templist.Count; i++)
            {
                if (Templist[i].transform == _obj)
                {
                    Templist[i].gameObject.SendMessage("GetDamage", _strength);
                }
            }
        }
    }
	
    //모든 플레이어가 죽었으면 게임오버 UI를 띄웁니다.
    public void GameOverCheck()
    {
        List<PlayerInteraction> Templist = Objects.m_Playerlist;
        for (int i = 0; i < Templist.Count; i++)
        {
            if (Templist[i].gameObject.activeInHierarchy)
                return;
        }
        m_UIMgr.ActiveUI("GameOver");
        m_UIMgr.StartCoroutine("NextScene", "Menu");
    }


    //파티클을 생성합니다.
    public void MakeParticle( Vector3 _pos , Quaternion _rot ,string _particleName)
    {
        List<GameObject> TempList = m_ObjPoolMgr.m_PooledObject[_particleName];
        foreach(GameObject _obj in TempList)
        {
            if(!_obj.activeInHierarchy)
            {
                m_ObjPoolMgr.CreatePooledObject(_particleName, _pos, _rot);
                return;
            }
        }
    }



    public void NextScene(string _str)     //Button에서 실행시켜주기 위함 
    {
        SceneManager.LoadScene(_str);
        if (_str == "Logo")
            m_SceneState = STATE_ID.STATE_LOGO;
        else if (_str == "Loading")
            m_SceneState = STATE_ID.STATE_LOADING;
        else if (_str == "WorkPlace")
            m_SceneState = STATE_ID.STATE_STAGE;
        else if (_str == "Ending")
            m_SceneState = STATE_ID.STATE_ENDING;
    }
}
