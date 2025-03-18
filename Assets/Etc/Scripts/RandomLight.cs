using UnityEngine;
using System.Collections;

public class RandomLight : BaseLight
{
    public float minFlickerTime = 0.1f;
    public float maxFlickerTime = 0.5f;

    protected override IEnumerator Flicker()
    {
        while (true)
        {
            flickerLight.enabled = !flickerLight.enabled; // ·£´ý On/Off ÀüÈ¯
            float flickerDelay = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(flickerDelay);
        }
    }
}
