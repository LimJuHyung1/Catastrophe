using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject openDoorUI;

    private BoxCollider boxCollider;
    private DoorBase door;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        door = transform.parent.GetComponent<DoorBase>();
    }

    private void OnTriggerStay(Collider other)
    {
        openDoorUI.gameObject.SetActive(true); // Show the UI when the player is in the trigger area
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("F key pressed");
            door.Open();
            openDoorUI.gameObject.SetActive(false);
            boxCollider.enabled = false; // Disable the trigger collider after opening the door
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!door.GetIsOpened())
        {
            openDoorUI.gameObject.SetActive(false);
        }
    }
}
