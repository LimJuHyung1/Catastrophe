using UnityEngine;
using System.Collections;


public class AdministrativeOfficer : NPCRole
{
    private Transform lookTarget;
    public Transform LookTarget
    {
        get { return lookTarget; }
        set { lookTarget = value; }
    }

    public HospitalManager hospitalManager;


    protected void Start()
    {
        base.Start();
        currentCharacter = Character.Sophia;

        lookTarget = transform.GetChild(0).transform;        
    }

    public void PlayAnimation(int state)
    {
        StartTalking();

        switch (state){
            case 0:
                PlayRestrainAnimation();
                break;
            case 1:
                PlayWorryAnimation();
                break;
            case 2:
                PlayTalkAnimation();
                break;

            default:
                Debug.LogError(this.name + " : Parameter error!");
                break;
        }
    }

    private void PlayRestrainAnimation()
    {
        animator.SetTrigger("Restraining");
    }

    private void PlayWorryAnimation()
    {
        animator.SetTrigger("Worrying");
    }

    private void PlayTalkAnimation()
    {
        animator.SetTrigger("Talking");
    }






    public void StartTalking()
    {
        cc.PlayBlendshapeAnimation("Talk", 3f); // 애니메이션 실행
    }

    public void StopTalking()
    {
        cc.ForceStopBlendshapeAnimation("Talk"); // 애니메이션 정지
    }



}
