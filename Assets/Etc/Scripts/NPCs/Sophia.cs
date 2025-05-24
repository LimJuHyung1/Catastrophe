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
        this.transform.position = chair.position + new Vector3(0, 0, -0.4f); // 의자 위치로 이동
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
        animator.SetTrigger("Sit"); // 의자에 앉는 애니메이션 실행
    }



    public void PlayDialogueAnimation(int index) 
    {
        animator.SetTrigger("Talk");
        animator.SetInteger("Index", index);

        Debug.Log("소피아 애니메이션 " + index);
    }

    public void StartTalking()
    {
        cc.PlayBlendshapeAnimation("Talk", 3f); // 애니메이션 실행
    }

    public void StopTalking()
    {
        cc.ForceStopBlendshapeAnimation("Talk"); // 애니메이션 정지
    }






    public void Walk()
    {
        animator.SetBool("Walk", true); // 걷는 애니메이션 실행
    }

    public void Sit()
    {
        animator.SetTrigger("Sit"); // 앉는 애니메이션 실행
    }

    public void StartSitTalk()
    {
        animator.SetBool("SitTalk", true); // 앉아서 대화하는 애니메이션 실행
    }

    public void StopSitTalk()
    {
        animator.SetBool("SitTalk", false); // 앉아서 대화하는 애니메이션 실행
    }
}
