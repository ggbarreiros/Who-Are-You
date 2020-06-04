using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineBasicMultiChannelPerlin myNoiseProfile;
    private CinemachineVirtualCamera vcam;
    public NoiseSettings normalProfile, cameraShake;

    public float amplitude, frequency;

    private void Awake()
    {
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        //vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = normalProfile;
        //vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = normalProfile;
    }

    public void Shacking()
    {
        StartCoroutine(ShakeCam());
    }

    public IEnumerator ShakeCam()
    {
        //vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = cameraShake;
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        yield return new WaitForSeconds(0.08f);
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
}
