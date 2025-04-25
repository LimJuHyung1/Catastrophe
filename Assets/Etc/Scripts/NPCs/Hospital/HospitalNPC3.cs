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

        // ���� ����ȭ
        RandomizeAppearance();
    }

    void RandomizeAppearance()
    {
        if (cc.Settings == null)
        {
            Debug.LogError("Character settings not assigned.");
            return;
        }

        // �� ��� ������ �� Ȯ�� �� ���� �ε��� ����
        cc.SetElementByIndex(CharacterElementType.Hair, Random.Range(0, cc.Settings.hairPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Beard, Random.Range(0, cc.Settings.beardPresets.Count));

        // Ű�� �Ӹ� ũ�� ���� (���� ���� ����)
        cc.SetHeight(Random.Range(-0.1f, 0.1f)); // �ణ Ű ����        
        cc.SetHeadSize(Random.Range(-0.1f, 0.1f)); // �ణ �Ӹ� ũ�� ����        

        // �Ӹ�ī�� ���� ���� ���� (�ڿ������� �÷� ���� ����)
        Color hairColor = Random.ColorHSV(0f, 0.1f, 0.2f, 0.6f, 0.2f, 0.6f); // ��ο� ����~�� ��
        cc.SetBodyColor(BodyColorPart.Hair, hairColor);

        // �� ���� ���� ���� (�ڿ������� �Ķ�/�ʷ�/���� ���� ����)
        Color eyeColor = Random.ColorHSV(0.1f, 0.6f, 0.3f, 0.8f, 0.4f, 0.8f);
        cc.SetBodyColor(BodyColorPart.Eye, eyeColor);

        // ���ǿ� ���� ���� ����
        cc.SetElementByIndex(CharacterElementType.Shirt, Random.Range(0, cc.Settings.shirtsPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Pants, Random.Range(0, cc.Settings.pantsPresets.Count));
        cc.SetElementByIndex(CharacterElementType.Accessory, Random.Range(0, cc.Settings.accessoryPresets.Count));
    }
}