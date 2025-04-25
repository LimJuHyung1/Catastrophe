using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConversationManager : BaseManager
{
    public Image screen;
    public GameObject conversationUI;    
    public GameObject dialogue;  // 대화 UI

    [SerializeField] GameObject waitingMark;  // NPC가 발언 준비 중 표시
    [SerializeField] GameObject clickMark;  // 플레이어가 입력 가능할 때 표시

    private Button endConversationBtn;  // 대화 종료 버튼
    private InputField inputField;   
    private Text NPCLine;

    public string tmpAnswer = "";
    public string tmpQuestion = "";
    public bool isReadyToSkip = false;  // NPC가 문장 출력 시작 시 true로 변경됨
    public bool IsReadyToSkip
    {
        get { return isReadyToSkip; }
        set { isReadyToSkip = value; }
    }


    private bool waitToSkip = true;
    private bool isTalking = false;
    private bool isAbleToGoNext = false;
    private bool isSkipping = false;



    public Player player;
    [SerializeField] private NPCRole npcRole;
    private Coroutine displayCoroutine;

    private Queue<string> sentencesQueue = new Queue<string>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NPCLine = dialogue.transform.GetChild(1).GetComponent<Text>();
        endConversationBtn = conversationUI.transform.GetChild(0).GetComponent<Button>();
        inputField = conversationUI.transform.GetChild(1).GetComponent<InputField>();

        conversationUI.SetActive(false);
    }

    /// <summary>
    /// 키 입력 감지 후 대화 스킵 기능 활성화
    /// </summary>
    void OnEnable()
    {
        StartCoroutine(WaitForSkipInput());
    }

    IEnumerator WaitForSkipInput()
    {
        while (true)
        {
            if (isReadyToSkip && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
            {
                isSkipping = true;
            }
            yield return null;
        }
    }

    /// <summary>
    /// Player 스크립트에서 현재 대화하는 NPC를 설정하는 메서드
    /// </summary>    
    public void GetNPCRole(NPCRole npcParam)
    {
        npcRole = npcParam;
    }

    /// <summary>
    /// 참조중인 NPC 제거 (대화가 종료될 때 호출됨)
    /// </summary>
    public void RemoveNPCRole()
    {
        npcRole = null;
    }


    /// <summary>
    /// InputField에서 실행되는 이벤트 리스너 등록
    /// </summary>
    public void AddListenersResponse()
    {
        if (npcRole != null)
        {
            OnEndEditAskField(npcRole.GetResponse);
            OnEndEditAskField(SetNullInputField);
        }
    }

    /// <summary>
    /// NPC 대화 전 엔터 키 가능하게 세팅
    /// </summary>
    /// <param name="action"></param>
    public void OnEndEditAskField(UnityAction action)
    {
        inputField.onEndEdit.AddListener((string text) =>
        {
            if (!string.IsNullOrWhiteSpace(text) && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                tmpQuestion = text.Trim();
                action();
            }
        });
    }

    /// <summary>
    /// input field의 질문 칸을 공백으로 설정
    /// </summary>
    public void SetNullInputField()
    {
        inputField.text = "";
    }





    /// <summary>
    /// 대화를 시작하는 메서드 (마우스 클릭 시 실행)
    /// </summary>
    public void StartConversation()
    {
        isTalking = true;
        StartCoroutine(StartConversationCoroutine());
        AddListenersResponse();
        CursorManager.Instance.OnVisualization();
    }

    private IEnumerator StartConversationCoroutine()
    {
        yield return FadeUtility.Instance.SwitchCameraWithFade(screen, player, npcRole);
        dialogue.SetActive(true);
        conversationUI.SetActive(true);        
        // SetAudio();
    }

    /// <summary>
    /// 대화를 종료하는 메서드 (NPC와 대화 종료 시 실행)
    /// </summary>
    public void EndConversation()
    {        
        dialogue.SetActive(false);
        conversationUI.SetActive(false);
        StartCoroutine(EndConversationCoroutine());
    }

    private IEnumerator EndConversationCoroutine()
    {
        SetNullInputField();
        RemoveOnEndEditListener();
        SetBlankAnswerText();

        yield return FadeUtility.Instance.SwitchCameraWithFade(
            screen, player, npcRole);
        
        CursorManager.Instance.OnVisualization();
        isTalking = false;
        RemoveNPCRole();
    }

    /// <summary>
    /// 대화 UI를 활성화/비활성화
    /// </summary>
    public void SetConversationUI(bool b)
    {
        conversationUI.SetActive(b);
    }


    public void RemoveOnEndEditListener()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }

    /// <summary>
    /// NPC 답변 텍스트를 공백으로 설정
    /// </summary>
    public void SetBlankAnswerText()
    {
        NPCLine.text = "";
    }

    /// <summary>
    /// NPC의 답변을 받아 화면에 출력 (문장 단위로 나누어 대사 큐에 저장)
    /// </summary>
    public void ShowAnswer(string answer)
    {
        if (npcRole == null)
        {
            Debug.LogError("NPC가 없습니다!");
            return;
        }

        // gameManager.logManager.AddLog(npcRole, gameManager.uIManager.GetQuestion(), answer);
        SetInteractableAskField(false);
        SetActiveEndConversationButton(false);
        // SetAudio();

        sentencesQueue.Clear();

        // 정규식을 이용해 '.', '?', '!'로 분할하면서 "..." 예외 처리
        // 리팩토링 당시 System.Text.RegularExpressions.Regex.Split - 이 과정은 요구되는 연산량이 많으나
        // '.', '!', '?' 등의 문자를 표시해야 하기 때문에 리팩토링을 진행하지 않음
        string[] sentences = System.Text.RegularExpressions.Regex.Split(answer, @"(?<=[^\.]\.|[!?])");

        foreach (string part in sentences)
        {
            if (!string.IsNullOrWhiteSpace(part)) // 공백이 아닌 유효한 문자열만 처리
            {
                string trimmedSentence = part.Trim();
                sentencesQueue.Enqueue(trimmedSentence);
            }
        }

        if (displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplaySentences());
        }
    }

    /// <summary>
    /// 대사를 한 글자씩 출력하는 코루틴
    /// </summary>
    public IEnumerator ShowLine(Text t, string answer, float second = 0.1f, bool isTyping = true)
    {
        t.text = ""; // 텍스트 초기화
        Coroutine dialogSoundCoroutine = null;

        SetWaitingUI(true);
        dialogSoundCoroutine = StartCoroutine(PlayDialogSound());

        yield return new WaitUntil(() => waitToSkip);
        for (int i = 0; i < answer.Length; i++)
        {
            if (isSkipping) // 마우스 왼쪽 클릭 or 엔터키 감지
            {
                isSkipping = false;
                waitToSkip = false;
                t.text = answer; // 전체 텍스트 즉시 표시
                break; // 루프를 중단하고 전체 텍스트를 표시하도록 이동
            }

            t.text += answer[i]; // 한 글자씩 추가
            yield return new WaitForSeconds(second); // 0.02초 대기
        }

        tmpAnswer = t.text;
        isReadyToSkip = false;
        ChangeIsSkipping(false);

        // 코루틴이 실행되었을 경우에만 종료 처리
        if (dialogSoundCoroutine != null)
        {
            SetWaitingUI(false);
            StopCoroutine(dialogSoundCoroutine); // PlayDialogSound 코루틴 중지
            // SoundManager.Instance.StopTextSound();
        }

        IsAbleToGoNextTrue();
    }

    /// <summary>
    /// NPC의 답변을 한문장 씩 화면에 출력
    /// </summary>
    private IEnumerator DisplaySentences()
    {
        if (!isTalking) yield break;

        while (sentencesQueue.Count > 0)
        {
            string sentence = sentencesQueue.Dequeue();
            isAbleToGoNext = false;
            IsReadyToSkip = true;

            ChatMessage message = new ChatMessage { Content = sentence };
            // npcRole.PlayEmotion(message);
            Debug.Log(GetNPCAnswer());
            yield return StartCoroutine(ShowLine(GetNPCAnswer(), sentence));

            yield return new WaitUntil(() => isAbleToGoNext);

            if (sentencesQueue.Count > 0)
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0)
                || Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.Return));

                ChangeIsSkipping(true);
            }
        }

        IsReadyToSkip = false;

        // 대화가 끝났으므로 "대화 종료" 버튼을 활성화
        SetActiveEndConversationButton(true);

        // 플레이어가 질문 입력 필드를 다시 사용할 수 있도록 설정
        SetInteractableAskField(true);

        // 대사 스킵 가능 여부를 다시 설정
        ChangeIsSkipping(true);

        // 플레이어가 질문을 입력할 수 있도록 입력 필드에 포커스
        FocusOnAskField();

        // 현재 실행 중인 코루틴을 초기화하여 다음 실행을 준비
        displayCoroutine = null;
    }    

    public void SetActiveEndConversationButton(bool b)
    {
        endConversationBtn.gameObject.SetActive(b);
    }

    public void SetInteractableAskField(bool b)
    {
        inputField.interactable = b;
    }

    public void ChangeIsSkipping(bool b)
    {
        waitToSkip = b;
    }

    void FocusOnAskField()
    {
        inputField.Select();
        inputField.ActivateInputField(); // InputField 활성화
    }

    // NPC 답변 반환
    public Text GetNPCAnswer()
    {
        return NPCLine;
    }

    /// <summary>
    /// NPC의 발언 상태에 따라 UI 설정
    /// </summary>
    /// <param name="isSpeaking">NPC가 발언 중인지 여부</param>
    public void SetWaitingUI(bool isSpeaking)
    {
        waitingMark.gameObject.SetActive(isSpeaking);
        clickMark.gameObject.SetActive(!isSpeaking);
    }

    /// <summary>
    /// 다음 문장으로 넘어갈 수 있도록 상태 변경
    /// </summary>
    public void IsAbleToGoNextTrue()
    {
        isAbleToGoNext = true;
    }

    IEnumerator PlayDialogSound()
    {
        while (true) // 무한 루프를 사용하여 반복 실행
        {
            // SoundManager.Instance.PlayTextSound();
            yield return new WaitForSeconds(0.1f);
        }
    }



    // InputField 텍스트 길이 반환
    public int GetAskFieldTextLength()
    {
        return inputField.text.Length;
    }

    // InputField 텍스트 반환
    public string GetAskFieldText()
    {
        return inputField.text;
    }

    /// <summary>
    /// 대화 중인지 여부 반환
    /// </summary>
    public bool GetIsTalking()
    {
        return isTalking;
    }
}
