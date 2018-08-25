using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERAPOS
{
    SNIPER_SHOOTPOS,
    NORMALPOS
}

public class CameraMove : MonoBehaviour {

    public Transform m_SniperPos;
    public Transform m_NormalPos;

    private PlayerData m_PlayerData;

    public void Awake()
    {
        m_PlayerData = transform.parent.GetComponent<PlayerData>();
    }

    public void CameraLerp(CAMERAPOS _camerapos)
    {
        StopCoroutine("CoCameraLerp");

        if (_camerapos == CAMERAPOS.NORMALPOS)
        {
            StopCoroutine("CoSniperCameraLerp");
            StartCoroutine("CoCameraLerp", m_NormalPos);
        }
        else if (_camerapos == CAMERAPOS.SNIPER_SHOOTPOS)
            StartCoroutine("CoSniperCameraLerp");
    }

    public IEnumerator CoCameraLerp(Transform _destpos)
    {
        Invoke("StopCameraLerp", 2f);
        while(true)
        {
            transform.position = Vector3.Lerp(transform.position, _destpos.position, 0.1f);
            
            yield return null;
        }
    }

    public void StopCameraLerp()
    {
        StopCoroutine("CoCameraLerp");
    }

    public IEnumerator CoSniperCameraLerp()
    {
        while (true)
        {
            transform.position = Vector3.Lerp(m_SniperPos.position, m_NormalPos.position, (m_PlayerData.m_Move.magnitude/2f));
            yield return null;
        }
    }
}
