using UnityEngine;
using System.Collections;

public class CertainLight : BaseLight
{
    protected override IEnumerator Flicker()
    {
        while (true)
        {
            flickerLight.enabled = true;
            yield return new WaitForSeconds(onDuration);

            flickerLight.enabled = false;
            yield return new WaitForSeconds(offDuration);
        }
    }
}
