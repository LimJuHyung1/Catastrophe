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
        "�Ӻ��Դϴ�. ���� ����, ���� �ܰ� �ǻ� � �ڻ簡 �����Ǿ����ϴ�.",
        "������ � �ڻ��� ������ �̰� �ܰ� ������ ���巹���� ���̹��� ä �߰ߵǾ�����, " +
        "� �ڻ��� ����� ��� ���� ��ó������ Ȯ�ε��� �ʾҴٰ� �����Ͽ����ϴ�.",
        "������ ���� ���ο��� ������ �߰ߵǾ�����, " +
        "���� ������� ���� �����ڰ� ��� ���� ������ ����� ���ɼ��� ���ο� �ΰ� �ֽ��ϴ�.",

        // (ȭ���� ��ȯ�Ǹ� � �ڻ��� ������ ������ ���� �Ż� ������ ȭ�鿡 ���.)
        "� �ڻ�� �ֱ� ���� ���ڻ� ������ ĳ��Ż���� ��� ������ ����� �� �� �ֽ��ϴ�.",
        "������ ĳ��Ż ���� � �ڻ��� ������ ���ڱ��� ������ Ȱ������ ���ߴٸ� ��� ����並 ��ǥ�߰�, " +
        "�� ���� ������ ��ȭ�� ������ �˷������ϴ�.",

        // (ȭ���� ��ȯ�Ǹ�, ��� �������� ���踦 �ٷ� �ֱ� ���� ��簡 ȭ�鿡 ǥ�õȴ�.)
        "�� ��, � �ڻ�� ������ ĳ��Ż�� ������ �λ��� ���� ���� ��� ������ ����� ���� �����Ǿ�����, " +
        "�� ����� �̿� ���� ��������� ���Ǹ� �̾�Դ� ������ ���������ϴ�.",
        "������ �� ���� ��� ������ �̹� ����� �������� �������� �ʰ� ���� ���Դϴ�.",

        // (ī�޶�� �ٽ� �������� ���ƿ� ������ �ֺ��� �����ϴ� ����� �����.)
        "���� ������ � �ڻ��� ������ ��� �ܼ��� �쿬����, Ȥ�� ����� ���İ� �ִ��� ö���� �����ϰ� �ֽ��ϴ�.",
        "���ο� �ܼ��� Ȯ���Ǵ� ��� �ż��� ���ص帮�ڽ��ϴ�."
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartSequence());      
    }

    /// <summary>
    /// ��Ʈ�� �������� �����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator StartSequence()
    {
        StartCoroutine(FadeUtility.Instance.FadeOut(screen, 3f));
        yield return StartCoroutine(ShowIntro()); // ��Ʈ�� �޽��� ���
    }


    /// <summary>
    /// ��Ʈ�� �ؽ�Ʈ�� ���������� ����ϴ� �ڷ�ƾ
    /// </summary>
    private IEnumerator ShowIntro()
    {
        string initialText = "Reporter : ";

        yield return new WaitForSeconds(3.5f);

        // �޽��� ���
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
            yield return new WaitForSeconds(0.75f);
        }
    }
}
