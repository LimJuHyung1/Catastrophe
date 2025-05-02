using UnityEngine;

[CreateAssetMenu(fileName = "NewEvidenceData", menuName = "EvidenceData/Evidence Scriptable Object")]
public class EvidenceSO : ScriptableObject
{
    public EvidenceData[] evidenceDatas;
}
