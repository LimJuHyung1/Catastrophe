using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;


public class HospitalManager : MonoBehaviour
{    
    public DialogueScript nurseDialogue;
    public DialogueScript soliloquy;
    public DialogueScript foyerDialogue;
    public DialogueScript hernyOfficeDialogue;
    public DialogueScript administrativeOfficeDialogue;

    public GameObject dialogue;
    public GameObject investigationUI;
    public Transform startPosition;
    public Transform foyerLookTarget;
    public Transform hernyOfficePos;
    public Transform hernyOfficeLookTarget;
    public Transform administrativeOfficePos;

    public Nurse nurse;
    public Player player;
    public AdministrativeOfficer administrativeOfficer;
    public NPCRole administrativeOfficer2;

    public Image[] slides;
    public Image screen;

    private float typeSpeed = 0.065f; // 한 글자 출력 간격 (조정 가능)
    private float duration = 1.0f;     // 이동에 걸리는 시간

    private AudioSource audioSource;
    private Text npcName;
    private Text line;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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

    void Start()
    {
        // StartCoroutine(PlaySoliloquy());        
    }

    private void InitDialogueUI(bool isTalking)
    {
        player.IsTalking = isTalking; // 대화 시작 시 플레이어 대화 상태 설정
        npcName.text = "";
        line.text = "";
    }

    private IEnumerator PlaySoliloquy()
    {
        InitDialogueUI(true);
        yield return StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));

        Slide(true);
        yield return new WaitForSeconds(2f);

        foreach (DialogueLine line in soliloquy.dialogueLines)
        {
            if (line.audioClip != null)
            {
                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                audioSource.clip = line.audioClip;
                audioSource.Play();                

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

        InitDialogueUI(false);
    }

    public void StartCoroutinePlayDialogue()
    {
        StartCoroutine(PlayNurseDialogue());
    }

    private IEnumerator PlayNurseDialogue()
    {
        InitDialogueUI(true);

        StartCoroutine(nurse.TurnTowardPlayer(player.transform)); // NPC가 플레이어를 바라보도록 설정                
        
        player.LookAtPosition(nurse.transform.GetChild(0).transform); // 플레이어가 NPC를 바라보도록 설정
        player.ZoomIn(); // 플레이어 줌 인
        dialogue.SetActive(true);
        Slide(true);
        yield return new WaitForSeconds(2f);

        int animationIndex = 0;
        foreach (DialogueLine line in nurseDialogue.dialogueLines)
        {
            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();

                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (npcName.text == "간호사")
                {
                    nurse.PlayAnimation(animationIndex % 5 + 1);
                    animationIndex++;                    
                }
                else nurse.StopTalking();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

        yield return FadeUtility.Instance.FadeIn(screen, 2f); // 대화가 끝나면 화면 페이드 인        
        player.ZoomOut(); // 플레이어 줌 인
        yield return FadeUtility.Instance.FadeOut(screen, 2f); // 대화가 끝나면 화면 페이드 인        

        InitDialogueUI(false);
    }

    public void StartFoyerDialogue()
    {
        StartCoroutine(PlayFoyerDialogue());
    }

    private IEnumerator PlayFoyerDialogue()
    {
        InitDialogueUI(true);
        player.LookAtPosition(foyerLookTarget);

        Slide(true);
        yield return new WaitForSeconds(2f);

        foreach (DialogueLine line in foyerDialogue.dialogueLines)
        {
            if (line.audioClip != null)
            {
                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                audioSource.clip = line.audioClip;
                audioSource.Play();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

        InitDialogueUI(false);
    }

    public void StartHernyOfficeDialogue()
    {
        StartCoroutine(PlayHernyOfficeDialogue());
    }

    private IEnumerator PlayHernyOfficeDialogue()
    {
        InitDialogueUI(true);

        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 2f));
        player.transform.position = hernyOfficePos.transform.position;
        yield return StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));

        player.LookAtPosition(hernyOfficeLookTarget);

        Slide(true);
        yield return new WaitForSeconds(2f);

        foreach (DialogueLine line in hernyOfficeDialogue.dialogueLines)
        {
            if (line.audioClip != null)
            {
                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                audioSource.clip = line.audioClip;
                audioSource.Play();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

        InitDialogueUI(false);
    }

    public void StartAdministrativeOfficeDialogue()
    {
        StartCoroutine(PlayAdministrativeOfficeDialogue());
    }

    private IEnumerator PlayAdministrativeOfficeDialogue()
    {
        InitDialogueUI(true);
        player.LookAtPosition(administrativeOfficer.LookTarget);

        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 2f));        
        player.transform.position = administrativeOfficePos.transform.position;
        yield return StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));
        
        Slide(true);

        StartCoroutine(administrativeOfficer.TurnTowardPlayer(player.transform)); // NPC가 플레이어를 바라보도록 설정
        StartCoroutine(administrativeOfficer2.TurnTowardPlayer(player.transform));
        yield return new WaitForSeconds(2f);

        int animationIndex = 0;
        foreach (DialogueLine line in administrativeOfficeDialogue.dialogueLines)
        {            
            if (line.audioClip != null)
            {
                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (line.characterName == "행정실 직원")
                {
                    administrativeOfficer.PlayAnimation(animationIndex % 3);
                    animationIndex++;
                }                    

                audioSource.clip = line.audioClip;
                audioSource.Play();

                // 대사를 한 글자씩 출력
                yield return StartCoroutine(TypeLine(line.line));

                // 대사가 다 나온 후 오디오 종료까지 대기
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 2f));
        Slide(false);
        administrativeOfficer.gameObject.SetActive(false);
        administrativeOfficer2.gameObject.SetActive(false);
        yield return StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));

        InitDialogueUI(false);
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

    public IEnumerator FindEvidence(string playerLine, AudioClip playerAudioClip, string tagName)
    {
        if(tagName == "Computer")
        {
            yield return StartCoroutine(InvastigationCoroutine());
        }

        npcName.text = "수잔";
        npcName.color = new Color32(0xA0, 0x00, 0x00, 0xFF); // 0xA00000 + 완전 불투명
        line.text = "";

        Slide(true, 0.3f);
        yield return new WaitForSeconds(1f);

        Debug.LogWarning(playerLine);
        StartCoroutine(TypeLine(playerLine));
        audioSource.clip = playerAudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(playerAudioClip.length + 0.5f);

        Slide(false, 0.3f);

        player.IsTalking = false; // 독백 종료 시 증거 탐색 가능하게 설정
    }




    public void Slide(bool isTalking, float fadeDuration = 1f)
    {
        SlideMove(0, isTalking, fadeDuration);
        SlideMove(1, isTalking, fadeDuration);

        FadeDialogue(isTalking, fadeDuration);
    }

    private void FadeDialogue(bool isTalking, float duration = 1f)
    {
        if(isTalking)
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

    private void SlideMove(int index, bool isOn, float fadeDuration)
    {
        if (index < 0 || index >= slides.Length)
        {
            Debug.LogError("Slide index out of range.");
        }

        RectTransform tmp = slides[index].GetComponent<RectTransform>();
        float y = isOn ? 0f : (index == 0 ? tmp.rect.height : -tmp.rect.height);  // 방향에 따라 위치 계산
        Vector2 targetPosition = new Vector2(tmp.anchoredPosition.x, y);

        StartCoroutine(SmoothMove(tmp, tmp.anchoredPosition, targetPosition));        
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 startPos, Vector2 endPos)
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
    }



    public IEnumerator InvastigationCoroutine()
    {
        Image screen = investigationUI.transform.GetChild(0).GetComponent<Image>();
        Text investigationText = screen.transform.GetChild(0).GetComponent<Text>();
        investigationText.text = "";

        investigationUI.gameObject.SetActive(true);

        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 1f));

        yield return StartCoroutine(LoopDots(investigationText));
        // StartCoroutine(FadeUtility.Instance.FadeOut(investigationText, 1f));

        yield return StartCoroutine(FadeUtility.Instance.FadeOut(screen, 1f));

        investigationUI.gameObject.SetActive(false);
    }

    private IEnumerator LoopDots(Text investigationText)
    {
        string baseText = "조사 중";
        int dotCount = 0;
        float interval = 0.5f;

        investigationText.text = baseText;

        while (dotCount < 3)
        {
            dotCount = (dotCount + 1);
            investigationText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(interval);
        }
    }

}

