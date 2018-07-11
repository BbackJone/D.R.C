using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour {
    private static SaveDataManager instance = null;
    public SaveData currentSaveData = null;

	void Start () {
        if (instance != null) {
            Destroy(this);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void InitGameSaveData(bool isContinue) {
        if (isContinue) {
            // check if savedata 0 exist
            if (SaveData.SaveExists(0)) {
                // load savedata 0 as current
                currentSaveData = SaveData.Load(0);

                Debug.Log("Save loaded: wave " + currentSaveData.currentWave);
            } else {
                // set it to null
                currentSaveData = null;
                Debug.Log("Save not found");
            }
        } else {
            // create new savedata and continue
            currentSaveData = SaveData.CreateNew();
        }
    }
}
