using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HernyHomeManager : MonoBehaviour
{
    public float typeSpeed = 0.065f; // �� ���� ��� ���� (���� ����)

    public HernyHomeDialogueScript dialogueScript;
    public GameObject dialogue;    

    public Sophia sophia;
    public Player player;
    public GameObject chair;

    public Image screen;    
    
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

        dialogue.SetActive(false);
        npcName = dialogue.transform.GetChild(0).GetComponent<Text>();
        line = dialogue.transform.GetChild(1).GetComponent<Text>();
    }

    private void Start()
    {
        if (dialogueScript != null)
        {
            // ������ �� �� �Ʒ� �ڷ�ƾ �����ؾ� ��
            // StartCoroutine(PlayDialogueAudioClips());
            sophia.SitOnAChair(chair.transform); // NPC ��ȭ ����
        }
    }

    private IEnumerator PlayDialogueAudioClips()
    {
        player.IsTalking = true; // ��ȭ ���� �� �÷��̾� ��ȭ ���� ����
        sophia.TurnTowardPlayer(player.transform); // NPC�� �÷��̾ �ٶ󺸵��� ����
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 2f));        
        player.LookAtPosition(sophia.transform.GetChild(0).transform); // �÷��̾ NPC�� �ٶ󺸵��� ����

        yield return new WaitForSeconds(1f); // NPC�� �÷��̾ �ٶ󺸴� �ð�

        FadeUtility.Instance.FadeIn(npcName, 1f);
        FadeUtility.Instance.FadeIn(line, 1f);

        foreach (DialogueLine line in dialogueScript.dialogueLines)
        {
            if (line.audioClip != null)
            {
                audioSource.clip = line.audioClip;
                audioSource.Play();

                npcName.color = line.nameColor;
                npcName.text = line.characterName;

                if (npcName.text == "���Ǿ�")
                    sophia.StartTalking();
                else sophia.StopTalking();

                // ��縦 �� ���ھ� ���
                yield return StartCoroutine(TypeLine(line.line));

                // ��簡 �� ���� �� ����� ������� ���
                float remainingTime = line.audioClip.length - line.line.Length * typeSpeed;
                yield return new WaitForSeconds(Mathf.Max(0f, remainingTime + 0.5f));
            }
        }

        FadeUtility.Instance.FadeOut(npcName, 2f);
        FadeUtility.Instance.FadeOut(line, 2f);        
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
        dialogue.SetActive(true); // ���� ����

        npcName.text = "����";
        Debug.LogWarning(playerLine);
        StartCoroutine(TypeLine(playerLine));
        audioSource.clip = playerAudioClip;
        audioSource.Play();
        
        yield return new WaitForSeconds(playerAudioClip.length + 0.5f);

        dialogue.SetActive(false); // ���� ����
        player.IsTalking = false; // ���� ���� �� ���� Ž�� �����ϰ� ����
    }
}
