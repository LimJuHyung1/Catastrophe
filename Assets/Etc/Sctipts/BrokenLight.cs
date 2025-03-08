using UnityEngine;
using System.Collections;

public class BrokenLight : MonoBehaviour
{
    private Light flickerLight; // 전구 Light 컴포넌트
    public float minFlickerTime = 0.1f; // 최소 깜빡임 시간
    public float maxFlickerTime = 0.5f; // 최대 깜빡임 시간

    void Start()
    {
        flickerLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()

    {
        while (true)
        {
            flickerLight.enabled = !flickerLight.enabled; // On/Off 전환
            float flickerDelay = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(flickerDelay);
        }
    }
}
