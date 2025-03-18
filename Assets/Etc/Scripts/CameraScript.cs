using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float rayDistance = 3f; // Ray의 최대 거리
    private LayerMask targetLayers;
    private Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
        targetLayers = LayerMask.GetMask("NPC", "Evidence"); // Awake에서 설정
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
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 3f);

        // 여러 개의 레이어 감지 가능
        if (Physics.Raycast(ray, out hit, rayDistance, targetLayers, QueryTriggerInteraction.Collide))
        {
            Debug.Log($"Hit Object: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            return hit.collider.gameObject;
        }

        Debug.Log("No Object Hit");
        return null;
    }
}
