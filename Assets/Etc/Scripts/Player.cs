using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using AdvancedPeopleSystem;

public class Player : MonoBehaviour
{
    public ConversationManager conversationManager;
    public GameObject clickMark;
    public GameObject FMark;

    [SerializeField] private bool isTalking = false; // ��ȭ ������ ����
    public bool IsTalking
    {
        get { return isTalking; }
        set { isTalking = value; }
    }



    [SerializeField] private float moveSpeed = 3.5f;  // �̵� �ӵ�
    [SerializeField] private float lookSensitivity = 0.15f; // ���콺 ����
    private float velocityY = 0f; // Y�� �ӵ� (���� & �߷�)

    private float footstepTimer = 0f; // �߼Ҹ� Ÿ�̸�
    private float footstepDelay = 0.65f; // �߼Ҹ� ���� (�� ����)

    private Vector2 moveInput; // �̵� �Է� ��
    private Vector2 lookInput; // ���콺 �Է� ��

    private Animator animator;
    private AudioSource audioSource;
    private CameraScript cam;
    private CharacterController controller;
    private CharacterCustomization cc;
    private Transform cameraTransform;
    private float rotationY = 0f; // ī�޶� ȸ�� �� (���Ʒ�)


    private float rotateSpeed = 2.0f;        // ȸ�� �ӵ�
    private float stopThreshold = 0.01f;     // ȸ�� ���� ���� (���� ����)    
    private Coroutine lookCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam = transform.GetChild(0).GetComponent<CameraScript>();
        controller = GetComponent<CharacterController>();
        cc = GetComponent<CharacterCustomization>();
        cc.InitColors();
        cameraTransform = Camera.main.transform; // ���� ī�޶� ��������
        SetActiveMark(false);
    }

    void Update()
    {
        if (!isTalking && !conversationManager.GetIsTalking())
        {
            Move();
            Look();


            if (cam.RaycastFromCamera() != null)
            {
                if (cam.RaycastFromCamera().layer == LayerMask.NameToLayer("Evidence")
                    && !cam.RaycastFromCamera().GetComponent<Evidence>().IsFound)
                    clickMark.gameObject.SetActive(true);
                else if (cam.RaycastFromCamera().layer == LayerMask.NameToLayer("Door")
                    && !cam.RaycastFromCamera().GetComponent<DoorBase>().GetIsOpened())
                {
                    FMark.gameObject.SetActive(true);                    
                }                    

                // ���콺 ���� ��ư Ŭ�� �� Raycast ����
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    int tmpLayer = cam.RaycastFromCamera().layer;
                    if (tmpLayer == LayerMask.NameToLayer("NPC"))
                    {
                        conversationManager.GetNPCRole(cam.RaycastFromCamera().GetComponent<NPCRole>());
                        conversationManager.StartConversation();
                    }
                    // ���� ���̾��̰� ���Ű� �߰ߵ��� ���� ���
                    else if (tmpLayer == LayerMask.NameToLayer("Evidence")
                        && !cam.RaycastFromCamera().GetComponent<Evidence>().IsFound)
                    {
                        isTalking = true; // ���� ����
                        clickMark.gameObject.SetActive(false);
                        cam.RaycastFromCamera().GetComponent<Evidence>().FindEvidence();
                    }
                }

                if (Input.GetKey(KeyCode.F))
                {
                    FMark.gameObject.SetActive(false);
                    cam.RaycastFromCamera().GetComponent<DoorBase>().Open();
                }
            }
            else
            {
                SetActiveMark(false);
            }
        }        
    }

    // �̵� ó�� (CharacterController ���)
    private void Move()
    {
        // ī�޶��� forward ������ �������� (Y���� 0���� ����)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // ���� ���� ����

        // ī�޶� ������ (0,0,0)�� �ƴ� ���� ����
        if (cameraForward.sqrMagnitude > 0.01f)
        {
            transform.forward = cameraForward;
        }

        // �̵� ���� ���� (ī�޶� �������� �̵�)
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // �߷� ����
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f; // �ٴڿ��� ����
        }

        // �̵� ���� ����
        Vector3 velocity = moveDirection * moveSpeed + Vector3.up * velocityY;
        animator.SetBool("IsWalking", velocity.magnitude > 0.01f);

        // �߼Ҹ� ��� ó�� (���� �ð� ���� ����)
        if (moveDirection.sqrMagnitude > 0.01f && controller.isGrounded) // �̵� �� + ���� ���� ����
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepDelay) // ������ �ð����� �߼Ҹ� ���
            {
                audioSource.Play();
                footstepTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            }
        }
        else
        {
            footstepTimer = 0f; // �������� ���� �� Ÿ�̸� �ʱ�ȭ
            audioSource.Stop();
        }

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

    /// <summary>
    /// �÷��̾ NPC �������� ȸ��
    /// - `Vector3.ProjectOnPlane()`�� ����Ͽ� y�� ȸ���� ����
    /// </summary>
    public void TurnTowardNPC(Transform npcTrans)
    {
        Vector3 direction = npcTrans.position - transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up); // y���� ������� �ʰ� ��鿡�� ���⸸ ����
        transform.rotation = Quaternion.LookRotation(direction);

        cameraTransform.GetComponent<CameraScript>().FocusNPC(this.transform);
    }

    /// <summary>
    /// ��ȭ �غ� ���·� ���� (�̵��� ���߰� ����)
    /// </summary>
    public void ReadyConversation()
    {
        // move = Vector3.zero;
        animator.SetBool("IsWalking", false);
    }






    /// <summary>
    /// ������ ��ġ�� �ε巴�� �ٶ󺾴ϴ�.
    /// </summary>
    /// <param name="targetPosition">�ٶ� ��ġ</param>
    public void LookAtPosition(Transform targetPosition)
    {
        // ���� ȸ�� ���̸� �ߴ�
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);

        lookCoroutine = StartCoroutine(SmoothLookAt(targetPosition));
    }

    private IEnumerator SmoothLookAt(Transform targetPosition)
    {
        Vector3 direction = targetPosition.position - cam.transform.position;
        if (direction == Vector3.zero) yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(cam.transform.rotation, targetRotation) > stopThreshold)
        {
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        cam.transform.rotation = targetRotation; // ��Ȯ�� ����
        lookCoroutine = null;
    }

    private void SetActiveMark(bool state)
    {
        clickMark.SetActive(state);
        FMark.SetActive(state);
    }
}
