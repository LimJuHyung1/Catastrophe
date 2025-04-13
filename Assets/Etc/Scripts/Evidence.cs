using UnityEngine;

public class Evidence : MonoBehaviour
{
    [SerializeField] private string koreanEvidenceName;             // 증거 이름
    public string KoreanEvidenceName { get => koreanEvidenceName; set => koreanEvidenceName = value; }

    [SerializeField] private string englishEvidenceName;             // 증거 이름
    public string EnglishEvidenceName { get => englishEvidenceName; set => englishEvidenceName = value; }

    [SerializeField] private string description;              // 증거 설명
    public string Description { get => description; set => description = value; }

    [SerializeField] private Sprite icon;                     // UI 이미지
    public Sprite Icon { get => icon; set => icon = value; }

    [SerializeField] private AudioClip playerAudioClip;       // 플레이어 음성
    public AudioClip PlayerAudioClip { get => playerAudioClip; set => playerAudioClip = value; }

    [SerializeField] private string playerLine;               // 플레이어 대사
    public string PlayerLine { get => playerLine; set => playerLine = value; }



    private bool isFound = false; // 증거 발견 여부
    public bool IsFound { get => isFound; set => isFound = value; }

    private HernyHomeManager hernyHomeManager;
    public HernyHomeManager HernyHomeManager { set => hernyHomeManager = value; }




    /// <summary>
    /// Evidence Manager에서 Start-Initialize 전에 호출되어야 함
    /// </summary>
    void Awake()
    {
        EnglishEvidenceName = gameObject.name; // 게임 오브젝트 이름을 영어 증거 이름으로 설정
    }


    public void FindEvidence()
    {
        // HernyHomeManager의 FindEvidence 메서드 호출
        isFound = true; // 증거 발견 상태로 변경
        StartCoroutine(hernyHomeManager.FindEvidence(PlayerLine, this.playerAudioClip));
    } 
}
