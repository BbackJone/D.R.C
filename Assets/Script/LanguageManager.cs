using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager {
    private LanguageManager instance;

    private LanguageManager()
    {
        // TODO: load database here...
    }

    public LanguageManager GetInstance()
    {
        if (instance != null) instance = new LanguageManager();

        return instance;
    }
}
