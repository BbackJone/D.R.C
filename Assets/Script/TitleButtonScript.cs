using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButtonScript : MonoBehaviour {
    //public GameObject loadingIndicator;
    private GameObject titleButtons;
    private GameObject loadingText;

    private bool isButtonPressed = false;

    void Start() {
        titleButtons = GameObject.Find("TitleButtons");
        loadingText = GameObject.Find("LoadingText");
    }
    
    public void CallLoadSceneAsync(bool isContinue) {
        if (isContinue) {
            if (!SaveData.SaveExists(0)) return;
        }

        if (Time.timeScale == 0f) return;

        if (isButtonPressed) return;
        isButtonPressed = true;

        ObjectManager.m_Inst.Objects.m_Playerlist.Clear();
        ObjectManager.m_Inst.Objects.m_Colleaguelist.Clear();
        ObjectManager.m_Inst.Objects.m_Enemylist.Clear();
        ObjectManager.m_Inst.Objects.m_Weaponlist.Clear();
        ObjectManager.m_Inst.Objects.m_Bulletlist.Clear();

        GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>().InitGameSaveData(isContinue);
        
        var titleSanta = GameObject.Find("TitleSanta");
        //titleSanta.GetComponent<TitleSantaAction>().enabled = false;
        //titleSanta.GetComponent<PlayerInput>().enabled = false;
        GameObject.Find("GameController").GetComponent<SantaPositionPreserver>().SaveSantaPos(titleSanta.transform.position, titleSanta.transform.rotation,
                                                                                              titleSanta.GetComponent<PlayerInput>().m_Mouse_Y);

        //loadingIndicator.SetActive(true);
        foreach (Transform t in transform) {
            t.gameObject.SetActive(false);
        }
        loadingText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);

        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync() {
        // properly load database first
        if (!ObjectManager.m_Inst.m_DBMgr.loaded) {
            string jsonDB;
            if (Application.platform == RuntimePlatform.Android) {
                WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/DeepRedChristMas_DB.json");
                yield return www;
                jsonDB = www.text;
            } else {
                jsonDB = File.ReadAllText (Path.Combine (Application.streamingAssetsPath, "DeepRedChristMas_DB.json"));
            }
            var gameDB = JsonUtility.FromJson<GameDB> (jsonDB);

            ZomebieDB[] TempZomebie = gameDB.Zombie;
            WeaponDB[] TempWeapon = gameDB.Weapon;
            WaveDB[] TempWave = gameDB.Wave;

            for (int i = 0; i < TempZomebie.Length; i++)
                ObjectManager.m_Inst.m_DBMgr.m_ZomebieDB.Add (TempZomebie [i].Name, TempZomebie [i]);
            for (int i = 0; i < TempWeapon.Length; i++)
                ObjectManager.m_Inst.m_DBMgr.m_WeaponDB.Add (TempWeapon [i].Name, TempWeapon [i]);
            for (int i = 0; i < TempWave.Length; i++)
                ObjectManager.m_Inst.m_DBMgr.m_WaveDB.Add (TempWave [i].Level, TempWave [i]);

            ObjectManager.m_Inst.m_DBMgr.loaded = true;
        }

        ObjectManager.m_Inst.SetState(STATE_ID.STATE_STAGE);
        var asyncOp = SceneManager.LoadSceneAsync("Stage");

        while (!asyncOp.isDone) {
            yield return null;
        }
    }

    public void ResetSave() {
        StartCoroutine(ResetSaveCo());
    }

    IEnumerator ResetSaveCo()
    {
        var lu = GameObject.Find("LanguageUpdater").GetComponent<LanguageUpdater>();
        while (!lu.lm.IsLoaded) { yield return new WaitForEndOfFrame(); }

        SaveData.ClearAll();
        GameObject.Find("SaveClearButton").transform.Find("Text").GetComponent<Text>().text = lu.lm.Get("settings_cleared");
        transform.Find("ContinueButton").transform.Find("Text").GetComponent<Text>().text = lu.lm.Get("title_continue_nf");
    }
}
