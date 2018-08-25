using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class LanguageManager {
    private static LanguageManager instance;

    private Dictionary<string, string> map;
    public bool IsLoaded;
    
    private LanguageManager()
    {
        IsLoaded = false;
        map = new Dictionary<string, string>();
    }

    public IEnumerator LoadLanguage(bool forceDefault = false)
    {
        string rawLang;

        if (Application.platform == RuntimePlatform.Android) {
            WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/default.lang");
            yield return www;
            rawLang = www.text.Replace("\r", "");
        } else {
            rawLang = File.ReadAllText(Path.Combine (Application.streamingAssetsPath, "default.lang")).Replace("\r", "");
        }

        foreach (string s in rawLang.Split('\n'))
        {
            if (s.Contains(":"))
            {
                var ss = s.Split(new char[] { ':' }, 2);
                map.Add(ss[0].Trim(), ss[1].Trim().Replace("[newline]", "\n"));
            }
        }

        if (!forceDefault)
        {
            // load additional language
            bool additionalLanguageExist = false;
            string userlang = Application.systemLanguage.ToString().ToLower();

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/" + userlang + ".lang");
                yield return www;
                if (string.IsNullOrEmpty(www.text))
                {
                    additionalLanguageExist = false;
                }
                else
                {
                    additionalLanguageExist = true;
                    rawLang = www.text.Replace("\r", "");
                }
            }
            else
            {
                if (File.Exists(Path.Combine(Application.streamingAssetsPath, userlang + ".lang")))
                {
                    additionalLanguageExist = true;
                    rawLang = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, userlang + ".lang")).Replace("\r", "");
                }
                else
                {
                    additionalLanguageExist = false;
                }
            }

            if (!additionalLanguageExist)
            {
                // no additional language found - just use default
                Debug.Log("Language " + userlang + " not found");
            }
            else
            {
                foreach (string s in rawLang.Split('\n'))
                {
                    if (s.Contains(":"))
                    {
                        var ss = s.Split(new char[] { ':' }, 2);
                        map.Remove(ss[0].Trim());
                        map.Add(ss[0].Trim(), ss[1].Trim().Replace("[newline]", "\n"));
                    }
                }
            }
        }

        IsLoaded = true;
    }

    public static LanguageManager GetInstance()
    {
        if (instance == null) instance = new LanguageManager();

        return instance;
    }

    public string Get(string key)
    {
        if (map.ContainsKey(key)) return map[key];
        else return key;
    }
}
