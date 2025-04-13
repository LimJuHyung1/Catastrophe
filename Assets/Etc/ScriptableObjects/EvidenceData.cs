using UnityEngine;

[System.Serializable]
public class EvidenceData
{
    public string koreanEvidenceName;             // 증거 이름 (예: "혈흔 묻은 칼")
    public string englishEvidenceName;            // 증거 이름 (예: "Bloodstained Knife")

    [TextArea(2, 5)]
    public string description;              // 증거 설명 (획득 시 보여줄 정보)

    public Sprite icon;                     // UI에 표시할 이미지

    public AudioClip playerAudioClip;       // 증거를 발견했을 때 플레이어의 음성
    public string playerLine;               // 증거를 발견했을 때 플레이어의 대사
}