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

public class ObjectManager : MonoBehaviour
{
    //SingleTon
    public static ObjectManager m_Inst;

    //public ObjectsWithTag Objects { get; set; }
    public PlayerInteraction m_Player { get; set; }
    private UIManager m_UIMgr;
    public DBManager m_DBMgr { get; set; }    //NotRef
    private STATE_ID m_SceneState;

    // Use this for initialization
    void Awake()
    {
        if (m_Inst == null)
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
        //Objects = new ObjectsWithTag();

        m_UIMgr = GetComponent<UIManager>();
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

    public void SetState(STATE_ID id) {
        m_SceneState = id;
    }
}
