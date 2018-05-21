using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class MyPath : MonoBehaviour {

    public string fileName;
    public string streamingPath;
    public string persistentPath;

	// Use this for initialization
    void Awake()
    {
        StartCoroutine(SavePath());
        StartCoroutine(LoadPath());
    }

	IEnumerator LoadPath()
    {
        streamingPath = Application.streamingAssetsPath + "/" + fileName;

        yield return null;
    }

    IEnumerator SavePath()
    {
        string path;
        path = Application.persistentDataPath + "/" + "MyGame" + "/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        persistentPath = path + fileName;

        yield return null;
    }
}
