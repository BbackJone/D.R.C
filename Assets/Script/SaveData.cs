using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDB
{
    public ZomebieDB[] Zombie;
    public WeaponDB[] Weapon;
    public WaveDB[] Wave;
}

/// <summary>
/// Class holding game's saved data and related functions.
/// </summary>
public class SaveData {
    public int currentWave;
    public int kills;
    public int spkills;
    public int elapsedTime;
    public int health;

    private const char delimiter = '|';

    private SaveData() { }
    
    /// <summary>
    /// Load saved data from disk.
    /// </summary>
    /// <param name="index">Index of target save data</param>
    /// <returns></returns>
    public static SaveData Load(int index) {
        CheckIndexRange(index);
        if (!SaveExists(index)) return null;
        string rawSaveData = PlayerPrefs.GetString("GameSave" + index);
        SaveData sd = StringToSaveData(rawSaveData);
        return sd;
    }

    /// <summary>
    /// Create new saved data. Note that this function does nothing about saving newly created game data.
    /// </summary>
    /// <returns></returns>
    public static SaveData CreateNew() {
        SaveData sd = new SaveData();

        // input new save data here
        sd.currentWave = 1;
        sd.kills = 0;
        sd.spkills = 0;
        sd.elapsedTime = 0;
        sd.health = 100;

        return sd;
    }

    /// <summary>
    /// Write saved data into disk.
    /// </summary>
    /// <param name="saveData">SaveData to save</param>
    /// <param name="index">Index of saved data</param>
    public static void Write(SaveData saveData, int index) {
        CheckIndexRange(index);
        string rawSaveData = SaveDataToString(saveData);
        PlayerPrefs.SetString("GameSave" + index, rawSaveData);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Check if saved data exists in index.
    /// </summary>
    /// <param name="index">Index of saved data</param>
    /// <returns></returns>
    public static bool SaveExists(int index) {
        CheckIndexRange(index);
        return PlayerPrefs.HasKey("GameSave" + index);
    }

    /// <summary>
    /// Clear all saved data.
    /// </summary>
    public static void ClearAll() {
        for (int i = 0; i < 16; i++) {
            if (PlayerPrefs.HasKey("GameSave" + i)) {
                PlayerPrefs.DeleteKey("GameSave" + i);
            }
        }
        PlayerPrefs.Save();
    }

    private static string SaveDataToString(SaveData sd) {
        return string.Join(delimiter.ToString(), new string[] { sd.currentWave.ToString(),
                                                                sd.kills.ToString(),
                                                                sd.elapsedTime.ToString(),
                                                                sd.health.ToString(),
                                                                sd.spkills.ToString() });
    }

    private static SaveData StringToSaveData(string rsd) {
        string[] rsds = rsd.Split(delimiter);
        if (rsds.Length != 5) return null;

        SaveData sd = new SaveData();

        sd.currentWave = int.Parse(rsds[0]);
        sd.kills = int.Parse(rsds[1]);
        sd.elapsedTime = int.Parse(rsds[2]);
        sd.health = int.Parse(rsds[3]);
        sd.spkills = int.Parse(rsds[4]);

        return sd;
    }

    private static void CheckIndexRange(int index) {
        if (index < 0 || index > 15) throw new IndexOutOfRangeException("SaveData index must be between 0 to 15");
    }
}
