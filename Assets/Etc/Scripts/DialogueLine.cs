using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;             // 수사관, 검사 등    
    [TextArea(2, 5)]
    public string line;                   // 여러 줄 대사

    public AudioClip audioClip;           // 대사 음성 파일
}
