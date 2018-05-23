using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//make file route to be used to read or write jason file
public class MyPath : MonoBehaviour {

    public string fileName;
    public string streamingPath;    //location to put jason file.
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
