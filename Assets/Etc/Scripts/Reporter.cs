using AdvancedPeopleSystem;
using UnityEngine;
using System.Collections;

public class Reporter : NPC
{
    protected void Awake()
    {
        base.Awake();
        StartCoroutine(RepeatBlendshapeAnimation());
    }

    private IEnumerator RepeatBlendshapeAnimation()
    {
        while (true) // 애니메이션이 계속 반복 실행됨
        {
            cc.PlayBlendshapeAnimation("Talk", 3f); // 애니메이션 실행
            yield return new WaitForSeconds(2f); // 1초 후 다시 실행 (원하는 대로 조정 가능)
        }
    }
}
