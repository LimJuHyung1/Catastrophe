using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // �̵� �ӵ�
    [SerializeField] private float lookSensitivity = 0.15f; // ���콺 ����
    private float velocityY = 0f; // Y�� �ӵ� (���� & �߷�)
    private float rayDistance = 3f; // Ray�� �ִ� �Ÿ�

    private Vector2 moveInput; // �̵� �Է� ��
    private Vector2 lookInput; // ���콺 �Է� ��
    private CharacterController controller;
    private Transform cameraTransform;
    private float rotationY = 0f; // ī�޶� ȸ�� �� (���Ʒ�)

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // ���� ī�޶� ��������

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Look();

        // ���콺 ���� ��ư Ŭ�� �� Raycast ����
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {            
            RaycastFromCamera();
        }
    }

    // �̵� ó�� (CharacterController ���)
    private void Move()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // �߷� ����
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f; // �ٴڿ��� ����
        }

        Vector3 velocity = moveDirection * moveSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    // ���콺 ȸ�� ó��
    private void Look()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }

    // Input System���� �̵� �Է� �ޱ�
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input System���� ���콺 �Է� �ޱ�
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void RaycastFromCamera()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        Camera mainCamera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        // Ray ����� ���
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 3f);

        if (Physics.Raycast(ray, out hit, rayDistance, ~0, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Hit Object: " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("No Object Hit");
        }
    }
}
