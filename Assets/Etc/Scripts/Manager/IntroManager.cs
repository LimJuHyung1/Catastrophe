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

    public HernyHomeDialogueScript reporterScript; // 새로 추가된 ScriptableObject 참조

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
    /// 인트로 시퀀스를 시작하는 코루틴
    /// </summary>
    private IEnumerator StartSequence()
    {        
        yield return StartCoroutine(ShowIntro()); // 인트로 메시지 출력
    }


    /// <summary>
    /// 인트로 텍스트를 순차적으로 출력하는 코루틴
    /// </summary>
    private IEnumerator ShowIntro()
    {
        float interval = 0.5f;

        yield return new WaitForSeconds(1f);

        DialogueLine[] lines = reporterScript.dialogueLines;

        // 메시지 출력
        for (int i = 0; i < lines.Length; i++)
        {   
            if (i == 3)
                cameraSup.ActivateDollyCam();
            // i == 5 일 때 신문지 표시하기
            else if (i == 7)
                cameraSup.ActivateTVCam();

            audioSource.clip = lines[i].audioClip;

            if (i == 0)
            {
                StartCoroutine(FadeUtility.Instance.FadeOut(screen, 3f));                
                audioSource.volume = 0f; // 볼륨 0에서 시작
                audioSource.Play();
                StartCoroutine(FadeUtility.Instance.FadeInAudio(audioSource, 2f)); // 2초 동안 볼륨 증가
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
        // 화면 페이드인 및 로딩 UI 활성화
        screen.transform.SetSiblingIndex(screen.transform.parent.childCount - 1);
        yield return StartCoroutine(FadeUtility.Instance.FadeIn(screen, 3f));

        loading.transform.SetSiblingIndex(loading.transform.parent.childCount - 1);
        loading.gameObject.SetActive(true);
        rainPrefab.gameObject.SetActive(false);

        // 다음 씬으로 이동
        yield return StartCoroutine(WaitNextScene());
        */
    }

    /// <summary>
    /// 한 글자씩 출력하는 코루틴
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