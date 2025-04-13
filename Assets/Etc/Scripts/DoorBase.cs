using UnityEngine;

public abstract class DoorBase : MonoBehaviour
{
    [SerializeField] protected bool isOpened = false;
    protected AudioSource audio;

    protected virtual void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public virtual void Open()
    {
        if (isOpened) return;
        isOpened = true;

        if (audio != null)
            audio.Play();

        StartCoroutine(OpenCoroutine());
    }

    protected abstract System.Collections.IEnumerator OpenCoroutine();

    public bool GetIsOpened()
    {
        return isOpened;
    }
}
