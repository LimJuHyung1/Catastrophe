using UnityEngine;
using System.Collections;


public class CameraScript : MonoBehaviour
{
    private float rayDistance = 3f; // Ray�� �ִ� �Ÿ�
    private LayerMask targetLayers;
    private Camera mainCamera;

    [SerializeField] private float zoomInFOV = 30f;   // �� �� �� �� ��ǥ FOV
    [SerializeField] private float zoomOutFOV = 60f;  // �� �ƿ� �� �� ��ǥ FOV
    [SerializeField] private float zoomSpeed = 5f;    // �� �ӵ�
    private Coroutine zoomCoroutine;                 // ���� ���� ���� �ڷ�ƾ �����

    void Awake()
    {
        mainCamera = Camera.main;
        targetLayers = LayerMask.GetMask("NPC", "Evidence", "Door"); // Awake���� ����
    }




    /// <summary>
    /// Player ��ũ��Ʈ���� ȣ��
    /// </summary>
    /// <param �÷��̾� transform="playerTrans"></param>
    public void FocusNPC(Transform playerTrans)
    {
        Vector3 direction = playerTrans.forward;

        // ī�޶� �ٶ󺸴� ������ y��ǥ�� ��¦ ����ϴ�.
        direction.y -= 0.3f;

        // ���ο� ���� ���͸� ����Ͽ� ī�޶� ȸ�� ����
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
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 3f);

        // ���� ���� ���̾� ���� ����
        if (Physics.Raycast(ray, out hit, rayDistance, targetLayers, QueryTriggerInteraction.Collide))
        {
            Debug.Log($"Hit Object: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            return hit.collider.gameObject;
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

        mainCamera.fieldOfView = targetFOV; // ��Ȯ�ϰ� ��ǥ FOV�� ���� ����
        zoomCoroutine = null;
    }
}
