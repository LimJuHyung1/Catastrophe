using System.Collections;
using UnityEngine;

public class HingedDoor : DoorBase
{
    private float openAngle = -120;
    private float openSpeed = 80f;

    protected override void Awake()
    {
        base.Awake();        
    }

    protected override IEnumerator OpenCoroutine()
    {
        float targetAngle = transform.eulerAngles.y + openAngle;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.1f)
        {
            float newYRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, openSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
            yield return null;
        }

        Vector3 finalRotation = transform.eulerAngles;
        finalRotation.y = targetAngle;
        transform.eulerAngles = finalRotation;
    }
}
