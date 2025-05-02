using System.Collections;
using UnityEngine;

public class SpecialDoor : DoorBase
{
    public HospitalManager hospitalManager;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Open()
    {
        if (isOpened) return;
        isOpened = true;

        hospitalManager.StartFoyerDialogue();
        boxCollider.enabled = false; // Disable the trigger collider after opening the door
    }


    protected override IEnumerator OpenCoroutine()
    {
        float targetAngle = transform.eulerAngles.y;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.1f)
        {
            float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
            yield return null;
        }

        Vector3 finalRotation = transform.eulerAngles;
        finalRotation.y = targetAngle;
        transform.eulerAngles = finalRotation;
    }

}
