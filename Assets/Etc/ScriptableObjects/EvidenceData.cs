using UnityEngine;

[System.Serializable]
public class EvidenceData
{
    public string koreanEvidenceName;             // ���� �̸� (��: "���� ���� Į")
    public string englishEvidenceName;            // ���� �̸� (��: "Bloodstained Knife")

    [TextArea(2, 5)]
    public string description;              // ���� ���� (ȹ�� �� ������ ����)

    public Sprite icon;                     // UI�� ǥ���� �̹���

    public AudioClip playerAudioClip;       // ���Ÿ� �߰����� �� �÷��̾��� ����
    public string playerLine;               // ���Ÿ� �߰����� �� �÷��̾��� ���
}