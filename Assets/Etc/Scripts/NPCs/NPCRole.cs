using OpenAI;
using UnityEngine;
using System.Collections.Generic;
using AdvancedPeopleSystem;


public class NPCRole : NPC
{
    public enum Character
    {
        Henry,      // ����A
        William,    // ����B
        John,       // ����C

        Sophia,     // ����A
        Emma,       // ����B
        Police,     // ����
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

    // �� NPC ���� �н�
    public void SetRole()
    {
        string npcName = currentCharacter.ToString();
        chatMessages = new List<ChatMessage>(); // �� NPC���� �� ����Ʈ ����
        ChatMessage systemMessage = new ChatMessage
        {            
            Role = "system",
            Content = GetPrompt(npcName) // �� NPC�� ������Ʈ�� ������        
        };
        chatMessages.Add(systemMessage);
    }


    // �亯 ���    
    public async void GetResponse()
    {
        if (conversationManager.GetAskFieldTextLength() < 1)
        {
            return;
        }

        ChatMessage newMessage = new ChatMessage
        {
            Content = conversationManager.GetAskFieldText(),      // ���� �Է�
            Role = "user"
        };

        chatMessages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest
        {
            Messages = chatMessages,
            Model = "gpt-4o-mini"                 // chatGPT 3.5 ���� ��� gpt-3.5-turbo
        };

        var response = await openAIApi.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message; // ������ ���� ���� ��ü ����

            chatMessages.Add(chatResponse); // �޼��� ����Ʈ�� �߰�

            answer = chatResponse.Content;  // ������ string���� ��ȯ
            Debug.Log(answer);
            conversationManager.ShowAnswer(answer);  // ȭ�鿡 ���
        }
    }







    /// <summary>
    /// �÷��̾� �������� rotate
    /// </summary>
    public void TurnTowardPlayer(Transform playerTrans)
    {
        // ���� ������Ʈ�� ��ġ
        Vector3 targetPosition = playerTrans.position;

        // ���� ������Ʈ�� y ��ġ�� �������� ����
        targetPosition.y = transform.position.y;

        // Ÿ�� ��ġ�� �ٶ󺸵��� ȸ��
        transform.LookAt(targetPosition);
    }
}
