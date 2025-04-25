using AdvancedPeopleSystem;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Splines;

public class HospitalNPC1 : MonoBehaviour
{
    protected Animator animator;
    protected CharacterCustomization cc;
    protected CinemachineSplineCart cinemachineSplineCart;
    protected PoolingManager poolingManager;

    public SplineContainer splineContainer; // ���ö��� �����̳�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterCustomization>();
        cinemachineSplineCart = GetComponent<CinemachineSplineCart>();
        
        cinemachineSplineCart.SplinePosition = 0; // ���� ��ġ ����

        cc.InitColors();

        // ���� ����ȭ
        RandomizeAppearance();

        cinemachineSplineCart.Spline = splineContainer; // ���ö��� ����
    }

    private void FixedUpdate()
    {
        if (cinemachineSplineCart.SplinePosition >= 1)
        {
            Deactivate(); // ���ö��� ���� �����ϸ� ��Ȱ��ȭ
            cinemachineSplineCart.SplinePosition = 0;
        }
    }

    public void RandomizeAppearance()
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
    }

    public void ReAssignSpline(SplineContainer assignedSplineContainer)
    {
        cinemachineSplineCart.Spline = null;
        cinemachineSplineCart.Spline = assignedSplineContainer; // ���ö��� ����
        cinemachineSplineCart.SplinePosition = 0;
    }


    public void Initialize(PoolingManager assignedPool, SplineContainer splineContainer)
    {
        poolingManager = assignedPool;
        this.splineContainer = splineContainer;
    }

    void Deactivate()
    {
        poolingManager?.ReturnToPool(gameObject);
    }
}