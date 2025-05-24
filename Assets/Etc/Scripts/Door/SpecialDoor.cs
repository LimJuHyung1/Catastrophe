using System.Collections;
using UnityEngine;

public class SpecialDoor : DoorBase
{
    [SerializeField] private float openAngle = 120;
    private float openSpeed = 80f;

    public HospitalManager hospitalManager;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Open()
    {
        if (isOpened) return;
        isOpened = true;


        switch (gameObject.tag)
        {
            case "Foyer":
                hospitalManager.StartFoyerDialogue();
                break;
            case "Herny's Office":
                StartCoroutine(OpenCoroutine());
                hospitalManager.StartHernyOfficeDialogue();
                break;
            case "Administrative Office":
                StartCoroutine(OpenCoroutine());
                hospitalManager.StartAdministrativeOfficeDialogue();
                break;
            case "Strategic Planning Office":
                hospitalManager.StartStrategicPlanningOfficeDialogue(this);
                break;
        }

        boxCollider.enabled = false; // Disable the trigger collider after opening the door
    }

    public void StartOpenCoroutine()
    {
        StartCoroutine(OpenCoroutine());
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
