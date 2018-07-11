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
    public ObjectManager objMgr;

    private bool isButtonPressed = false;

    void Start() {
        titleButtons = GameObject.Find("TitleButtons");
        loadingText = GameObject.Find("LoadingText");
    }
    
    public void CallLoadSceneAsync(bool isContinue) {
        if (isButtonPressed) return;
        isButtonPressed = true;

        GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>().InitGameSaveData(isContinue);

        StartCoroutine(LoadAsync());

        //loadingIndicator.SetActive(true);
        titleButtons.SetActive(false);
        loadingText.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator LoadAsync() {
        // properly load database first
        string jsonDB;
        if (Application.platform == RuntimePlatform.Android) {
            using (WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/DeepRedChristMas_DB.json")) {
                while (!www.isDone && www.error == null) {
                    yield return null;
                }
                jsonDB = www.text;
            }
        } else {
            jsonDB = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "DeepRedChristMas_DB.json"));
        }
        var gameDB = JsonUtility.FromJson<GameDB>(jsonDB);

        ZomebieDB[] TempZomebie = gameDB.Zombie;
        WeaponDB[] TempWeapon = gameDB.Weapon;
        WaveDB[] TempWave = gameDB.Wave;

        for (int i = 0; i < TempZomebie.Length; i++)
            objMgr.m_DBMgr.m_ZomebieDB.Add(TempZomebie[i].Name, TempZomebie[i]);
        for (int i = 0; i < TempWeapon.Length; i++)
            objMgr.m_DBMgr.m_WeaponDB.Add(TempWeapon[i].Name, TempWeapon[i]);
        for (int i = 0; i < TempWave.Length; i++)
            objMgr.m_DBMgr.m_WaveDB.Add(TempWave[i].Level, TempWave[i]);

        var asyncOp = SceneManager.LoadSceneAsync("Stage");
        GameObject.Find("GameController").GetComponent<ObjectManager>().SetState(STATE_ID.STATE_STAGE);

        while (!asyncOp.isDone) {
            yield return null;
        }
    }

    // temporary function?
    public void ResetSave() {
        SaveData.ClearAll();
        transform.Find("SaveClearButton").Find("Text").GetComponent<Text>().text = "Cleared!";
        transform.Find("ContinueButton").transform.Find("Text").GetComponent<Text>().text = "Continue from\nNew Game";
    }
}
