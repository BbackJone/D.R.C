using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveDB
{
    public int Level;
    public int NormalZombieNumber;
    public int SpecialZombieNumber;
    public float NormalZombieSpeedRate;
    public float NormalZombiePowerRate;
    public float NormalZombieHealthRate;
}


public class StageMgr : MonoBehaviour
{
    public static StageMgr instance;    //singleton
    public GameObject[] m_LevelImage;
    public WaveDB m_CurrentWave;
    public float m_GameTime;    //Initializing at ResultScoreContainerScript.

    public int m_Spawned_NormalZombieNumber = 0;     //number of all amount of spawned zombies
    public int m_Spawned_SpecialZombieNumber = 0;
    public int m_Current_NormalZombieNumber = 0;     //number of zombies which are alive in stage
    public int m_Current_SpecialZombieNumber = 0;

    private SaveDataManager m_SaveMgr;

    private List<string> m_NormalZombieNameList = new List<string>();
    private List<string> m_SpecialZombieNameList = new List<string>();

    private void Awake()
    {
        instance = this;
        m_SaveMgr = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
    }

    // Use this for initialization
    void Start()
    {
        int startWave = 3;
        if (m_SaveMgr.currentSaveData != null) startWave = m_SaveMgr.currentSaveData.currentWave;

        m_CurrentWave = ObjectManager.m_Inst.m_DBMgr.m_WaveDB[3];

        ShowImageForseconds(m_LevelImage[m_CurrentWave.Level-1], 3f);
        StartCoroutine("CheckWave");

        m_SpecialZombieNameList.Add("RugbyZombie");
        m_SpecialZombieNameList.Add("DevilZombie");
        m_SpecialZombieNameList.Add("PrisonerZombie");
        m_SpecialZombieNameList.Add("SoldierZombie");

        m_NormalZombieNameList.Add("NormalZombie1");
        m_NormalZombieNameList.Add("NormalZombie2");
        m_NormalZombieNameList.Add("NormalZombie3");
        m_NormalZombieNameList.Add("NormalZombie4");
        m_NormalZombieNameList.Add("NormalZombie5");
    }

    private void Update()
    {
        m_GameTime += Time.deltaTime * Time.timeScale;
        ZombieSpawnTimer();

        if (Input.GetKeyDown("y"))
            SpawnZombie("RugbyZombie");
    }


    //Show Image for seconds on the middle of Canvas.
    public void ShowImageForseconds(GameObject _image, float _seconds)
    {
        _image.SetActive(true);
        StartCoroutine(Cancel_Image(_image, _seconds));
    }

    public IEnumerator Cancel_Image(GameObject _image, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _image.SetActive(false);
    }

    public void ZombieSpawnTimer()
    {
        //Spawn Normal zombies
        if (m_Spawned_NormalZombieNumber < m_CurrentWave.NormalZombieNumber)
        {
            if (m_GameTime % 1f < 0.5f)
            {
                int ZombieNameIndex = Random.Range(0, m_NormalZombieNameList.Count);
                SpawnZombie(m_NormalZombieNameList[ZombieNameIndex]);
            }
        }

        //Spawn Special zombies
        if (m_Spawned_SpecialZombieNumber < m_CurrentWave.SpecialZombieNumber)
        {
            if (m_GameTime % 1f < 0.1f)
            {
                int ZombieNameIndex = Random.Range(0, m_SpecialZombieNameList.Count);
                SpawnZombie(m_SpecialZombieNameList[ZombieNameIndex]);
            }
        }
    }

    //Spawn zombies at random spot
    public void SpawnZombie(string _zombiename)
    {
        int posindex = Random.Range(0, ObjectPoolMgr.instance.m_PoolingPos.Length);
        Vector3 temppos = ObjectPoolMgr.instance.m_PoolingPos[posindex];
        ObjectPoolMgr.instance.CreatePooledObject(_zombiename, temppos, Quaternion.identity);
    }

    public void AddNormalZombieNumber(int _num)
    {
        m_Current_NormalZombieNumber += _num;
        if (_num > 0)
            m_Spawned_NormalZombieNumber += _num;
    }

    public void AddSpecialZombieNumber(int _num)
    {
        m_Current_SpecialZombieNumber += _num;
        if (_num > 0)
            m_Spawned_SpecialZombieNumber += _num;
    }

    public void NextWave()
    {
        m_CurrentWave = ObjectManager.m_Inst.m_DBMgr.m_WaveDB[m_CurrentWave.Level + 1];
        ShowImageForseconds(m_LevelImage[m_CurrentWave.Level-1], 3f);
        m_Spawned_NormalZombieNumber = 0;
        m_Spawned_SpecialZombieNumber = 0;

        StopCoroutine("CheckWave");     //give 3 seconds to spawn zombie in scene
        StartCoroutine("CheckWave");
    }

    public IEnumerator CheckWave()
    {
        yield return new WaitForSeconds(3f);

        while(true)
        {
            if (m_Current_NormalZombieNumber + m_Current_SpecialZombieNumber <= 0)
            {
                NextWave();
            }

            yield return null;
        }
    }
}