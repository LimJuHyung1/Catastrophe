using UnityEngine;
using System.Collections;

public class BrokenLight : MonoBehaviour
{
    private Light flickerLight; // ���� Light ������Ʈ
    public float minFlickerTime = 0.1f; // �ּ� ������ �ð�
    public float maxFlickerTime = 0.5f; // �ִ� ������ �ð�

    void Start()
    {
        flickerLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()

    {
        while (true)
        {
            flickerLight.enabled = !flickerLight.enabled; // On/Off ��ȯ
            float flickerDelay = Random.Range(minFlickerTime, maxFlickerTime);
            yield return new WaitForSeconds(flickerDelay);
        }
    }
}
