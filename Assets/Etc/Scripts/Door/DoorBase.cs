using UnityEngine;

public abstract class DoorBase : MonoBehaviour
{
    [SerializeField] protected bool isOpened = false;
    protected AudioSource audio;
    protected BoxCollider boxCollider;

    protected virtual void Awake()
    {
        audio = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.isTrigger = true; // Ensure the collider is a trigger
    }

    public virtual void Open()
    {
        if (isOpened) return;
        isOpened = true;

        if (audio != null)
            audio.Play();

        StartCoroutine(OpenCoroutine());
        boxCollider.enabled = false; // Disable the trigger collider after opening the door
    }

    protected abstract System.Collections.IEnumerator OpenCoroutine();

    public bool GetIsOpened()
    {
        return isOpened;
    }
}
