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
    public GameObject dialogue;  // ��ȭ UI

    [SerializeField] GameObject waitingMark;  // NPC�� �߾� �غ� �� ǥ��
    [SerializeField] GameObject clickMark;  // �÷��̾ �Է� ������ �� ǥ��

    private Button endConversationBtn;  // ��ȭ ���� ��ư
    private InputField inputField;   
    private Text NPCLine;

    public string tmpAnswer = "";
    public string tmpQuestion = "";
    public bool isReadyToSkip = false;  // NPC�� ���� ��� ���� �� true�� �����
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
    /// Ű �Է� ���� �� ��ȭ ��ŵ ��� Ȱ��ȭ
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
    /// Player ��ũ��Ʈ���� ���� ��ȭ�ϴ� NPC�� �����ϴ� �޼���
    /// </summary>    
    public void GetNPCRole(NPCRole npcParam)
    {
        npcRole = npcParam;
    }

    /// <summary>
    /// �������� NPC ���� (��ȭ�� ����� �� ȣ���)
    /// </summary>
    public void RemoveNPCRole()
    {
        npcRole = null;
    }


    /// <summary>
    /// InputField���� ����Ǵ� �̺�Ʈ ������ ���
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
    /// NPC ��ȭ �� ���� Ű �����ϰ� ����
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
    /// input field�� ���� ĭ�� �������� ����
    /// </summary>
    public void SetNullInputField()
    {
        inputField.text = "";
    }





    /// <summary>
    /// ��ȭ�� �����ϴ� �޼��� (���콺 Ŭ�� �� ����)
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
    /// ��ȭ�� �����ϴ� �޼��� (NPC�� ��ȭ ���� �� ����)
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
    /// ��ȭ UI�� Ȱ��ȭ/��Ȱ��ȭ
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
    /// NPC �亯 �ؽ�Ʈ�� �������� ����
    /// </summary>
    public void SetBlankAnswerText()
    {
        NPCLine.text = "";
    }

    /// <summary>
    /// NPC�� �亯�� �޾� ȭ�鿡 ��� (���� ������ ������ ��� ť�� ����)
    /// </summary>
    public void ShowAnswer(string answer)
    {
        if (npcRole == null)
        {
            Debug.LogError("NPC�� �����ϴ�!");
            return;
        }

        // gameManager.logManager.AddLog(npcRole, gameManager.uIManager.GetQuestion(), answer);
        SetInteractableAskField(false);
        SetActiveEndConversationButton(false);
        // SetAudio();

        sentencesQueue.Clear();

        // ���Խ��� �̿��� '.', '?', '!'�� �����ϸ鼭 "..." ���� ó��
        // �����丵 ��� System.Text.RegularExpressions.Regex.Split - �� ������ �䱸�Ǵ� ���귮�� ������
        // '.', '!', '?' ���� ���ڸ� ǥ���ؾ� �ϱ� ������ �����丵�� �������� ����
        string[] sentences = System.Text.RegularExpressions.Regex.Split(answer, @"(?<=[^\.]\.|[!?])");

        foreach (string part in sentences)
        {
            if (!string.IsNullOrWhiteSpace(part)) // ������ �ƴ� ��ȿ�� ���ڿ��� ó��
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
    /// ��縦 �� ���ھ� ����ϴ� �ڷ�ƾ
    /// </summary>
    public IEnumerator ShowLine(Text t, string answer, float second = 0.1f, bool isTyping = true)
    {
        t.text = ""; // �ؽ�Ʈ �ʱ�ȭ
        Coroutine dialogSoundCoroutine = null;

        SetWaitingUI(true);
        dialogSoundCoroutine = StartCoroutine(PlayDialogSound());

        yield return new WaitUntil(() => waitToSkip);
        for (int i = 0; i < answer.Length; i++)
        {
            if (isSkipping) // ���콺 ���� Ŭ�� or ����Ű ����
            {
                isSkipping = false;
                waitToSkip = false;
                t.text = answer; // ��ü �ؽ�Ʈ ��� ǥ��
                break; // ������ �ߴ��ϰ� ��ü �ؽ�Ʈ�� ǥ���ϵ��� �̵�
            }

            t.text += answer[i]; // �� ���ھ� �߰�
            yield return new WaitForSeconds(second); // 0.02�� ���
        }

        tmpAnswer = t.text;
        isReadyToSkip = false;
        ChangeIsSkipping(false);

        // �ڷ�ƾ�� ����Ǿ��� ��쿡�� ���� ó��
        if (dialogSoundCoroutine != null)
        {
            SetWaitingUI(false);
            StopCoroutine(dialogSoundCoroutine); // PlayDialogSound �ڷ�ƾ ����
            // SoundManager.Instance.StopTextSound();
        }

        IsAbleToGoNextTrue();
    }

    /// <summary>
    /// NPC�� �亯�� �ѹ��� �� ȭ�鿡 ���
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

        // ��ȭ�� �������Ƿ� "��ȭ ����" ��ư�� Ȱ��ȭ
        SetActiveEndConversationButton(true);

        // �÷��̾ ���� �Է� �ʵ带 �ٽ� ����� �� �ֵ��� ����
        SetInteractableAskField(true);

        // ��� ��ŵ ���� ���θ� �ٽ� ����
        ChangeIsSkipping(true);

        // �÷��̾ ������ �Է��� �� �ֵ��� �Է� �ʵ忡 ��Ŀ��
        FocusOnAskField();

        // ���� ���� ���� �ڷ�ƾ�� �ʱ�ȭ�Ͽ� ���� ������ �غ�
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
        inputField.ActivateInputField(); // InputField Ȱ��ȭ
    }

    // NPC �亯 ��ȯ
    public Text GetNPCAnswer()
    {
        return NPCLine;
    }

    /// <summary>
    /// NPC�� �߾� ���¿� ���� UI ����
    /// </summary>
    /// <param name="isSpeaking">NPC�� �߾� ������ ����</param>
    public void SetWaitingUI(bool isSpeaking)
    {
        waitingMark.gameObject.SetActive(isSpeaking);
        clickMark.gameObject.SetActive(!isSpeaking);
    }

    /// <summary>
    /// ���� �������� �Ѿ �� �ֵ��� ���� ����
    /// </summary>
    public void IsAbleToGoNextTrue()
    {
        isAbleToGoNext = true;
    }

    IEnumerator PlayDialogSound()
    {
        while (true) // ���� ������ ����Ͽ� �ݺ� ����
        {
            // SoundManager.Instance.PlayTextSound();
            yield return new WaitForSeconds(0.1f);
        }
    }



    // InputField �ؽ�Ʈ ���� ��ȯ
    public int GetAskFieldTextLength()
    {
        return inputField.text.Length;
    }

    // InputField �ؽ�Ʈ ��ȯ
    public string GetAskFieldText()
    {
        return inputField.text;
    }

    /// <summary>
    /// ��ȭ ������ ���� ��ȯ
    /// </summary>
    public bool GetIsTalking()
    {
        return isTalking;
    }
}
