using UnityEngine;

public class Boundary : MonoBehaviour
{
    BoxCollider boxCollider;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("���� ����");
    }
}
