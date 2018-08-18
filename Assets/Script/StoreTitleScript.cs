using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTitleScript : MonoBehaviour {

    public void ShowStore()
    {
        gameObject.SetActive(true);
    }

    public void HideStore()
    {
        gameObject.SetActive(false);
    }
}
