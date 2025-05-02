using System.Collections.Generic;
using UnityEngine;

public class EvidenceManager : MonoBehaviour
{
    public Evidence[] evidences; // 증거 데이터 배열
    public EvidenceSO evidenceData;
    public MonoBehaviour sceneManager; // HernyHomeManager 참조

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        // 1. EvidenceData들을 딕셔너리로 변환 (키: englishEvidenceName)
        var dataMap = new Dictionary<string, EvidenceData>();
        foreach (var data in evidenceData.evidenceDatas)
        {
            if (!dataMap.ContainsKey(data.englishEvidenceName))
                dataMap.Add(data.englishEvidenceName, data);
            else
                Debug.LogWarning($"중복된 증거 이름: {data.englishEvidenceName}");
        }

        // 2. 빠르게 대응되는 데이터로 Evidence 초기화
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

                Debug.Log($"Evidence 초기화: {tmpData.koreanEvidenceName} ({tmpData.englishEvidenceName})");
            }
            else
            {
                Debug.LogWarning($"대응되는 EvidenceData가 없습니다: {targetEvidence.EnglishEvidenceName}");
            }
        }
    }

}
