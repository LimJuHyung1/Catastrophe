using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HernyHomeManager : MonoBehaviour
{
    public float typeSpeed = 0.065f; // 한 글자 출력 간격 (조정 가능)

    public HernyHomeDialogueScript dialogueScript;
    public GameObject dialogue;    

    public Sophia sophia;
    public Player player;
    public GameObject chair;

    public Image screen;    
    
    private AudioSource audioSource;
    private Text npcName;
    private Text line;    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        dialogue.SetActive(false);
        npcName = dialogue.transform.GetChild(0).GetComponent<Text>();
        line = dialogue.transform.GetChild(1).GetComponent<Text>();
    }

    private void Start()
    {
        if (dialogueScript != null)
        {
            // 실제로 할 때 아래 코루틴 실행해야 함
            // StartCoroutine(PlayDialogueAudioClips());
            sophia.SitOnAChair(chair.transform); // NPC 대화 종료
        }
    }

    private IEnumerator PlayDialogueAudioClips()
    {
        player.IsTalking = true; // 대화 시작 시 플레이어 대화 상태 설정
        sophia.TurnTowardPlayer(player.transform); // NPC가 플레이어를 바라보도록 설정
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));        
        player.LookAtPosition(sophia.transform.GetChild(0).transform); // 플레이어가 NPC를 바라보도록 설정

        yield return new WaitForSeconds(1f); // NPC가 플레이어를 바라보는 시간

        FadeUtility.Instance.FadeIn(npcName, 1f);
        FadeUtility.Instance.FadeIn(line, 1f);

        foreach (DialogueLine line in dialogueScript.dialogueLines)
        {
            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();

                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (npcName.text == "소피아")
                    sophia.StartTalking();
                else sophia.StopTalking();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        FadeUtility.Instance.FadeOut(npcName, 2f);
        FadeUtility.Instance.FadeOut(line, 2f);        
        yield return FadeUtility.Instance.FadeIn(screen, 2f); // 대화가 끝나면 화면 페이드 인
        sophia.SitOnAChair(chair.transform); // NPC 대화 종료
        yield return FadeUtility.Instance.FadeOut(screen, 2f); // 대화가 끝나면 화면 페이드 인        
        
        player.IsTalking = false; // 대화 종료 시 플레이어 대화 상태 설정        
    }

    private IEnumerator TypeLine(string sentence)
    {
        line.text = ""; // 기존 텍스트 초기화

        foreach (char c in sentence)
        {
            line.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public IEnumerator FindEvidence(string playerLine, AudioClip playerAudioClip)
    {
        dialogue.SetActive(true); // 독백 시작

        npcName.text = "수잔";
        Debug.LogWarning(playerLine);
        StartCoroutine(TypeLine(playerLine));
        audioSource.clip = playerAudioClip;
        audioSource.Play();
        
        yield return new WaitForSeconds(playerAudioClip.length + 0.5f);

        dialogue.SetActive(false); // 독백 종료
        player.IsTalking = false; // 독백 종료 시 증거 탐색 가능하게 설정
    }
}
