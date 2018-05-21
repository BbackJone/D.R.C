using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour {

    private ButtonMgr BtnMgr;
    public string EventName;
    private bool Pressing_Check = false;
    public bool bcontinuous_Button;

    void Awake()
    {
        BtnMgr = transform.parent.GetComponent<ButtonMgr>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { MouseDown(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { MouseUp(); });
        trigger.triggers.Add(entry);
    }

    void Start()
    {
        if (bcontinuous_Button)
        {
            StartCoroutine("SendMessage");
        }
    }

    IEnumerator SendMessage()
    {
        while (true)
        {
            if (Pressing_Check == true)
            {
                BtnMgr.GetMessage(EventName);
            }

            yield return null;
        }
    }

    public void MouseDown()
    {
        if (bcontinuous_Button)
        {
            Pressing_Check = true;
        }
        else
        {
            BtnMgr.GetMessage(EventName);
        }
    }

    public void MouseUp()
    {
        Pressing_Check = false;
    }

    public void Change_Continuous_Attrib(bool _dat)
    {
        bcontinuous_Button = _dat;
    }
}