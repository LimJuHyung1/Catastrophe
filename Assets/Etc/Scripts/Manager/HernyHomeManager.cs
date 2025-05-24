using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HernyHomeManager : MonoBehaviour
{
    public DialogueScript dialogueScript;
    public GameObject dialogue;
    public Transform startPosition;

    public Sophia sophia;
    public Player player;
    public GameObject chair;

    public Image[] slides;
    public Image screen;

    private float typeSpeed = 0.065f; // 한 글자 출력 간격 (조정 가능)
    private float duration = 1.0f;     // 이동에 걸리는 시간

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

        npcName = dialogue.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        line = dialogue.transform.GetChild(1).GetChild(1).GetComponent<Text>();

        player.transform.position = startPosition.position; // 플레이어 위치 초기화
        player.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    private void Start()
    {
        if (dialogueScript != null)
        {
            // 실제로 할 때 아래 코루틴 실행해야 함
            StartCoroutine(PlayDialogueAudioClips());
            // sophia.SitOnAChair(chair.transform); // NPC 대화 종료
        }
    }

    private IEnumerator PlayDialogueAudioClips()
    {
        player.IsTalking = true; // 대화 시작 시 플레이어 대화 상태 설정
        dialogue.SetActive(true);
        StartCoroutine(sophia.TurnTowardPlayer(player.transform)); // NPC가 플레이어를 바라보도록 설정
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));        
        player.LookAtPosition(sophia.transform.GetChild(0).transform); // 플레이어가 NPC를 바라보도록 설정

        yield return new WaitForSeconds(1f); // NPC가 플레이어를 바라보는 시간

        Slide(true);

        int sophiaAnimIndex = 0;

        foreach (DialogueLine line in dialogueScript.dialogueLines)
        {            

            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();

                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (npcName.text == "소피아")
                {
                    sophia.PlayDialogueAnimation(sophiaAnimIndex++);
                    sophia.StartTalking();                    
                }                    
                else sophia.StopTalking();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

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
        npcName.text = "수잔";

        Slide(true);

        Debug.LogWarning(playerLine);
        StartCoroutine(TypeLine(playerLine));
        audioSource.clip = playerAudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(playerAudioClip.length + 0.5f);

        Slide(false);

        player.IsTalking = false; // 독백 종료 시 증거 탐색 가능하게 설정
    }





    private void FadeDialogue(bool isTalking, float duration = 1f)
    {
        if (isTalking)
        {
            StartCoroutine(FadeUtility.Instance.FadeIn(npcName, duration));
            StartCoroutine(FadeUtility.Instance.FadeIn(line, duration));
        }
        else
        {
            StartCoroutine(FadeUtility.Instance.FadeOut(npcName, duration));
            StartCoroutine(FadeUtility.Instance.FadeOut(line, duration));
        }
    }
    public void Slide(bool isTalking, float fadeDuration = 1f)
    {
        SlideMove(0, isTalking, fadeDuration);
        SlideMove(1, isTalking, fadeDuration);
    }
    private void SlideMove(int index, bool isOn, float fadeDuration)
    {
        if (index < 0 || index >= slides.Length)
        {
            Debug.LogWarning("Slide index out of range.");
            return;
        }

        RectTransform tmp = slides[index].GetComponent<RectTransform>();
        float y = isOn ? 0f : (index == 0 ? tmp.rect.height : -tmp.rect.height);  // 방향에 따라 위치 계산
        Vector2 targetPosition = new Vector2(tmp.anchoredPosition.x, y);

        StartCoroutine(SmoothMove(tmp, tmp.anchoredPosition, targetPosition, isOn, fadeDuration));
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 startPos, Vector2 endPos, bool isOn, float fadeDuration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        rect.anchoredPosition = endPos;
        FadeDialogue(isOn, fadeDuration);
    }

}
