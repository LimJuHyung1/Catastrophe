using UnityEngine;

public class DoorTrigger : MonoBehaviour
{    
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
        Debug.Log("Player entered the trigger zone");
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log("F key pressed");
            door.Open();
            boxCollider.enabled = false; // Disable the trigger collider after opening the door
        }
    }
}
