using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjType
{
    OBJ_PLAYER,
    OBJ_ENEMY,
    OBJ_WEAPON,
    OBJ_BULLET,
    OBJ_COLLEAGUE,
    OBJ_ETC,    //파티클 등등
}

public interface IObj
{
    ObjType m_Type { get; set; }
    ObjectManager m_ObjMgr { get; set; }
    string m_ObjName { get; set; }
    

    void ObjListAdd();
}
