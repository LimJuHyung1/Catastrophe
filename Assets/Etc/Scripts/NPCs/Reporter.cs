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
        while (true) // �ִϸ��̼��� ��� �ݺ� �����
        {
            cc.PlayBlendshapeAnimation("Talk", 3f); // �ִϸ��̼� ����
            yield return new WaitForSeconds(2f); // 1�� �� �ٽ� ���� (���ϴ� ��� ���� ����)
        }
    }
}
