using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAI botAi;
    public Animator animator;
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;

    private void Awake()
    {
        botAi = GetComponentInParent<BotAI>();
        //animator = GetComponent<Animator>();
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

    public void SpawnWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // спаун атака
    }

    public void ExplousionWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // взрыв
    }




    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружия
    }

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // отключение вращения пивота за целью
    }

    public void StaffFireBall()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("RangeAttack");
    }

    /*    public void ResetTriggerAttack()
        {
            animator.ResetTrigger("Hit");
        }*/
}
