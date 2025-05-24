using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HernyHomeManager : MonoBehaviour
{
    public DialogueScript dialogueScript;
    public GameObject dialogue;
    public Transform startPosition;

    public Sophia sophia;
    public Player player;
    public GameObject chair;

    public Image[] slides;
    public Image screen;

    private float typeSpeed = 0.065f; // �� ���� ��� ���� (���� ����)
    private float duration = 1.0f;     // �̵��� �ɸ��� �ð�

    private AudioSource audioSource;
    private Text npcName;
    private Text line;    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        npcName = dialogue.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        line = dialogue.transform.GetChild(1).GetChild(1).GetComponent<Text>();

        player.transform.position = startPosition.position; // �÷��̾� ��ġ �ʱ�ȭ
        player.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    private void Start()
    {
        if (dialogueScript != null)
        {
            // ������ �� �� �Ʒ� �ڷ�ƾ �����ؾ� ��
            StartCoroutine(PlayDialogueAudioClips());
            // sophia.SitOnAChair(chair.transform); // NPC ��ȭ ����
        }
    }

    private IEnumerator PlayDialogueAudioClips()
    {
        player.IsTalking = true; // ��ȭ ���� �� �÷��̾� ��ȭ ���� ����
        dialogue.SetActive(true);
        StartCoroutine(sophia.TurnTowardPlayer(player.transform)); // NPC�� �÷��̾ �ٶ󺸵��� ����
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));        
        player.LookAtPosition(sophia.transform.GetChild(0).transform); // �÷��̾ NPC�� �ٶ󺸵��� ����

        yield return new WaitForSeconds(1f); // NPC�� �÷��̾ �ٶ󺸴� �ð�

        Slide(true);

        int sophiaAnimIndex = 0;

        foreach (DialogueLine line in dialogueScript.dialogueLines)
        {            

            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();

                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (npcName.text == "���Ǿ�")
                {
                    sophia.PlayDialogueAnimation(sophiaAnimIndex++);
                    sophia.StartTalking();                    
                }                    
                else sophia.StopTalking();

                // ��縦 �� ���ھ� ���
                yield return StartCoroutine(TypeLine(line.line));

                // ��簡 �� ���� �� ����� ������� ���
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        Slide(false);

        yield return FadeUtility.Instance.FadeIn(screen, 2f); // ��ȭ�� ������ ȭ�� ���̵� ��
        sophia.SitOnAChair(chair.transform); // NPC ��ȭ ����
        yield return FadeUtility.Instance.FadeOut(screen, 2f); // ��ȭ�� ������ ȭ�� ���̵� ��        
        
        player.IsTalking = false; // ��ȭ ���� �� �÷��̾� ��ȭ ���� ����        
    }

    private IEnumerator TypeLine(string sentence)
    {
        line.text = ""; // ���� �ؽ�Ʈ �ʱ�ȭ

        foreach (char c in sentence)
        {
            line.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public IEnumerator FindEvidence(string playerLine, AudioClip playerAudioClip)
    {
        npcName.text = "����";

        Slide(true);

        Debug.LogWarning(playerLine);
        StartCoroutine(TypeLine(playerLine));
        audioSource.clip = playerAudioClip;
        audioSource.Play();

        yield return new WaitForSeconds(playerAudioClip.length + 0.5f);

        Slide(false);

        player.IsTalking = false; // ���� ���� �� ���� Ž�� �����ϰ� ����
    }





    private void FadeDialogue(bool isTalking, float duration = 1f)
    {
        if (isTalking)
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
    public void Slide(bool isTalking, float fadeDuration = 1f)
    {
        SlideMove(0, isTalking, fadeDuration);
        SlideMove(1, isTalking, fadeDuration);
    }
    private void SlideMove(int index, bool isOn, float fadeDuration)
    {
        if (index < 0 || index >= slides.Length)
        {
            Debug.LogWarning("Slide index out of range.");
            return;
        }

        RectTransform tmp = slides[index].GetComponent<RectTransform>();
        float y = isOn ? 0f : (index == 0 ? tmp.rect.height : -tmp.rect.height);  // ���⿡ ���� ��ġ ���
        Vector2 targetPosition = new Vector2(tmp.anchoredPosition.x, y);

        StartCoroutine(SmoothMove(tmp, tmp.anchoredPosition, targetPosition, isOn, fadeDuration));
    }

    private IEnumerator SmoothMove(RectTransform rect, Vector2 startPos, Vector2 endPos, bool isOn, float fadeDuration)
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
        FadeDialogue(isOn, fadeDuration);
    }

}
