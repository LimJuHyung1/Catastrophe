using UnityEngine;
using System.Collections;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueScript dialogueScript;
    private AudioSource audioSource;  // ����� ����� AudioSource

    private void Start()
    {
        if (dialogueScript == null)
        {
            Debug.LogError("DialogueScript�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(CheckAndPlayDialogueLines(dialogueScript));
    }

    private IEnumerator CheckAndPlayDialogueLines(DialogueScript script)
    {
        for (int i = 0; i < script.dialogueLines.Length; i++)
        {
            DialogueLine line = script.dialogueLines[i];

            if (string.IsNullOrWhiteSpace(line.line))
            {
                Debug.LogWarning($"[{i}] '{line.characterName}'�� ��簡 ����ֽ��ϴ�.");
            }

            if (line.audioClip == null)
            {
                Debug.LogWarning($"[{i}] '{line.characterName}'�� ����� Ŭ���� ����ֽ��ϴ�.");
                continue; // ����� Ŭ���� ������ ������� ����
            }

            Debug.Log($"[{i}] '{line.characterName}' ���: {line.line}");

            // ����� Ŭ�� ���
            audioSource.clip = line.audioClip;
            audioSource.Play();

            // ����� Ŭ�� ���̸�ŭ ���
            yield return new WaitForSeconds(audioSource.clip.length + 0.5f); // 0.5�� ����
        }

        Debug.Log("DialogueScript ����� �׽�Ʈ �Ϸ�!");
    }
}
