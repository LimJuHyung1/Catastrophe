using UnityEngine;
using System.Collections;

public class DialogueTester : MonoBehaviour
{
    [SerializeField] private DialogueScript dialogueScript;
    private AudioSource audioSource;  // 오디오 재생용 AudioSource

    private void Start()
    {
        if (dialogueScript == null)
        {
            Debug.LogError("DialogueScript가 할당되지 않았습니다!");
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
                Debug.LogWarning($"[{i}] '{line.characterName}'의 대사가 비어있습니다.");
            }

            if (line.audioClip == null)
            {
                Debug.LogWarning($"[{i}] '{line.characterName}'의 오디오 클립이 비어있습니다.");
                continue; // 오디오 클립이 없으면 재생하지 않음
            }

            Debug.Log($"[{i}] '{line.characterName}' 대사: {line.line}");

            // 오디오 클립 재생
            audioSource.clip = line.audioClip;
            audioSource.Play();

            // 오디오 클립 길이만큼 대기
            yield return new WaitForSeconds(audioSource.clip.length + 0.5f); // 0.5초 여유
        }

        Debug.Log("DialogueScript 오디오 테스트 완료!");
    }
}
