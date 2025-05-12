using UnityEngine;

public class Nurse : NPCRole
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
        string trigger = state.ToString();
        animator.SetTrigger(trigger);
        StartTalking();
    }




    public void StartTalking()
    {
        cc.PlayBlendshapeAnimation("Talk", 3f); // �ִϸ��̼� ����
        Debug.Log("Start Talking");
    }

    public void StopTalking()
    {
        cc.ForceStopBlendshapeAnimation("Talk"); // �ִϸ��̼� ����
    }
}
