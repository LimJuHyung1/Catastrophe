using UnityEngine;
using System.Collections;

public abstract class BaseLight : MonoBehaviour
{
    protected Light flickerLight; // 전구 Light 컴포넌트
    public float onDuration = 0.5f; // 빛이 켜져 있는 시간
    public float offDuration = 0.5f; // 빛이 꺼져 있는 시간

    protected virtual void Start()
    {
        flickerLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    protected abstract IEnumerator Flicker(); // 하위 클래스에서 구현해야 함
}
