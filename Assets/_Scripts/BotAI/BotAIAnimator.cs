using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    //BotAI botAi;
    Animator animator;
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    private void Start()
    {
        //botAi = GetComponentInParent<BotAI>();
        animator = GetComponent<Animator>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }
    public void CurrentWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();
    }

    public void RangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.RangeAttack();
    }


    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);
    }

    public void ResetTriggerAttack()
    {
        animator.ResetTrigger("Hit");
    }
}
