using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.Splines;



public class PoolingManager : MonoBehaviour
{
    public GameObject[] doctors;
    public GameObject[] walkingNPC;

    public GameObject[] sittingNPC;
    public GameObject[] sitAndTalkingNPC;
    
    public Transform[] waitingSeat;
    public Transform[] talkingSeat;
    public Transform[] idleSeat;
    public Transform[] table;


    public SplineContainer[] splineContainers;



    private Queue<GameObject> pool = new Queue<GameObject>();
    private Dictionary<Transform, bool> waitingSeatDict = new Dictionary<Transform, bool>();

    [SerializeField] private int currentCount = 0;
    private int waitingNPCCount = 6;
    private int talkingNPCCount = 3;
    private int maxSize = 15;               // 최대 생성 개수    
    private float spawnInterval = 2f;       // 오브젝트 생성 간격 (초)



    void Start()
    {
        SetSittingNPC();
        SetSitAndTalkingNPC();
        StartCoroutine(SpawnObjectsPeriodically());
    }

    void SetSittingNPC()
    {
        waitingSeatDict.Clear(); // 시작 시 좌석 상태 초기화

        int count = 0;
        while (count < waitingNPCCount && waitingSeatDict.Count < waitingSeat.Length)
        {
            int npcIndex = Random.Range(0, sittingNPC.Length);
            int seatIndex = Random.Range(0, waitingSeat.Length);
            Transform chosenSeat = waitingSeat[seatIndex];

            // 이미 누군가 앉은 자리면 스킵
            if (waitingSeatDict.ContainsKey(chosenSeat))
                continue;

            // 앉을 위치와 방향 설정
            Vector3 spawnPosition = chosenSeat.position;
            Vector3 leftDirection = -chosenSeat.right; // forward 기준으로 왼쪽 90도 방향
            Quaternion spawnRotation = Quaternion.LookRotation(leftDirection);

            Instantiate(sittingNPC[npcIndex], spawnPosition, spawnRotation);

            // 해당 좌석을 사용 중으로 표시
            waitingSeatDict.Add(chosenSeat, true);
            count++;
        }
    }

    void SetSitAndTalkingNPC()
    {
        int count = Mathf.Min(talkingNPCCount, Mathf.Min(talkingSeat.Length, table.Length));

        for (int i = 0; i < count; i++)
        {
            int npcIndex = Random.Range(0, sitAndTalkingNPC.Length);
            int npcIndex2 = Random.Range(0, sittingNPC.Length);

            Transform seat1 = talkingSeat[i];
            Transform seat2 = idleSeat[i];
            Transform targetTable = table[i];

            Vector3 spawnPosition = seat1.position;
            Vector3 spawnPosition2 = seat2.position;
            Vector3 lookDirection = (targetTable.position - seat1.position).normalized;
            Vector3 lookDirection2 = (targetTable.position - seat2.position).normalized;

            Quaternion spawnRotation = Quaternion.LookRotation(lookDirection);
            Quaternion spawnRotation2 = Quaternion.LookRotation(lookDirection2);

            Instantiate(sitAndTalkingNPC[npcIndex], spawnPosition, spawnRotation);
            Instantiate(sittingNPC[npcIndex2], spawnPosition2, spawnRotation2);
        }
    }


    private IEnumerator SpawnObjectsPeriodically()
    {
        while (true)
        {
            int activeObjects = currentCount - pool.Count;

            if (activeObjects < 6)
            {
                GameObject obj = null;
                bool isNewlyCreated = false;

                int randSplineContainerIndex = Random.Range(0, splineContainers.Length);
                SplineContainer selectedSplineContainer = splineContainers[randSplineContainerIndex];
                Spline targetSpline = selectedSplineContainer.Spline;
                BezierKnot knot = targetSpline[0];
                Vector3 spawnPosition = (Vector3)knot.Position;

                // doctor 또는 walkingNPC 중 랜덤 선택
                GameObject prefabToSpawn;
                if (Random.value < 0.5f && doctors.Length > 0)
                {
                    int randDoctorIndex = Random.Range(0, doctors.Length);
                    prefabToSpawn = doctors[randDoctorIndex];
                }
                else if (walkingNPC.Length > 0)
                {
                    int randWalkingIndex = Random.Range(0, walkingNPC.Length);
                    prefabToSpawn = walkingNPC[randWalkingIndex];
                }
                else
                {
                    yield return null;
                    continue;
                }

                // 풀에 남아있으면 재사용
                if (pool.Count > 0)
                {
                    obj = pool.Dequeue();
                    obj.transform.position = spawnPosition;
                    obj.transform.rotation = Quaternion.identity;

                    int reAssignIndex = Random.Range(0, splineContainers.Length);
                    obj.GetComponent<HospitalNPC1>().ReAssignSpline(splineContainers[reAssignIndex]);                    
                    obj.GetComponent<HospitalNPC1>().RandomizeAppearance(); // 외형 랜덤화

                    Debug.Log("재할당");
                }
                // 풀에 없고 최대 개수 미만이면 새로 생성
                else if (currentCount < maxSize)
                {
                    obj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                    currentCount++;
                    isNewlyCreated = true;
                }

                if (obj != null)
                {
                    obj.SetActive(true);

                    // 새 splineContainer를 할당해주기
                    HospitalNPC1 npc = obj.GetComponent<HospitalNPC1>();
                    if (npc != null)
                    {
                        npc.Initialize(this, selectedSplineContainer);                        
                    }
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public GameObject Get(Spline spline)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return null; // 더 이상 남은 오브젝트 없음
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);  // 꼭 꺼줘야 풀에 의미 있음
        pool.Enqueue(obj);
    }
}
