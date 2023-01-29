using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    //BotAI botAi;
    public Animator animator;
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;
    private void Start()
    {
        //botAi = GetComponentInParent<BotAI>();
        animator = GetComponent<Animator>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }
    public void CurrentWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // мили атака
    }

    public void RangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ренж атака
    }


    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружия
    }

    public void ResetTriggerAttack()
    {
        animator.ResetTrigger("Hit");
    }
}
