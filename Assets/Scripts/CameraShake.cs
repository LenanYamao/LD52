using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    CinemachineBasicMultiChannelPerlin noise;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void startShake(float shakeIntensity = 5f, float shakeTiming = 0.5f)
    {
        StartCoroutine(coroutineShake(shakeIntensity, shakeTiming));
    }

    public IEnumerator coroutineShake(float shakeIntensity, float shakeTiming)
    {
        shake(shakeIntensity, shakeTiming);
        yield return new WaitForSeconds(shakeTiming);
        shake(0, 0);
    }

    public void shake(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }
}
