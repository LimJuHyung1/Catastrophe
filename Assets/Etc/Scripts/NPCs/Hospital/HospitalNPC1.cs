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

    public SplineContainer splineContainer; // 스플라인 컨테이너

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterCustomization>();
        cinemachineSplineCart = GetComponent<CinemachineSplineCart>();
        
        cinemachineSplineCart.SplinePosition = 0; // 시작 위치 설정

        cc.InitColors();

        // 외형 랜덤화
        RandomizeAppearance();

        cinemachineSplineCart.Spline = splineContainer; // 스플라인 설정
    }

    private void FixedUpdate()
    {
        if (cinemachineSplineCart.SplinePosition >= 1)
        {
            Deactivate(); // 스플라인 끝에 도달하면 비활성화
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
    }

    public void ReAssignSpline(SplineContainer assignedSplineContainer)
    {
        cinemachineSplineCart.Spline = null;
        cinemachineSplineCart.Spline = assignedSplineContainer; // 스플라인 설정
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