using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAI botAi;                                    // ������ �� ����
    Boss boss;                                      // ������ �� �����
    [HideInInspector] public Animator animator;     // ��������
    BotAIMeleeWeaponHolder botAIMeleeWeaponHolder;  // ���� ������

    private void Awake()
    {
        botAi = GetComponentInParent<BotAI>();
        boss = GetComponentInParent<Boss>();
        animator = GetComponent<Animator>();
        botAIMeleeWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }



    // ��� ��������� �����
    public void BossAttacking(int status)
    {
        if (status == 0)
            boss.attackingNow = false;
        if (status == 1)
            boss.attackingNow = true;
    }

    // �����
    public void CurrentWeaponAttack(int type)
    {
        if (type == 1)
            botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // ���� �����
        if (type == 2)
            botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // �����
        if (type == 3)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ���� �����
        if (type == 4)
            botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // ����� �����
        if (type == 5)
            botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // ���������� �����
        if (type == 7)
            botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // ���������� �����!
    }

    public void LaserWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.LaserOn(number);         // ����� �����
    }


    // ����� � �����
    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // ����� ������ (��� ���� ������)
    }

/*    public void EffectStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.EffectOn(number);      // ������ ������ (��� ������)
    }*/

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // ���������� �������� ������ �� �����
    }
    public void SetPivotSpeedKoef(float number)
    {
        botAi.pivotSpeedKoef = number;
    }


    // ������� ��� ����
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
    public void StaffLaseer()
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("LaserAttack");
    }




    // ��� �������� �� ������ �����
    public void GamemanagerMessage(int number)
    {
        GameManager.instance.StartEvent(number);
    }

    /*    public void ResetTriggerAttack()
        {
            animator.ResetTrigger("Hit");
        }*/
}
