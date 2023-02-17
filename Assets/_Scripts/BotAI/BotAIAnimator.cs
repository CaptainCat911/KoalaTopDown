using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAI botAi;
    [HideInInspector] public Animator animator;
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;

    private void Awake()
    {
        botAi = GetComponentInParent<BotAI>();
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
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружи€
    }

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // отключение вращени€ пивота за целью
    }


    // Ёффекты дл€ посоха
    public void StaffFireBall()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("RangeAttack");
    }
    public void StaffSpawn()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("SpawnAttack");
    }    
    public void StaffExplousion()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("ExplousionAttack");
    }

    /*    public void ResetTriggerAttack()
        {
            animator.ResetTrigger("Hit");
        }*/
}
