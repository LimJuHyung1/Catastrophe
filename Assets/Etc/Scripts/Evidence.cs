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

    private HernyHomeManager hernyHomeManager;
    public HernyHomeManager HernyHomeManager { set => hernyHomeManager = value; }




    /// <summary>
    /// Evidence Manager���� Start-Initialize ���� ȣ��Ǿ�� ��
    /// </summary>
    void Awake()
    {
        EnglishEvidenceName = gameObject.name; // ���� ������Ʈ �̸��� ���� ���� �̸����� ����
    }


    public void FindEvidence()
    {
        // HernyHomeManager�� FindEvidence �޼��� ȣ��
        isFound = true; // ���� �߰� ���·� ����
        StartCoroutine(hernyHomeManager.FindEvidence(PlayerLine, this.playerAudioClip));
    } 
}
