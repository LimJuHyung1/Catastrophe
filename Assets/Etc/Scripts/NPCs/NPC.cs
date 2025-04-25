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

    public string GetPrompt(string npcName) => roleInfoManager.GetPrompt(npcName);
}
