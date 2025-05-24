using UnityEngine;
using System.Collections;


public class Sophia : NPCRole
{
    private Transform lookTarget;
    public Transform LookTarget
    {
        get { return lookTarget; }
        set { lookTarget = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        base.Start();
        currentCharacter = Character.Sophia;

        lookTarget = transform.GetChild(0).transform;
    }

    public void SitOnAChair(Transform chair)
    {
        this.transform.position = chair.position + new Vector3(0, 0, -0.4f); // ���� ��ġ�� �̵�
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
        animator.SetTrigger("Sit"); // ���ڿ� �ɴ� �ִϸ��̼� ����
    }



    public void PlayDialogueAnimation(int index) 
    {
        animator.SetTrigger("Talk");
        animator.SetInteger("Index", index);

        Debug.Log("���Ǿ� �ִϸ��̼� " + index);
    }

    public void StartTalking()
    {
        cc.PlayBlendshapeAnimation("Talk", 3f); // �ִϸ��̼� ����
    }

    public void StopTalking()
    {
        cc.ForceStopBlendshapeAnimation("Talk"); // �ִϸ��̼� ����
    }






    public void Walk()
    {
        animator.SetBool("Walk", true); // �ȴ� �ִϸ��̼� ����
    }

    public void Sit()
    {
        animator.SetTrigger("Sit"); // �ɴ� �ִϸ��̼� ����
    }

    public void StartSitTalk()
    {
        animator.SetBool("SitTalk", true); // �ɾƼ� ��ȭ�ϴ� �ִϸ��̼� ����
    }

    public void StopSitTalk()
    {
        animator.SetBool("SitTalk", false); // �ɾƼ� ��ȭ�ϴ� �ִϸ��̼� ����
    }
}
