using OpenAI;
using UnityEngine;
using System.Collections.Generic;
using AdvancedPeopleSystem;
using System.Collections;


public class NPCRole : NPC
{
    public enum Character
    {
        Henry,      // 남자A
        William,    // 남자B
        John,       // 남자C

        Sophia,     // 여자A
        Emma,       // 여자B
        Police,     // 경찰
        Nason
    }

    protected bool isReadyToTalk = true;
    public bool IsReadyToTalk
    {
        get { return isReadyToTalk; }
        set { isReadyToTalk = value; }
    }

    protected List<ChatMessage> chatMessages;
    protected OpenAIApi openAIApi;

    public ConversationManager conversationManager;
    public Character currentCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {        
        openAIApi = new OpenAIApi();
        SetRole();
    }

    // 각 NPC 역할 학습
    public void SetRole()
    {
        string npcName = currentCharacter.ToString();
        chatMessages = new List<ChatMessage>(); // 각 NPC마다 새 리스트 생성
        ChatMessage systemMessage = new ChatMessage
        {            
            Role = "system",
            Content = GetPrompt(npcName) // 각 NPC의 프롬프트를 가져옴        
        };
        chatMessages.Add(systemMessage);
    }


    // 답변 출력    
    public async void GetResponse()
    {
        if (conversationManager.GetAskFieldTextLength() < 1)
        {
            return;
        }

        ChatMessage newMessage = new ChatMessage
        {
            Content = conversationManager.GetAskFieldText(),      // 질문 입력
            Role = "user"
        };

        chatMessages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest
        {
            Messages = chatMessages,
            Model = "gpt-4o-mini"                 // chatGPT 3.5 버전 사용 gpt-3.5-turbo
        };

        var response = await openAIApi.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message; // 질문에 대한 응답 객체 받음

            chatMessages.Add(chatResponse); // 메세지 리스트에 추가

            answer = chatResponse.Content;  // 응답을 string으로 변환
            Debug.Log(answer);
            conversationManager.ShowAnswer(answer);  // 화면에 출력
        }
    }





    public IEnumerator TurnTowardPlayer(Transform target, float duration = 1f)
    {
        float elapsed = 0f;
        Quaternion startRotation = transform.rotation;

        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            yield return null;
        }

        transform.rotation = targetRotation; // 정확히 맞추기
    }
}
