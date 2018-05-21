using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiveObject : IObj
{
    //Obj(Interface)
    //ObjType m_Type { get; set; }
    //ObjectManager m_ObjMgr { get; set; }
    //void ObjListAdd();

    //Here
    int m_Hp { get; set; }
    bool m_Death { get; set; }
    float m_DeathTimer { get; set; }
    float m_Speed { get; set; }
}
