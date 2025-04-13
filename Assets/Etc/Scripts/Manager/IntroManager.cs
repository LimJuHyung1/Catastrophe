using System.Collections;
using System.Text;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    private AudioSource audioSource;
    private CameraSup cameraSup;
    
    public Text Announcement;
    public Image screen;

    public CinemachineCamera TVCam;
    public CinemachineCamera dollyCam;

    public HernyHomeDialogueScript reporterScript; // ���� �߰��� ScriptableObject ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cameraSup = new CameraSup(TVCam, dollyCam);
        cameraSup.ActivateTVCam();


        screen.gameObject.SetActive(true);
        StartCoroutine(StartSequence());      
    }

    /// <summary>
    /// ��Ʈ�� �������� �����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator StartSequence()
    {        
        yield return StartCoroutine(ShowIntro()); // ��Ʈ�� �޽��� ���
    }


    /// <summary>
    /// ��Ʈ�� �ؽ�Ʈ�� ���������� ����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator ShowIntro()
    {
        float interval = 0.5f;

        yield return new WaitForSeconds(1f);

        DialogueLine[] lines = reporterScript.dialogueLines;

        // �޽��� ���
        for (int i = 0; i < lines.Length; i++)
        {   
            if (i == 3)
                cameraSup.ActivateDollyCam();
            // i == 5 �� �� �Ź��� ǥ���ϱ�
            else if (i == 7)
                cameraSup.ActivateTVCam();

            audioSource.clip = lines[i].audioClip;

            if (i == 0)
            {
                StartCoroutine(FadeUtility.Instance.FadeOut(screen, 3f));                
                audioSource.volume = 0f; // ���� 0���� ����
                audioSource.Play();
                StartCoroutine(FadeUtility.Instance.FadeInAudio(audioSource, 2f)); // 2�� ���� ���� ����
            }
            else
            {                
                audioSource.Play();
            }


            StartCoroutine(ShowIntroDispatch(Announcement, lines[i].line));
            yield return new WaitForSeconds(audioSource.clip.length + interval);
        }       

        yield return new WaitForSeconds(3.5f);

        /*
        // ȭ�� ���̵��� �� �ε� UI Ȱ��ȭ
        screen.transform.SetSiblingIndex(screen.transform.parent.childCount - 1);
        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 3f));

        loading.transform.SetSiblingIndex(loading.transform.parent.childCount - 1);
        loading.gameObject.SetActive(true);
        rainPrefab.gameObject.SetActive(false);

        // ���� ������ �̵�
        yield return StartCoroutine(WaitNextScene());
        */
    }

    /// <summary>
    /// �� ���ھ� ����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator ShowIntroDispatch(Text t, string dispatch, string initialText = "")
    {
        StringBuilder sb = new StringBuilder(initialText);
        t.text = sb.ToString();

        foreach (char letter in dispatch)
        {
            sb.Append(letter);
            t.text = sb.ToString();
            yield return new WaitForSeconds(0.075f);
        }
    }
}


public class CameraSup
{
    private CinemachineCamera TVCam;
    private CinemachineCamera dollyCam;

    public CameraSup(CinemachineCamera TVCam, CinemachineCamera dollyCam)
    {
        this.TVCam = TVCam;
        this.dollyCam = dollyCam;
    }

    public void ActivateTVCam()
    {       
        TVCam.Priority = 1;
        dollyCam.Priority = 0;
    }

    public void ActivateDollyCam()
    {
        TVCam.Priority = 0;
        dollyCam.Priority = 1;
    }
}