using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SaveAndExitButtonScript : MonoBehaviour, IPointerUpHandler {
    private ObjectManager objMgr;
    private StageMgr stageMgr;
    private SaveDataManager saveMgr;

	void Start () {
        objMgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectManager>();
        stageMgr = GameObject.Find("StageMgr").GetComponent<StageMgr>();
        saveMgr = GameObject.Find("SaveDataManager").GetComponent<SaveDataManager>();
        if (objMgr == null || stageMgr == null || saveMgr == null) enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData) {
        SaveData sd = saveMgr.currentSaveData;
        sd.currentWave = stageMgr.m_CurrentWave.Level;
        SaveData.Write(sd, 0);
        objMgr.NextScene("Menu");
    }
}
