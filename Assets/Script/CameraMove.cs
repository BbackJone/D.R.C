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

    public void CameraLerp(CAMERAPOS _camerapos)
    {
        StopCoroutine("CoCameraLerp");

        if (_camerapos == CAMERAPOS.NORMALPOS)
            StartCoroutine("CoCameraLerp", m_NormalPos);
        else if (_camerapos == CAMERAPOS.SNIPER_SHOOTPOS)
            StartCoroutine("CoCameraLerp", m_SniperPos);
    }

    public IEnumerator CoCameraLerp(Transform _destpos)
    {
        while(true)
        {
            transform.position = Vector3.Lerp(transform.position, _destpos.position, Time.deltaTime * 5f);

            if (transform.position == _destpos.position)
                StopCoroutine("CoCameraLerp");
            yield return null;
        }
    }
}
