using AdvancedPeopleSystem;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected Animator animator;
    protected CharacterCustomization cc;
    protected string answer;
    protected NPCRoleInfoManager roleInfoManager;

    protected void Awake()
    {
        animator = GetComponent<Animator>();

        cc = GetComponent<CharacterCustomization>();
        cc.InitColors();

        // NPCEmotionHandler ÃÊ±âÈ­
        roleInfoManager = new NPCRoleInfoManager();
    }

    public string GetRole(string npcName) => roleInfoManager.GetRole(npcName);
    public string GetInstructions(string npcName) => roleInfoManager.GetInstructions(npcName);
    public string GetBackground(string npcName) => roleInfoManager.GetBackground(npcName);
    public string GetFriends(string npcName) => roleInfoManager.GetFriends(npcName);
    public string GetAlibi(string npcName) => roleInfoManager.GetAlibi(npcName);
    public string GetResponseGuidelines(string npcName) => roleInfoManager.GetResponseGuidelines(npcName);
}
