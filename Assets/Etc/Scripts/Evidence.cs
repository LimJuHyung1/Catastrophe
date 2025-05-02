using System.Reflection;
using System.Collections;
using UnityEngine;


public class Evidence : MonoBehaviour
{
    [SerializeField] private string koreanEvidenceName;             // ���� �̸�
    public string KoreanEvidenceName { get => koreanEvidenceName; set => koreanEvidenceName = value; }

    [SerializeField] private string englishEvidenceName;             // ���� �̸�
    public string EnglishEvidenceName { get => englishEvidenceName; set => englishEvidenceName = value; }

    [SerializeField] private string description;              // ���� ����
    public string Description { get => description; set => description = value; }

    [SerializeField] private Sprite icon;                     // UI �̹���
    public Sprite Icon { get => icon; set => icon = value; }

    [SerializeField] private AudioClip playerAudioClip;       // �÷��̾� ����
    public AudioClip PlayerAudioClip { get => playerAudioClip; set => playerAudioClip = value; }

    [SerializeField] private string playerLine;               // �÷��̾� ���
    public string PlayerLine { get => playerLine; set => playerLine = value; }



    private bool isFound = false; // ���� �߰� ����
    public bool IsFound { get => isFound; set => isFound = value; }

    private MonoBehaviour sceneManager;
    public MonoBehaviour SceneManager { set => sceneManager = value; }




    /// <summary>
    /// Evidence Manager���� Start-Initialize ���� ȣ��Ǿ�� ��
    /// </summary>
    void Awake()
    {
        EnglishEvidenceName = gameObject.name; // ���� ������Ʈ �̸��� ���� ���� �̸����� ����
    }


    public void FindEvidence()
    {
        if (sceneManager == null)
        {
            Debug.LogError("Ÿ�� ��ũ��Ʈ�� �Ҵ��ϼ���.");
            return;
        }

        // �޼��� ã��
        MethodInfo method = sceneManager.GetType().GetMethod("FindEvidence", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (method != null)
        {
            isFound = true; // ���� �߰� ���·� ����
            object result = method.Invoke(sceneManager, new object[] { PlayerLine, playerAudioClip });

            // �ڷ�ƾ�� ��� StartCoroutine �ؾ� ��
            if (result is IEnumerator coroutine)
            {
                StartCoroutine(coroutine);
            }
        }
        else
        {
            Debug.LogWarning("FindEvidence �޼��带 ã�� �� �����ϴ�.");
        }
    }
}
