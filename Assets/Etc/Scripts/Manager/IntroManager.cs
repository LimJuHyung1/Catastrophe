using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class IntroManager : MonoBehaviour
{
    private int voiceIndex = 0;
    private AudioSource audioSource;

    public AudioClip[] reporterVoices;
    public Text Announcement;
    public Image screen;    


    private string[] cueSheet = {
        "속보입니다. 오늘 새벽, 유명 외과 의사 헨리 박사가 실종되었습니다.",
        "경찰은 헨리 박사의 차량이 이곳 외곽 도로의 가드레일을 들이받은 채 발견되었으며, " +
        "헨리 박사의 모습은 사고 현장 근처에서는 확인되지 않았다고 보고하였습니다.",
        "경찰은 차량 내부에서 혈흔이 발견되었으며, " +
        "강한 충격으로 인해 운전자가 사고 직후 현장을 벗어났을 가능성을 염두에 두고 있습니다.",

        // (화면이 전환되며 헨리 박사의 프로필 사진과 그의 신상 정보가 화면에 뜬다.)
        "헨리 박사는 최근 대형 투자사 오리온 캐피탈과의 계약 문제로 논란이 된 바 있습니다.",
        "오리온 캐피탈 측은 헨리 박사의 병원이 투자금을 적절히 활용하지 못했다며 계약 재검토를 발표했고, " +
        "양 측의 갈등이 심화된 것으로 알려졌습니다.",

        // (화면이 전환되며, 헨리와 윌리엄의 관계를 다룬 최근 뉴스 기사가 화면에 표시된다.)
        "얼마 전, 헨리 박사와 오리온 캐피탈의 윌리엄 부사장 간의 투자 계약 차질이 언론을 통해 보도되었으며, " +
        "두 사람이 이에 대해 비공식적인 논의를 이어왔던 것으로 전해졌습니다.",
        "경찰은 이 투자 계약 문제와 이번 사건의 연관성을 배제하지 않고 조사 중입니다.",

        // (카메라는 다시 현장으로 돌아와 경찰이 주변을 수색하는 모습을 비춘다.)
        "현재 경찰은 헨리 박사의 실종과 사고가 단순한 우연인지, 혹은 사건의 배후가 있는지 철저히 조사하고 있습니다.",
        "새로운 단서가 확보되는 대로 신속히 전해드리겠습니다."
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartSequence());      
    }

    /// <summary>
    /// 인트로 시퀀스를 시작하는 코루틴
    /// </summary>
    private IEnumerator StartSequence()
    {
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 3f));
        yield return StartCoroutine(ShowIntro()); // 인트로 메시지 출력
    }


    /// <summary>
    /// 인트로 텍스트를 순차적으로 출력하는 코루틴
    /// </summary>
    private IEnumerator ShowIntro()
    {
        string initialText = "Reporter : ";

        yield return new WaitForSeconds(3.5f);

        // 메시지 출력
        for (int i = 0; i < cueSheet.Length; i++)
        {            
            audioSource.clip = reporterVoices[voiceIndex];
            audioSource.Play();                    

            StartCoroutine(ShowIntroDispatch(Announcement, cueSheet[i], initialText));
            yield return new WaitForSeconds(reporterVoices[voiceIndex].length + 1f);

            voiceIndex++;
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
            yield return new WaitForSeconds(0.75f);
        }
    }
}
