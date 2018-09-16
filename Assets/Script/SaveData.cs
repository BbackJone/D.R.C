using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    public List<Vector3> turretPos;

    private const char delimiter = '|';
    private const char turretDelimiter = '?';
    private const char turretPosDelimiter = '!';

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
        string turretData;
        if (sd.turretPos == null || sd.turretPos.Count == 0) {
            turretData = null;
        } else {
            StringBuilder sb = new StringBuilder();
            foreach (Vector3 tpos in sd.turretPos) {
                sb.Append(tpos.x);
                sb.Append(turretPosDelimiter);
                sb.Append(tpos.y);
                sb.Append(turretPosDelimiter);
                sb.Append(tpos.z);
                sb.Append(turretDelimiter);
            }
            sb.Length = sb.Length - 1;
            turretData = sb.ToString();
        }

        return string.Join(delimiter.ToString(), new string[] { sd.currentWave.ToString(),
                                                                sd.kills.ToString(),
                                                                sd.elapsedTime.ToString(),
                                                                sd.health.ToString(),
                                                                sd.spkills.ToString(),
                                                                turretData });
    }

    private static SaveData StringToSaveData(string rsd) {
        string[] rsds = rsd.Split(delimiter);
        if (rsds.Length < 4) return null;

        SaveData sd = new SaveData();

        sd.currentWave = int.Parse(rsds[0]);
        sd.kills = int.Parse(rsds[1]);
        sd.elapsedTime = int.Parse(rsds[2]);
        sd.health = int.Parse(rsds[3]);
        sd.spkills = int.Parse(rsds[4]);

        try {
            if (rsds.Length == 5 || string.IsNullOrEmpty(rsds[5].Trim())) {
                sd.turretPos = null;
            } else {
                string[] turPos = rsds[5].Trim().Split(turretDelimiter);
                sd.turretPos = new List<Vector3>();
                foreach (string tur in turPos) {
                    string[] turXY = tur.Split(turretPosDelimiter);
                    Vector3 vTurPos = new Vector3(float.Parse(turXY[0]), float.Parse(turXY[1]), float.Parse(turXY[2]));
                    sd.turretPos.Add(vTurPos);
                }
            }
        } catch (Exception) {
            // if any kind of error happens during turret parsing, just pretend like turrets never existed
            sd.turretPos = null;
        }

        return sd;
    }

    private static void CheckIndexRange(int index) {
        if (index < 0 || index > 15) throw new IndexOutOfRangeException("SaveData index must be between 0 to 15");
    }
}
