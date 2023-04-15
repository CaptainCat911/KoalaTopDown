using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAI botAi;                                    // ссылка на бота
    Boss boss;                                      // ссылка на босса
    [HideInInspector] public Animator animator;     // аниматор
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;  // мили холдер

    private void Awake()
    {
        botAi = GetComponentInParent<BotAI>();
        boss = GetComponentInParent<Boss>();
        animator = GetComponent<Animator>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }


    // јтаки
    public void CurrentWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // мили атака
    }

    public void RangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ренж атака
    }

    public void MultiRangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // мультиренж атака
    }

    public void ExplousionWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // взрыв
    }

    public void SpawnWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // спаун атака
    }

    public void TimeReverceWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // возвращаает врем€!
    }




    // “реил и пивот
    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружи€
    }

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // отключение вращени€ пивота за целью
    }
    public void SetPivotSpeedKoef(float number)
    {
        botAi.pivotSpeedKoef = number;
    }


    // Ёффекты дл€ атак
    public void StaffFireBall()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("RangeAttack");
    }
    public void StaffMultiFireBall()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("MultiRangeAttack");
    }
    public void StaffSpawn()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("SpawnAttack");
    }    
    public void StaffExplousion()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("ExplousionAttack");
    }    
    public void StaffTimeReverce()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("TimeReverceAttack");
    }


    // ƒл€ аниматора босса
    public void BossAttacking(int status)
    {
        if(status == 0)
            boss.attackingNow = false;
        if (status == 1)
            boss.attackingNow = true;
    }


    // ƒл€ перехода на другую сцену
    public void GamemanagerMessage(int number)
    {
        GameManager.instance.StartEvent(number);
    }

    /*    public void ResetTriggerAttack()
        {
            animator.ResetTrigger("Hit");
        }*/
}
