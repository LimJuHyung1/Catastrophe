using UnityEngine;
using System.Collections;

public abstract class BaseLight : MonoBehaviour
{
    protected Light flickerLight; // ���� Light ������Ʈ
    public float onDuration = 0.5f; // ���� ���� �ִ� �ð�
    public float offDuration = 0.5f; // ���� ���� �ִ� �ð�

    protected virtual void Start()
    {
        flickerLight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    protected abstract IEnumerator Flicker(); // ���� Ŭ�������� �����ؾ� ��
}
