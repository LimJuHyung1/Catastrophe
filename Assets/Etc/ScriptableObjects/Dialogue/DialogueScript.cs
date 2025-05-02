using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Script")]
public class DialogueScript : ScriptableObject
{
    public DialogueLine[] dialogueLines;
}