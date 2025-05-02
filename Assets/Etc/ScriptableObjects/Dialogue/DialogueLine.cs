using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;             // �����, �˻� ��    
    [TextArea(2, 5)]
    public string line;                   // ���� �� ���

    public AudioClip audioClip;           // ��� ���� ����

    public Color nameColor = new Color(0f, 0f, 0f, 1f); // ������ + ������
}
