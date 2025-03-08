using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float moveSpeed = 5f;       // 이동 속도
    public float mouseSensitivity = 2f; // 마우스 감도

    private float rotationY = 0f; // 위아래 회전 값
    public Transform cameraTransform; // 카메라 Transform (Inspector에서 할당)

    void Start()
    {
        // 마우스 커서 숨기기 & 고정 (FPS 스타일)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 방향키 이동 처리
        float moveX = Input.GetAxis("Horizontal"); // 좌우 이동 (A, D 또는 ←, →)
        float moveZ = Input.GetAxis("Vertical");   // 앞뒤 이동 (W, S 또는 ↑, ↓)

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 마우스 회전 처리
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // 플레이어(오브젝트) 좌우 회전 (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // 카메라 위아래 회전 (Pitch)
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // 위아래 90도 제한

        // 카메라의 로컬 회전 적용
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }
}
