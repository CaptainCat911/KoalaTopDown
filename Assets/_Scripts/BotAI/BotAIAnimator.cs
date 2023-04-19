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



    // ƒл€ аниматора босса
    public void BossAttacking(int status)
    {
        if (status == 0)
            boss.attackingNow = false;
        if (status == 1)
            boss.attackingNow = true;
    }

    // јтаки
    public void CurrentWeaponAttack(int type)
    {
        if (type == 1)
            botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // мили атака
        if (type == 2)
            botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // взрыв
        if (type == 3)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttackBig();      // ренж атака (сильна€)
        if (type == 4)
            botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // спаун атака
        if (type == 5)
            botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // мультиренж атака
        if (type == 7)
            botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // возвращает врем€!
        if (type == 8)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ренж атака (слаба€)
        if (type == 9)
            botAIMeleeWeaponHolder.currentWeapon.Teleport();            // телепорт
    }

    public void LaserWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.LaserOn(number);       // лазер атака
    }
    
    public void GravityWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.GravityOn(number);     // гравитаци€ атака
    }


    // “реил и пивот
    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружи€ (дл€ мили оружи€)
    }

/*    public void EffectStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.EffectOn(number);      // эффект оружи€ (дл€ лазера)
    }*/

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // отключение вращени€ пивота за целью
    }
    public void SetPivotSpeedKoef(float number)
    {
        botAi.pivotSpeedKoef = number;
    }


    // Ёффекты дл€ атак
    public void StaffStartEffect(int type)
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetFloat("Type", type);
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("StartEffect");
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
