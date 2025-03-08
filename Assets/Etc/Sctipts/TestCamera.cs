using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float moveSpeed = 5f;       // �̵� �ӵ�
    public float mouseSensitivity = 2f; // ���콺 ����

    private float rotationY = 0f; // ���Ʒ� ȸ�� ��
    public Transform cameraTransform; // ī�޶� Transform (Inspector���� �Ҵ�)

    void Start()
    {
        // ���콺 Ŀ�� ����� & ���� (FPS ��Ÿ��)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ����Ű �̵� ó��
        float moveX = Input.GetAxis("Horizontal"); // �¿� �̵� (A, D �Ǵ� ��, ��)
        float moveZ = Input.GetAxis("Vertical");   // �յ� �̵� (W, S �Ǵ� ��, ��)

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // ���콺 ȸ�� ó��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // �÷��̾�(������Ʈ) �¿� ȸ�� (Yaw)
        transform.Rotate(Vector3.up * mouseX);

        // ī�޶� ���Ʒ� ȸ�� (Pitch)
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // ���Ʒ� 90�� ����

        // ī�޶��� ���� ȸ�� ����
        cameraTransform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }
}
