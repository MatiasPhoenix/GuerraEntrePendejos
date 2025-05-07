using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    private void Awake() => Instance = this;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("AnimationManager Instance riassegnato dopo cambio scena!");
        }
    }
    private BaseUnit _heroSelected;

    public void PlayWalkAnimation(BaseUnit unit, bool isRunning)
    {
        Animator anim = FindAnimatorComponent(unit);
        if (anim != null) anim.SetBool("1_Move", isRunning);
    }
    public void PlayAttackAnimation(BaseUnit unit)
    {
        Animator anim = FindAnimatorComponent(unit);
        if (anim != null) anim.SetTrigger("2_Attack");
    }
    public void TakeDamageAnimation(BaseUnit unit)
    {
        Animator anim = FindAnimatorComponent(unit);
        if (anim != null) anim.SetTrigger("3_Damaged");
    }
    public void DeadAnimation(BaseUnit unit)
    {
        Animator anim = FindAnimatorComponent(unit);
        if (anim != null) anim.SetTrigger("4_Death");
    }
    public void CharacterIsDead(BaseUnit unit)
    {
        Animator anim = FindAnimatorComponent(unit);
        if (anim != null) anim.SetBool("isDeath", true);
    }

    Animator FindAnimatorComponent(BaseUnit unit)
    {
        _heroSelected = unit;

        Transform unitRoot = unit.transform.GetChild(0).Find("UnitRoot");
        if (unitRoot == null) return null;

        Animator anim = unitRoot.GetComponent<Animator>();
        return anim;
    }

    public bool CheckAnimation()
    {
        Animator anim = FindAnimatorComponent(_heroSelected);
        return anim.GetBool("1_Move");
    }
}
