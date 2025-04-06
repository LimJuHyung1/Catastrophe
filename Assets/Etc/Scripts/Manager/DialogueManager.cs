using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    public Text dialogueText;
    public GameObject dialoguePanel;

    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public bool autoPlay = false;
    public float autoPlayDelay = 2f;

    private Queue<string> dialogueQueue = new Queue<string>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Start()
    {
        dialoguePanel.SetActive(false); // Ã³À½¿¡´Â ¼û±è
    }

    public void StartDialogue(string[] lines)
    {
        dialogueQueue.Clear();
        foreach (var line in lines)
        {
            dialogueQueue.Enqueue(line);
        }

        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    public void ShowNextLine()
    {
        if (isTyping) return;

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = dialogueQueue.Dequeue();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (autoPlay)
        {
            yield return new WaitForSeconds(autoPlayDelay);
            ShowNextLine();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!autoPlay && dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextLine();
        }
    }
}
