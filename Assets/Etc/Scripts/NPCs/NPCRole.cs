using OpenAI;
using UnityEngine;
using System.Collections.Generic;
using AdvancedPeopleSystem;


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
            Content = GetRole(npcName)
            + GetInstructions(npcName)
            + GetBackground(npcName)
            + GetFriends(npcName)
            + GetAlibi(npcName)
            + GetResponseGuidelines(npcName)            
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
            Model = "gpt-3.5-turbo"                 // chatGPT 3.5 버전 사용
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







    /// <summary>
    /// 플레이어 방향으로 rotate
    /// </summary>
    public void TurnTowardPlayer(Transform playerTrans)
    {
        // 현재 오브젝트의 위치
        Vector3 targetPosition = playerTrans.position;

        // 현재 오브젝트의 y 위치는 변경하지 않음
        targetPosition.y = transform.position.y;

        // 타겟 위치를 바라보도록 회전
        transform.LookAt(targetPosition);
    }
}
