using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public Evidence[] evidences; // ���� ������ �迭
    public EvidenceSO evidenceData;
    public MonoBehaviour sceneManager; // HernyHomeManager ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // 1. EvidenceData���� ��ųʸ��� ��ȯ (Ű: englishEvidenceName)
        var dataMap = new Dictionary<string, EvidenceData>();
        foreach (var data in evidenceData.evidenceDatas)
        {
            if (!dataMap.ContainsKey(data.englishEvidenceName))
                dataMap.Add(data.englishEvidenceName, data);
            else
                Debug.LogWarning($"�ߺ��� ���� �̸�: {data.englishEvidenceName}");
        }

        // 2. ������ �����Ǵ� �����ͷ� Evidence �ʱ�ȭ
        foreach (var targetEvidence in evidences)
        {
            if (dataMap.TryGetValue(targetEvidence.EnglishEvidenceName, out var tmpData))
            {
                targetEvidence.KoreanEvidenceName = tmpData.koreanEvidenceName;
                targetEvidence.Description = tmpData.description;
                targetEvidence.Icon = tmpData.icon;
                targetEvidence.PlayerAudioClip = tmpData.playerAudioClip;
                targetEvidence.PlayerLine = tmpData.playerLine;
                targetEvidence.SceneManager = sceneManager;

                Debug.Log($"Evidence �ʱ�ȭ: {tmpData.koreanEvidenceName} ({tmpData.englishEvidenceName})");
            }
            else
            {
                Debug.LogWarning($"�����Ǵ� EvidenceData�� �����ϴ�: {targetEvidence.EnglishEvidenceName}");
            }
        }
    }

}
