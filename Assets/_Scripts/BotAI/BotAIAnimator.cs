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


    // �����
    public void CurrentWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // ���� �����
    }

    public void RangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ���� �����
    }

    public void MultiRangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // ���������� �����
    }

    public void ExplousionWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // �����
    }

    public void SpawnWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // ����� �����
    }

    public void TimeReverceWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // ����������� �����!
    }




    // ����� � �����
    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // ����� ������
    }

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


    // ��� ��������� �����
    public void BossAttacking(int status)
    {
        if(status == 0)
            boss.attackingNow = false;
        if (status == 1)
            boss.attackingNow = true;
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
