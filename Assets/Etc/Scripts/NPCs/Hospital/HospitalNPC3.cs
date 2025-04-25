using AdvancedPeopleSystem;
using UnityEngine;
using Unity.Cinemachine;

public class HospitalNPC3 : MonoBehaviour
{
    protected Animator animator;
    protected CharacterCustomization cc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterCustomization>();
        
        cc.InitColors();

        // 외형 랜덤화
        RandomizeAppearance();
    }

    void RandomizeAppearance()
    {
        if (cc.Settings == null)
        {
            Debug.LogError("Character settings not assigned.");
            return;
        }

        // 각 요소 프리셋 수 확인 후 랜덤 인덱스 지정
        cc.SetElementByIndex(CharacterElementType.Hair, Random.Range(0, cc.Settings.hairPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Beard, Random.Range(0, cc.Settings.beardPresets.Count));

        // 키와 머리 크기 조정 (범위 조정 가능)
        cc.SetHeight(Random.Range(-0.1f, 0.1f)); // 약간 키 차이        
        cc.SetHeadSize(Random.Range(-0.1f, 0.1f)); // 약간 머리 크기 차이        

        // 머리카락 색상 랜덤 설정 (자연스러운 컬러 범위 예시)
        Color hairColor = Random.ColorHSV(0f, 0.1f, 0.2f, 0.6f, 0.2f, 0.6f); // 어두운 갈색~블랙 톤
        cc.SetBodyColor(BodyColorPart.Hair, hairColor);

        // 눈 색상 랜덤 설정 (자연스러운 파랑/초록/갈색 범위 예시)
        Color eyeColor = Random.ColorHSV(0.1f, 0.6f, 0.3f, 0.8f, 0.4f, 0.8f);
        cc.SetBodyColor(BodyColorPart.Eye, eyeColor);

        // 상의와 하의 랜덤 설정
        cc.SetElementByIndex(CharacterElementType.Shirt, Random.Range(0, cc.Settings.shirtsPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Pants, Random.Range(0, cc.Settings.pantsPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Accessory, Random.Range(0, cc.Settings.accessoryPresets.Count));
    }
}