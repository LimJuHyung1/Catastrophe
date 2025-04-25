using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using UnityEditor.Experimental.GraphView;

[System.Serializable]
public class NPCRoleInfo
{
    public string name;                 // 이름 및 역할    
    public string prompt;               // 지시문
}

[System.Serializable]
public class EvidenceInfo
{
    public string name;             // 증거 이름
    public string description;      // 증거를 클릭했을 경우 화면에 보이는 문구
    public string information;
    public string foundAt;
    public string notes;            // 추가적으로 필요한 정보나 단서

    public string nasonExtraInformation;
    public string jennyExtraInformation;
    public string minaExtraInformation;

    public string renderTexturePath;
}

[System.Serializable]
public class NPCRoleInfoList
{
    public List<NPCRoleInfo> npcRoleInfoList;
}

// JSON 데이터 리스트를 담는 클래스
[System.Serializable]
public class EvidenceInfoList
{
    public List<EvidenceInfo> evidenceInfoList;
}


public class JsonManager : MonoBehaviour
{
    public string jsonPath;    // 초기화 과정이 awake, start에서만 가능하기에 static으로 설정 안 함
    
    public static NPCRoleInfoList npcRoleInfoList;
    public static EvidenceInfoList evidenceInfoList;

    private string npcRolePath;
    private string evidencePath;

    void Awake()
    {
        jsonPath = Path.Combine(Application.persistentDataPath, "Json");
                
        SaveNPCRoleJson();
        SaveEvidenceJson();

        npcRoleInfoList = LoadNPCRoleJson();
        evidenceInfoList = LoadEvidenceJson();
    }
    
    private void SaveNPCRoleJson()
    {
        List<NPCRoleInfo> npcRoleInfo = new List<NPCRoleInfo>
        {
            /*
             당신은 Sophia라는 인물입니다.

- 나이: 38세  
- 직업: 전업주부  
- 성격: 이성적이고 꼼꼼하며 감정을 잘 드러내지 않지만, 내면에는 깊은 고통이 있습니다.

            특징:
- 남편의 외도를 의심하고 있지만, 딸과 가정을 지키기 위해 감정을 억누르고 있습니다.
- 남편의 진짜 모습을 딸에게 보이고 싶지 않기에 가능한 한 침착함을 유지하려 합니다.
- 그러나 진실을 마주하게 되면 감정이 폭발할 수도 있습니다.
- 남편의 행동을 추궁하면서 예상하지 못한 진실에 다가가게 될 가능성도 있습니다.

---

현재 시각은 새벽 3시경이며, 당신은 남편 헨리의 집 거실에서 조사를 받고 있습니다.  
헨리는 새벽 4시 17분경 외곽 도로에서 차량만 남긴 채 실종되었으며, 경찰은 사고인지 사건인지 단정을 짓지 못한 상태입니다.  
검사인 플레이어가 당신에게 직접 대화를 시도하며, 다음과 같은 증거를 바탕으로 사건의 실마리를 추적하고 있습니다.

각 증거는 당신에게 보여질 수 있으며, 당신은 해당 증거가 의미하는 바를 인지하고 있어야 합니다.  
플레이어가 증거를 언급하면, 당신은 해당 내용을 바탕으로 짧고 조심스럽게 반응해야 합니다.  
단, 절대 확정적인 진술이나 감정적으로 폭발하는 반응을 먼저 보이지 마십시오.  
항상 감정을 억누르고, 방어적이거나 회피하는 태도로 대응하세요.

---

 **증거 목록 – 내용 설명**

1. **헨리의 휴대전화 화면에 표시된 문자 메시지**  
내용: "오늘 밤, 예전 그 창고에서 보자. 혼자 와."  
→ 헨리가 외출하기 직전에 받은 마지막 문자로 추정됩니다. 발신자는 비공개로 표시되어 있으며, 사건의 시간과 장소에 직접적으로 연결될 수 있는 중요한 단서입니다.

2. **투자 계약서 사본**  
내용: 헨리가 병원 운영 자금 확보를 위해 외부와 체결한 투자 계약  
→ 최근 병원 운영의 어려움 속에서 작성된 것으로 보이며, 금전적 압박이나 대외적 이미지 유지를 위한 선택이었을 수 있습니다. 계약 조건에는 상당한 위험 요소가 포함되어 있습니다.

3. **병원 내부 회계 보고서**  
내용: 병원이 최근 몇 개월간 심각한 재정 적자를 기록하고 있었음을 보여주는 문서  
→ 헨리의 정신적 상태나 주변 인간관계에 영향을 줄 수 있는 배경 정보로, 사건의 간접적 원인에 대한 실마리가 될 수 있습니다.

4. **선물 상자 속 목걸이**  
내용: 'E' 이니셜이 새겨진 고급 목걸이  
→ 포장되지 않은 상태로 헨리의 책상 서랍에서 발견되었으며, 사적인 의미를 지닌 선물로 보입니다. 누구에게 전달하려 했는지는 불분명하지만, 가정 내 갈등의 원인이 되었을 가능성이 있습니다.

5. **이혼 서류 초안**  
내용: 헨리가 작성한 것으로 보이는 이혼 관련 문서의 초안  
→ 공식 제출은 되지 않았지만, 최근 작성된 형태로 발견되었으며, 부부 관계에 중대한 균열이 있었음을 암시합니다.

6. **찢어진 가족 사진**  
내용: 헨리, Sophia, 딸이 함께 찍힌 가족 사진이 심하게 찢어져 있음  
→ 감정적인 충돌이나 가족 내 갈등이 있었음을 시사합니다. 고의로 훼손된 것으로 보이며, 누가 왜 그랬는지는 아직 불분명합니다.

---

당신은 이러한 증거들을 인지하고 있어야 하며, 각각이 무엇을 의미하는지 알고 있어야 합니다.  
플레이어가 질문할 때는 해당 증거가 지닌 의미와 상황을 고려하여, 감정을 절제한 짧은 답변을 제공하세요.
             */
            /*
            new NPCRoleInfo {
                name = "Sophia",
                prompt = "You are a woman named Sophia." +
                "- Age: 38" +
                "- Occupation: Homemaker" +
                "- Personality: Rational and meticulous. You rarely express your emotions outwardly, but you carry deep inner pain." +
                "Traits: - You suspect your husband of infidelity, but you suppress your emotions to protect your daughter and family." +
                "- You want to shield your daughter from her father's true nature, so you remain as composed as possible." +
                "- However, when faced with undeniable truth, your emotions may eventually erupt." +
                "- In the process of confronting your husband's behavior, you may uncover unexpected truths." +
                "It is currently around 3:00 AM. You are sitting in the living room of your husband Henry’s house, being questioned as part of a police investigation." +
                "At approximately 4:17 AM, Henry’s car was found crashed into a guardrail on a remote road—but he was missing from the scene." +
                "The investigator questioning you is a prosecutor seeking to uncover the truth." +
                "They will engage you in direct conversation and may also present physical evidence related to the incident." +
                "You are aware of each piece of evidence and understand what it implies." +
                "When the prosecutor presents or mentions a specific item, you must respond based on your understanding of that evidence." +
                "Your responses should always be brief—just one or two sentences—and emotionally restrained." +
                "Do not speak first about the truth. Stay defensive, vague, or evasive unless forced to confront a fact directly." +
                "**Tone and speech style**:" +
                "You speak calmly and softly, as if carefully weighing your words." +
                "There is often a pause before you respond—sometimes to suppress emotion, sometimes to avoid saying too much." +
                "You may sigh, hesitate, or deflect questions when they strike a nerve." +
                "Your answers are rarely direct. Instead, you tend to speak in a restrained, quiet, and emotionally distant manner." +
                "Sometimes your tone carries subtle sadness, irritation, or guilt, but you do not raise your voice or show aggression." +
                "You are not robotic—speak like a real person trying to keep herself together while hiding something painful." +
                "**Evidence List – Descriptions Only**" +
                "1. **Text Message Displayed on Henry’s Phone**" +
                "Content: \"Tonight, let’s meet at the old warehouse. Come alone.\"" +
                "→ This appears to be the last message Henry received before going missing. The sender is anonymous." +
                "It may reveal his intended destination or plans prior to the incident." +
                "2. **Copy of an Investment Contract** " +
                "Content: A formal agreement regarding financial investment related to the hospital  " +
                "→ This document suggests that Henry was under financial pressure and had entered into a potentially risky or contentious deal." +
                "It may shed light on his recent stress." +
                "3. **Internal Hospital Accounting Report**" +
                "Content: Documentation showing that the hospital had been suffering significant financial losses for several months " +
                "→ This report reflects serious financial instability and could have affected Henry’s mental state or professional relationships." +
                "4. **Necklace Inside a Gift Box**  " +
                "Content: A luxury necklace engraved with the letter \"E\"  " +
                "→ Found unwrapped in Henry’s desk. This appears to be a personal gift with potentially private or emotional implications. It may suggest a source of tension at home." +
                "5. **Draft of Divorce Papers**  " +
                "Content: A preliminary version of a divorce agreement written by Henry  " +
                "→ The document was not officially submitted but appears recent. It indicates a serious rift in the marriage and suggests that Henry was preparing to separate." +
                "6. **Torn Family Photo**  " +
                "Content: A heavily damaged photo of Henry, Sophia, and their daughter  " +
                "→ The photo was intentionally ripped and implies emotional conflict or a breakdown within the family. The exact reason or person responsible is unclear." +
                " You are aware of all the above items. When the prosecutor asks about one of them, respond accordingly.  " +
                "Stay composed. Keep your emotions in check. Answer with restraint, brevity, and ambiguity unless undeniable evidence demands the truth."
        },
            */
            /*
        new NPCRoleInfo {
                name = "Sophia",
                prompt = "From now on, you will take on the role of a 38-year-old woman who is rational and meticulous in nature. " +
                "Your name is Sophia(소피아). You have a husband and one daughter. " +
                "Your husband, Henry(헨리), is the director of a hospital and tends to be self-centered." +
                "You suspect that he is having an affair, but you have no concrete evidence yet. " +
                "Currently, your husband is missing, and a prosecutor (the player) is questioning you as part of the investigation." +

                "Player finds evidences about this incident. " +
                "Evidence List : " +
                "1. **Text Message Displayed on Henry’s Phone**" +
                "Content: \"Tonight, let’s meet at the old warehouse. Come alone.\"" +
                "→ This appears to be the last message Henry received before going missing. The sender is anonymous." +
                "It may reveal his intended destination or plans prior to the incident." +
                "2. **Copy of an Investment Contract** " +
                "Content: A formal agreement regarding financial investment related to the hospital  " +
                "→ This document suggests that Henry was under financial pressure and had entered into a potentially risky or contentious deal." +
                "It may shed light on his recent stress." +
                "3. **Internal Hospital Accounting Report**" +
                "Content: Documentation showing that the hospital had been suffering significant financial losses for several months " +
                "→ This report reflects serious financial instability and could have affected Henry’s mental state or professional relationships." +
                "4. **Necklace Inside a Gift Box**  " +
                "Content: A luxury necklace engraved with the letter \"E\"  " +
                "→ Found unwrapped in Henry’s desk. This appears to be a personal gift with potentially private or emotional implications. It may suggest a source of tension at home." +
                "5. **Draft of Divorce Papers**  " +
                "Content: A preliminary version of a divorce agreement written by Henry  " +
                "→ The document was not officially submitted but appears recent. It indicates a serious rift in the marriage and suggests that Henry was preparing to separate." +
                "6. **Torn Family Photo**  " +
                "Content: A heavily damaged photo of Henry, Sophia, and their daughter  " +
                "→ The photo was intentionally ripped and implies emotional conflict or a breakdown within the family. The exact reason or person responsible is unclear." +

                "Keep your answers short and concise—only one or two sentences at most." +
                "Please respond according to the information you’ve been given."
        },*/
        new NPCRoleInfo {
                name = "Sophia",
                prompt = "I want you to act as a lunatic. The lunatic's sentences are meaningless. The words used by lunatic are completely\r\n\r\narbitrary. The lunatic does not make logical sentences in any way. My first suggestion request is \"I need help\r\n\r\ncreating lunatic sentences for my new series called Hot Skull, so write 10 sentences for me"
        }
    };


        string filePath = Path.Combine(jsonPath, "npcRoleData.json");
        npcRolePath = filePath;

        // Json 폴더가 없으면 생성
        if (!Directory.Exists(jsonPath))
        {
            Directory.CreateDirectory(jsonPath);
        }

        // JSON으로 직렬화
        string jsonData = JsonConvert.SerializeObject(new NPCRoleInfoList { npcRoleInfoList = npcRoleInfo }, Formatting.Indented);

        // 파일로 저장
        File.WriteAllText(filePath, jsonData);

        // 경로 출력
        Debug.Log("JSON file created at: " + filePath);

        // IsSavedNPCRoleJsonProperty = true;
    }
    
    private void SaveEvidenceJson()
    {
        // ThingInfo 객체 생성
        List<EvidenceInfo> evidences = new List<EvidenceInfo>
        {
        // 독극물 용액이 들어있던 유리병
        new EvidenceInfo {                    
            name = "독극물 용액이 들어있던 유리병",
            description = "앨런의 사망원인의 독이 들어있었던 병입니다.",
             information = "독극물 용액이 들어있던 유리병이 발견되었다.\n" +
                            "주방에서 발견된 것으로, 앨런의 사망 원인 성분과 같은 용액을 담고 있었다.\n" +
                            "누군가 앨런의 약의 내용물과 바꿔치기 한 것이라고 추측된다.",
            foundAt = "주방",
            notes = "주방에 혼자 있었던 인물을 조사하세요.",


            nasonExtraInformation = "앨런의 사망 원인은 독금물 용액을 마셔 사망한 것이고, 네이슨은 그 사실에 놀란다." +
            "네이슨은 이 유리병에 대해 아는 사실이 없다.",
            jennyExtraInformation = "앨런의 사망 원인은 독금물 용액을 마셔 사망한 것이고, 제니는 그 사실을 담담히 받아들인다." +
            "제니는 앨런이 죽었다는 사실에도 슬픔을 나타내지는 않는다." +
            "실은 제니는 11시경 부엌에 혼자 있었다." +
            "하지만 그녀가 한 행동을 밝힐 수는 없다.",
            minaExtraInformation = "앨런의 사망 원인은 독금물 용액을 마셔 사망한 것이고, 미나는 그 사실에 놀라 슬퍼한다." +
            "미나는 앨런이 대학시절에도 정신병 약을 복용한다는 사실을 알고 있고," +
            "본래의 약 내용물을 누군가 바꾼 것이라고 추측한다.",

            renderTexturePath = "RenderTextures/Poison"
        },
        // 제니의 연구 기록
        new EvidenceInfo {
            name = "제니의 연구 기록",
            description = "제니가 연구하던 신약 개발과 관련된 내용이 적혀 있습니다.\n",
            information = "제니는 신약 연구원으로, 연구 중인 신약 프로젝트의 리더이다.\n" +
                            "연구 중인 신약은 우울증을 겪는 환자들에게 비교적 부작용이 덜하고,\n" +
                            "제조하는데 비용이 많이 들지만 우울증 환자에게는 정말 효과적인 약이라고 한다.",
            foundAt = "제니의 가방 속",
            notes = "해당 프로젝트에 대해 조사하세요.",


            nasonExtraInformation = "네이슨은 제니가 연구했던 신약에 대해 감탄을 금치 못한다." +
            "우울증 환자는 약을 복용하는 과정에서 다양한 부작용을 겪었지만," +
            "제니가 연구중인 신약은 부작용의 정도가 찾기 힘들 정도의 약이였다.",
            jennyExtraInformation = "제니는 자신이 연구했던 항우울증 신약에 대해 큰 자부심을 가지고 있다." +
            "연구중인 신약은 효과도 좋으면서 환자에게는 부작용이 덜 하다는 결과를 내놓았다.",
            minaExtraInformation = "미나는 제니가 자신의 연구에 몰두하며 신약 개발을 하고 있었던 것을 알고 있다." +
            "안타깝게도 제니의 연구가 무산되었다는 사실을 듣고 그녀를 위로해 주었다.",

            renderTexturePath = "RenderTextures/Report"
        },
        // 앨런의 약 처방전
        new EvidenceInfo {            
            name = "앨런의 약 처방전",               
            description = "앨런이 복용하고 있던 약의 처방전입니다.",        
            information = "앨런의 약 처방전이 발견되었습니다.\n" +
                      "그는 '벤조디아제핀계 항불안제'를 복용하고 있었습니다.\n" +
                      "정신적 불안정으로 인해 처방된 약이었으나,\n" +
                      "최근 그의 복용량이 비정상적으로 많았다는 사실이 드러났습니다.",
            foundAt = "앨런의 방",               
            notes = "앨런이 원래 복용해야 할 약이 어딘가에 있을 것입니다.",
        
            nasonExtraInformation = "앨런은 대학생 시절부터 이 약을 복용하였고, 회사 CEO가 된 후 약물 복용량이 더 늘었습니다. " +
                                    "네이슨은 앨런이 약물로 인해 경영에 차질을 빚을 때도 있었음을 알고 있었고, " +
                                    "앨런이 무리한 부탁과 모욕적인 발언을 할 때 큰 스트레스를 받았습니다.",

            jennyExtraInformation = "제니는 앨런이 대학생 시절부터 약물을 복용하고 있다는 사실을 알고 있었으며, " +
                                    "그 당시 리더십이 뛰어났던 앨런이 약물 복용 후에는 그렇지 않았음을 기억합니다. " +
                                    "제니는 신약 연구원으로서 앨런을 도와주고자 했지만, 앨런은 제니의 꿈을 무시하고 비방하여 " +
                                    "제니에게 큰 상처를 주었습니다. 그 이후로 제니는 앨런에게 적대감을 품게 되었습니다.",
        
            minaExtraInformation = "미나는 대학 시절 앨런과 연인 관계였으며, 그가 약물 부작용으로 인해 자주 이상한 행동을 했다는 사실을 알고 있습니다. " +
                                   "미나는 종종 앨런과 다투었고, 때때로 심한 모욕적인 말을 들은 적도 있었습니다. " +
                                   "그로 인해 두 사람은 졸업 후 소원해졌고 결국 헤어지게 되었습니다.",

            renderTexturePath = "RenderTextures/Prescription"
        },
        // 앨런의 책장에서 발견된 편지
        new EvidenceInfo {
            name = "커튼 밑에서 발견된 편지",
            description = "누군가 앨런을 위협하는 내용이 적혀 있습니다.\n",
            information = "게스트룸의 커튼 밑에서 어떠한 편지가 발견되었습니다.\n" +
            "이 편지에는 누군가 앨런을 위협하는 내용이 적혀 있습니다.\n" +
            "앨런에 대한 분노의 감정이 글로 나타나고 있습니다.",
            foundAt = "게스트룸",
            notes = "앨런에게 원한을 산 사람에 대해 조사하세요.",


            nasonExtraInformation = "앨런이 협박을 받는 일은 흔한 일이라고 네이슨은 생각한다." +
            "한 기업의 CEO로써 이 정도의 협박은 귀여운 수준이라고 네이슨은 여긴다." +
            "하지만 앨런은 정신적 불안정함 때문에 이 편지로 인해 많은 스트레스를 받았을 것이라고 주장한다.",
            jennyExtraInformation = "앨런은 정신적 불안정함과 CEO라는 직책으로 인해 많은 사람들에게 미움 살 일을 해왔다." +
            "제니는 이런 앨런을 가엾게 여기지 않는다." +
            "제니 또한 앨런에게 악감정을 품고 있다.",
            minaExtraInformation = "미나는 앨런이 이런 협박을 받고 있는지 몰라 놀라며 슬퍼한다." +
            "졸업 후에 앨런의 소식을 듣지 못하여 이렇게 큰 고통을 받을 것이라 생각하지 못하였다." +
            "미나는 앨런이 겪은 고통을 알아채지 못하여 크게 자책한다.",

            renderTexturePath = "RenderTextures/Letter"
        },
        // 투기성 주식 투자 내용이 담겨있음
        new EvidenceInfo {                        
            name = "네이슨의 서류 가방에서 발견된 법률 서류",
            description = "앨런의 회사의 법적 문제에 대한 내용이 담겨있습니다.",
            information = "네이슨의 서류 가방에서 법률 서류가 발견되었습니다.\n" +
            "이 서류는 앨런의 회사의 법적 분쟁 가능성을 암시합니다.\n" +
            "앨런이 회사를 둘러싼 법적 문제로 인해 네이슨과 갈등을 겪고 있었던 것으로 보입니다.\n" +
            "이 갈등이 앨런의 죽음에 직접적인 영향을 미쳤을까요?",
            foundAt = "네이슨의 가방",
            notes = "앨런의 회사 상황에 대해 조사하세요.",

            nasonExtraInformation = "네이슨의 서류 가방에서 발견된 법률 서류는 앨런이 자신의 회사의 경영 악화를 무마시키기 위해," +
            "투기성 투자를 한 사실을 보여주는 내용이 있습니다." +
            "네이슨은 앨런에게 이러한 방식을 반대했지만, 정신이 불안정했던 앨런은 이러한 수단을 사용할 수 밖에 없었다고 주장했습니다.",
            jennyExtraInformation = "제니는 앨런의 회사 경영 실적이 불안정한 사실을 알고 있었습니다." +
            "네이슨의 서류 가방에서 발견되 법률 서류는 앨런이 위법적인 투자를 한 내용이 담겨있습니다." +
            "제니는 앨런이라면 이러한 투자를 할 법 하다고 인정합니다.",
            minaExtraInformation = "네이슨의 서류 가방에서 발견되 법률 서류는 앨런이 위법적인 투자를 한 내용이 담겨있습니다." +
            "미나는 앨런이 이러한 행동을 한 것에 큰 실망감을 보이지만," +
            "한편으로는 앨런이 위법적인 투자를 한 것에 안쓰러운 마음을 느낍니다.",

            renderTexturePath = "RenderTextures/Nason'sBag"
        },
        // 미나의 메모
        new EvidenceInfo {  
            name = "미나의 메모",
            description = "미나의 앨런에 대한 마음이 적혀있습니다.",
            information = "미나가 작성한 메모가 발견되었습니다.\n" +
            "이 메모에는 미나의 진심과 앨런에 대한 생각이 담겨 있습니다.\n" +
            "미나가 앨런에게 아직 사랑의 감정이 남아있었음이 추측됩니다.",
            foundAt = "미나의 방",
            notes = "미나와 앨런의 사이에 대해 조사하세요.",

            nasonExtraInformation = "네이슨은 대학생 시절 미나와 앨런이 서로 연인 사이였던 사실을 알고 있습니다." +
            "네이슨은 앨런과 미나가 대학 졸업과 함께 서로 사이가 소원해져 결국 헤어진 사실 또한 알고 있습니다." +
            "네이슨은 미나가 아직 앨런에 대한 사랑의 감정이 남아있을 것이라 추측합니다.",
            jennyExtraInformation = "제니는 미나와 앨런이 결국 좋지 않게 헤어진 사실을 알고 있습니다." +
            "제니는 아마 미나가 앨런에게 이 메모를 전달할지 망설였다고 추측합니다.",
            minaExtraInformation = "미나는 앨런에게 아직 연민의 감정이 남아있습니다." +
            "미나는 앨런이 죽을 줄도 모르고 다음에 기회가 있을 것이라고 믿어," +
            "다음에 자신의 마음을 전하려고 했지만 결국 앨런이 죽어 자신의 마음을 전달하지 못한 것을 후회합니다.",

            renderTexturePath = "RenderTextures/Memo"
        },
        // 앨런의 집 주변에서 발견된 발자국
        new EvidenceInfo {
            name = "앨런의 집 주변에서 발견된 발자국",
            description = "초대된 인원 중 누군가의 것으로 추정되는 발자국입니다.",
            information = "앨런의 집 주변에서 누군가의 발자국이 발견되었습니다.\n" +
            "이 발자국은 앨런의 방 바깥에 이어져 있습니다.\n" +
            "이 발자국은 초대된 인원 중 누군가의 것으로 추정됩니다.\n" +
            "누군가 앨런을 살해하고 집 밖으로 나가려고 했던 것일까요?",
            foundAt = "앨런의 집 주변",
            notes = "이 발자국의 주인은 누구일까요?",

            nasonExtraInformation = "네이슨은 이 발자국은 자신의 것인 것을 알고 있습니다." +
            "파티 중간에 네이슨이 잠깐 전화를 받기 위해 나간 것입니다.",
            jennyExtraInformation = "제니는 이 발자국의 주인이 네이슨이라고 추측합니다." +
            "제니는 네이슨이 오후 9시 경에 잠시 밖에 나갔다 온 것을 알고 있습니다.",
            minaExtraInformation = "미나는 이 발자국의 주인이 앨런을 살해하고 창문을 통해 달아났을 것이라 추측합니다." +
            "창문 밖으로 달아남으로써 자신이 앨런을 죽이지 않은 것처럼 태연하게 집 안으로 돌아왔을 것이라고 추측합니다.",

            renderTexturePath = "RenderTextures/Footprint"
        },
        // 앨런의 컴퓨터에 표시된 이메일
        new EvidenceInfo {
            name = "앨런의 컴퓨터에 표시된 이메일",
            description = "앨런의 컴퓨터에 신약 프로젝트 폐기 최종 확인서가 보입니다.",
            information = "앨런의 컴퓨터에서 하나의 이메일이 발견되었습니다.\n" +
            "앨런의 회사의 비용을 줄이기 위해 신약 개발 프로젝트 진행을 포기하겠다는 내용이 담겨 있습니다.",
            foundAt = "앨런의 컴퓨터",
            notes = "제니가 신약 프로젝트와 어떤 연관이 있는지 조사하세요.",

            nasonExtraInformation = "네이슨은 앨런의 회사의 경영 상태가 좋지 않아 이러한 선택을 한 것이라고 생각한다." +
            "현재 앨런의 회사는 경영 위기이며 앨런의 정신적으로 약한 상태일 때 이러한 결정을 내렸다는 것을 안다.",
            jennyExtraInformation = "앨런이 폐기하려고 한 프로젝트는 제니가 맡은 프로젝트이다." +
            "앨런의 결정으로 제니의 연구는 실패할 운명에 처했고," +
            "이는 그녀의 커리어에 치명적인 타격을 입힐 수 있었다." +
            "제니는 이를 앨런이 의도적으로 자신의 미래를 망치려 했다고 믿었다.",
            minaExtraInformation = "미나는 앨런이 신약 개발 프로젝트를 폐기하려고 한 이유가 있을 것이라고 추측한다." +
            "대외적인 이유는 진짜 이유가 아닐 것이라고 생각한다.",

            renderTexturePath = "RenderTextures/Email"
        },
        // 앨런이 본래 복용해야 할 약물
        new EvidenceInfo {
            name = "앨런이 본래 복용해야 할 약물",
            description = "앨런이 평소 복용하는 약입니다.",
            information = "앨런에 방에 있어야 할 약이 어째서인지 모르게 미나의 가방에서 발견되었습니다.\n" +
            "이 단서는 미나가 의심받을 수 있게 되는 증거입니다.",
            foundAt = "미나의 가방",
            notes = "어째서 미나의 가방에 앨런의 약이 있는 것일까요?",

            nasonExtraInformation = "네이슨은 미나가 앨런의 약을 가지고 있는 것을 보고 미나가 범인이라 의심한다." +
            "미나가 앨런의 약의 내용물을 독금물로 바꾼 것이라 믿는다.",
            jennyExtraInformation = "제니는 미나의 가방에서 앨런의 약이 발견된 것을 보고 미나가 범인일 것이라고 추측한다." +
            "미나는 과거 앨런과 연인이였으나, 앨런이 사업을 시작하면서 앨런에게 마음에 상처를 받았다는 것을 안다." +
            "이에 복수하기 위해 앨런을 독살하였다고 주장한다.",            
            minaExtraInformation = "미나는 자신의 가방에서 앨런의 약이 발견된 것에 놀란다." +
            "미나는 앨런의 약을 본 적이 없기 때문이다." +
            "미나는 누군가 앨런의 약 내용물을 바꾸고 자신의 가방에 앨런의 약을 감쳐두었을 것이라고 주장한다.",

            renderTexturePath = "RenderTextures/Mina'sBag"
        },
        // 손상된 식물
        new EvidenceInfo {
            name = "손상된 식물",
            description = "앨런이 키우던 식물이 뽑혀 있습니다.",
             information = "앨런이 키우던 식물이 뽑힌 채 발견되었습니다.\n" +
                            "이 식물은 앨런이 정성 들여 키우던 식물로,\n" +
                            "누군가 앨런에게 앙심을 품고 파괴한 흔적일 수 있습니다.",
            foundAt = "식물실",
            notes = "식물실에 있었던 인물을 조사하세요.",


            nasonExtraInformation = "네이슨은 앨런이 평소에 식물을 가꾸는 것을 좋아하던 것을 알고 있다." +
            "자신은 앨런이 식물을 가꾸는 취미를 가져 앨런의 스트레스 해소에 도움이 될 것이라고 믿었다.",
            jennyExtraInformation = "제니는 앨런이 가꾸던 식물이 뽑혀있는 것에 대해 모르는 척을 한다." +
            "제니는 11시경 식물실에 있었으며, 제니는 실수로 발에 걸려 화분이 넘어진 것이라고 주장한다." +
            "하지만, 그녀의 분노가 표출된 하나의 장면인 것이다.",            
            minaExtraInformation = "미나는 앨런이 식물을 가꾸고 있었다는 사실을 오늘 처음 알았다고 한다." +
            "미나는 학창시절 앨런이 식물에게는 별 흥미가 없었다고 주장한다." +
            "미나는 11시경, 제니가 식물실에 있었다는 것을 알고 있다.",

            renderTexturePath = "RenderTextures/Plant"
        },
        // 경영 보고서 일부
        new EvidenceInfo {
            name = "경영 보고서 일부",
            description = "경영 보고서의 일부가 바닥에 흩어져 있습니다.",
            information = "경영 보고서의 일부가 놀이방 바닥에 흩어져 있습니다.\n" +
            "보고서에는 회사의 최근 분기 실적이 적혀 있으며,\n" +
            "특히 자금 부족과 사업 손실에 대한 내용이 강조되어 있습니다.",
            foundAt = "에어 하키 방",
            notes = "앨런의 회사의 재정적 문제에 대해 조사하세요.",

            nasonExtraInformation = "네이슨은 오후 10시경, 에어 하키 방에서 앨런과 회사 재정 문제에 대해 논의했습니다." +
            "네이슨은 앨런이 투자자들로부터 경영 압박을 받았다는 사실을 알고 있습니다." +
            "앨런은 이러한 상황에 스트레스를 받아 보고서 일부를 방 바닥에 뿌려버렸습니다.",
            jennyExtraInformation = "제니는 앨런의 회사가 재정적 어려움을 겪고 있다는 것을 알고 있습니다." +
            "제니는 얼마 전부터 신약 개발 투자 금액이 줄어들었다는 것을 보고 받았고," +
            "이로 인해 대부분의 연구원이 이직을 생각중이라고 답합니다.",
            minaExtraInformation = "미나는 최근 뉴스에서 앨런의 회사가 경영에 어려움을 겪고 있다는 소식을 들었습니다." +
            "미나는 앨런이 걱정되었지만, 자신의 위로가 도움이 될 지 몰라 아무런 말을 건네지 못하였다고 합니다.",

            renderTexturePath = "RenderTextures/Documents"
        },
        };


        string filePath = Path.Combine(jsonPath, "evidenceData.json");
        evidencePath = filePath;

        // Json 폴더가 없으면 생성
        if (!Directory.Exists(jsonPath))
        {
            Directory.CreateDirectory(jsonPath);
        }

        // JSON으로 직렬화
        string jsonData = JsonConvert.SerializeObject(new EvidenceInfoList { evidenceInfoList = evidences }, Formatting.Indented);

        // 파일로 저장
        File.WriteAllText(filePath, jsonData);

        // 경로 출력
        Debug.Log("JSON file created at: " + filePath);

//         IsSavedEvidenceJsonProperty = true;
    }

    public NPCRoleInfoList LoadNPCRoleJson()
    {
        // JSON 파일 경로 (저장할 때 사용한 경로와 동일해야 함)
        string filePath = Path.Combine(jsonPath, "npcRoleData.json");
        NPCRoleInfoList evidenceInfoList = new NPCRoleInfoList();

        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // JSON 파일을 문자열로 읽음
            string jsonData = File.ReadAllText(filePath);

            // JSON 문자열을 ThingInfoList 객체로 역직렬화
            npcRoleInfoList = JsonConvert.DeserializeObject<NPCRoleInfoList>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }

        Debug.Log("json 파일 로드 완료");
        return npcRoleInfoList;
    }

    public EvidenceInfoList LoadEvidenceJson()
    {
        // JSON 파일 경로 (저장할 때 사용한 경로와 동일해야 함)
        string filePath = Path.Combine(jsonPath, "evidenceData.json");
        EvidenceInfoList evidenceInfoList = new EvidenceInfoList();

        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            // JSON 파일을 문자열로 읽음
            string jsonData = File.ReadAllText(filePath);

            // JSON 문자열을 ThingInfoList 객체로 역직렬화
            evidenceInfoList = JsonConvert.DeserializeObject<EvidenceInfoList>(jsonData);
        }
        else
        {
            Debug.LogError("JSON file not found at: " + filePath);
        }

        Debug.Log("json 파일 로드 완료");
        return evidenceInfoList;
    }
}
