using System.Collections;
using UnityEngine;

public class SlidingDoor : DoorBase
{
    public Vector3 slideOffset = new Vector3(1.5f, 0f, 0f);
    public float slideSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    protected override void Awake()
    {
        base.Awake();
        closedPosition = transform.position;
        openPosition = closedPosition + slideOffset;
    }

    protected override IEnumerator OpenCoroutine()
    {
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = openPosition;
    }
}
