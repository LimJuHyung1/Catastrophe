using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float rayDistance = 3f; // Ray�� �ִ� �Ÿ�
    private LayerMask targetLayers;
    private Camera mainCamera;
    void Awake()
    {
        mainCamera = Camera.main;
        targetLayers = LayerMask.GetMask("NPC", "Evidence"); // Awake���� ����
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

        Debug.Log("No Object Hit");
        return null;
    }
}
