using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // 이동 속도
    [SerializeField] private float lookSensitivity = 0.15f; // 마우스 감도
    private float velocityY = 0f; // Y축 속도 (점프 & 중력)
    private float rayDistance = 3f; // Ray의 최대 거리

    private Vector2 moveInput; // 이동 입력 값
    private Vector2 lookInput; // 마우스 입력 값
    private CharacterController controller;
    private Transform cameraTransform;
    private float rotationY = 0f; // 카메라 회전 값 (위아래)

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // 메인 카메라 가져오기

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        Look();

        // 마우스 왼쪽 버튼 클릭 시 Raycast 실행
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {            
            RaycastFromCamera();
        }
    }

    // 이동 처리 (CharacterController 사용)
    private void Move()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // 중력 적용
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f; // 바닥에서 리셋
        }

        Vector3 velocity = moveDirection * moveSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    // 마우스 회전 처리
    private void Look()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }

    // Input System에서 이동 입력 받기
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Input System에서 마우스 입력 받기
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

        // Ray 디버깅 출력
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
