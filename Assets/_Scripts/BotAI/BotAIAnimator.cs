using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAI botAi;                                    // ссылка на бота
    NPC npc;                                        // ссылка на босса
    [HideInInspector] public Animator animator;     // аниматор
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;  // мили холдер

    private void Awake()
    {
        botAi = GetComponentInParent<BotAI>();
        npc = GetComponentInParent<NPC>();
        animator = GetComponent<Animator>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }



    // Для аниматора босса
    public void BossAttacking(int status)
    {
        if (status == 0)
            npc.attackingNow = false;
        if (status == 1)
            npc.attackingNow = true;
    }

    public void BotAttacking(int status)
    {
        if (status == 0)
            botAi.nowAttacking = false;
        if (status == 1)
            botAi.nowAttacking = true;
    }

    // Атаки
    public void CurrentWeaponAttack(int type)
    {
        if (type == 1)
            botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // мили атака
        if (type == 2)
            botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // взрыв
        if (type == 3)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttackBig();      // ренж атака (сильная)
        if (type == 4)
            botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // спаун атака
        if (type == 5)
            botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // мультиренж атака
        if (type == 7)
            botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // возвращает время!
        if (type == 8)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ренж атака (слабая)
        if (type == 9)
            botAIMeleeWeaponHolder.currentWeapon.Teleport();            // телепорт
    }

    public void LaserWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.LaserOn(number);       // лазер атака
    }
    
    public void GravityWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.GravityOn(number);     // гравитация атака
    }


    public void MakePlayerImmortal()
    {
        GameManager.instance.player.isImmortal = true;
    }


    // Звук
    public void SoundAttack(string type)
    {
        if (type == "StartMeleeAttack")
        {
            botAIMeleeWeaponHolder.currentWeapon.audioSource.clip = botAIMeleeWeaponHolder.currentWeapon.audioWeapon.hitStart;
            botAIMeleeWeaponHolder.currentWeapon.audioSource.Play();
        }
        if (type == "StartExplousionAttack")    // 135 frame + explousion
        {
            botAIMeleeWeaponHolder.currentWeapon.audioSource.clip = botAIMeleeWeaponHolder.currentWeapon.audioWeapon.hitExplousion;
            botAIMeleeWeaponHolder.currentWeapon.audioSource.Play();
        }
        if (type == "StartLasetAttack")         // 150 frame
        {
            botAIMeleeWeaponHolder.currentWeapon.audioSource.clip = botAIMeleeWeaponHolder.currentWeapon.audioWeapon.hitLaser;
            botAIMeleeWeaponHolder.currentWeapon.audioSource.Play();
        }
        if (type == "StartTimeReverce")         // 420 frame
        {
            botAIMeleeWeaponHolder.currentWeapon.audioSource.clip = botAIMeleeWeaponHolder.currentWeapon.audioWeapon.hitTimeReverce;
            botAIMeleeWeaponHolder.currentWeapon.audioSource.Play();
        }
    }





    // Треил и пивот
    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // треил оружия (для мили оружия)
    }

/*    public void EffectStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.EffectOn(number);      // эффект оружия (для лазера)
    }*/

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // отключение вращения пивота за целью
    }
    public void SetPivotSpeedKoef(float number)
    {
        botAi.pivotSpeedKoef = number;
    }


    // Эффекты для атак
    public void StaffStartEffect(int type)
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetFloat("Type", type);
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("StartEffect");
    } 




    // Для перехода на другую сцену
    public void GamemanagerMessage(int number)
    {
        ArenaManager.instance.StartEvent(number);
    }

    /*    public void ResetTriggerAttack()
        {
            animator.ResetTrigger("Hit");
        }*/
}
