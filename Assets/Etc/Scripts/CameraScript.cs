using UnityEngine;
using System.Collections;
using System;


public class CameraScript : MonoBehaviour
{
    private float rayDistance = 3f; // Ray의 최대 거리
    private LayerMask targetLayers;
    private Camera mainCamera;

    private float zoomInFOV = 40f;   // 줌 인 할 때 목표 FOV
    private float zoomOutFOV = 60f;  // 줌 아웃 할 때 목표 FOV
    private float zoomSpeed = 5f;    // 줌 속도
    private Coroutine zoomCoroutine;                 // 현재 실행 중인 코루틴 저장용

    void Awake()
    {
        mainCamera = Camera.main;
        targetLayers = LayerMask.GetMask("NPC", "Evidence", "Door"); // Awake에서 설정
    }




    /// <summary>
    /// Player 스크립트에서 호출
    /// </summary>
    /// <param 플레이어 transform="playerTrans"></param>
    public void FocusNPC(Transform playerTrans)
    {
        Vector3 direction = playerTrans.forward;

        // 카메라가 바라보는 방향의 y좌표를 살짝 낮춥니다.
        direction.y -= 0.3f;

        // 새로운 방향 벡터를 사용하여 카메라 회전 설정
        transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public GameObject RaycastFromCamera()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return null;
        }

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, ~0, QueryTriggerInteraction.Ignore); // 모든 레이어 감지
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            GameObject hitObj = hit.collider.gameObject;
            int layer = hitObj.layer;

            if (((1 << layer) & targetLayers) != 0)
            {
                // 타겟 오브젝트면 반환
                return hitObj;
            }
            else
            {
                // 타겟이 아니고 막고 있는 오브젝트 → 가려졌다고 판단
                break;
            }
        }

        return null;
    }

    public void ZoomTo(bool isZoomIn)
    {
        float targetFOV = isZoomIn ? zoomInFOV : zoomOutFOV;

        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }

        zoomCoroutine = StartCoroutine(SmoothZoom(targetFOV));
    }

    private IEnumerator SmoothZoom(float targetFOV)
    {
        float startFOV = mainCamera.fieldOfView;
        float time = 0f;

        while (Mathf.Abs(mainCamera.fieldOfView - targetFOV) > 0.01f)
        {
            time += Time.deltaTime * zoomSpeed;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, time);
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV; // 정확하게 목표 FOV에 맞춰 고정
        zoomCoroutine = null;
    }
}
