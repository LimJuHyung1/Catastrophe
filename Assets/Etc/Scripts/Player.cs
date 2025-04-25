using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using AdvancedPeopleSystem;

public class Player : MonoBehaviour
{
    public ConversationManager conversationManager;
    public GameObject clickMark;
    public GameObject FMark;

    [SerializeField] private bool isTalking = false; // 대화 중인지 여부
    public bool IsTalking
    {
        get { return isTalking; }
        set { isTalking = value; }
    }



    [SerializeField] private float moveSpeed = 3.5f;  // 이동 속도
    [SerializeField] private float lookSensitivity = 0.15f; // 마우스 감도
    private float velocityY = 0f; // Y축 속도 (점프 & 중력)

    private float footstepTimer = 0f; // 발소리 타이머
    private float footstepDelay = 0.65f; // 발소리 간격 (초 단위)

    private Vector2 moveInput; // 이동 입력 값
    private Vector2 lookInput; // 마우스 입력 값

    private Animator animator;
    private AudioSource audioSource;
    private CameraScript cam;
    private CharacterController controller;
    private CharacterCustomization cc;
    private Transform cameraTransform;
    private float rotationY = 0f; // 카메라 회전 값 (위아래)


    private float rotateSpeed = 2.0f;        // 회전 속도
    private float stopThreshold = 0.01f;     // 회전 종료 조건 (각도 차이)    
    private Coroutine lookCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cam = transform.GetChild(0).GetComponent<CameraScript>();
        controller = GetComponent<CharacterController>();
        cc = GetComponent<CharacterCustomization>();
        cc.InitColors();
        cameraTransform = Camera.main.transform; // 메인 카메라 가져오기
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

                // 마우스 왼쪽 버튼 클릭 시 Raycast 실행
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    int tmpLayer = cam.RaycastFromCamera().layer;
                    if (tmpLayer == LayerMask.NameToLayer("NPC"))
                    {
                        conversationManager.GetNPCRole(cam.RaycastFromCamera().GetComponent<NPCRole>());
                        conversationManager.StartConversation();
                    }
                    // 증거 레이어이고 증거가 발견되지 않은 경우
                    else if (tmpLayer == LayerMask.NameToLayer("Evidence")
                        && !cam.RaycastFromCamera().GetComponent<Evidence>().IsFound)
                    {
                        isTalking = true; // 독백 시작
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

    // 이동 처리 (CharacterController 사용)
    private void Move()
    {
        // 카메라의 forward 방향을 가져오기 (Y축은 0으로 설정)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // 수직 방향 제거

        // 카메라 방향이 (0,0,0)이 아닐 때만 적용
        if (cameraForward.sqrMagnitude > 0.01f)
        {
            transform.forward = cameraForward;
        }

        // 이동 방향 설정 (카메라 기준으로 이동)
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        // 중력 적용
        if (controller.isGrounded && velocityY < 0)
        {
            velocityY = -2f; // 바닥에서 리셋
        }

        // 이동 벡터 생성
        Vector3 velocity = moveDirection * moveSpeed + Vector3.up * velocityY;
        animator.SetBool("IsWalking", velocity.magnitude > 0.01f);

        // 발소리 재생 처리 (일정 시간 간격 유지)
        if (moveDirection.sqrMagnitude > 0.01f && controller.isGrounded) // 이동 중 + 땅에 있을 때만
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepDelay) // 설정된 시간마다 발소리 재생
            {
                audioSource.Play();
                footstepTimer = 0f; // 타이머 초기화
            }
        }
        else
        {
            footstepTimer = 0f; // 움직이지 않을 때 타이머 초기화
            audioSource.Stop();
        }

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

    /// <summary>
    /// 플레이어가 NPC 방향으로 회전
    /// - `Vector3.ProjectOnPlane()`을 사용하여 y축 회전을 방지
    /// </summary>
    public void TurnTowardNPC(Transform npcTrans)
    {
        Vector3 direction = npcTrans.position - transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up); // y축을 고려하지 않고 평면에서 방향만 설정
        transform.rotation = Quaternion.LookRotation(direction);

        cameraTransform.GetComponent<CameraScript>().FocusNPC(this.transform);
    }

    /// <summary>
    /// 대화 준비 상태로 변경 (이동을 멈추고 정지)
    /// </summary>
    public void ReadyConversation()
    {
        // move = Vector3.zero;
        animator.SetBool("IsWalking", false);
    }






    /// <summary>
    /// 지정한 위치를 부드럽게 바라봅니다.
    /// </summary>
    /// <param name="targetPosition">바라볼 위치</param>
    public void LookAtPosition(Transform targetPosition)
    {
        // 기존 회전 중이면 중단
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

        cam.transform.rotation = targetRotation; // 정확히 고정
        lookCoroutine = null;
    }

    private void SetActiveMark(bool state)
    {
        clickMark.SetActive(state);
        FMark.SetActive(state);
    }
}
