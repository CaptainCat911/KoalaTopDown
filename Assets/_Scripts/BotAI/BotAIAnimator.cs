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
            botAIMeleeWeaponHolder.currentWeapon.RangeAttackBig();      // ���� ����� (�������)
        if (type == 4)
            botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // ����� �����
        if (type == 5)
            botAIMeleeWeaponHolder.currentWeapon.MultiRangeAttack();    // ���������� �����
        if (type == 7)
            botAIMeleeWeaponHolder.currentWeapon.TimeReverceAttack();   // ���������� �����!
        if (type == 8)
            botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ���� ����� (������)
        if (type == 9)
            botAIMeleeWeaponHolder.currentWeapon.Teleport();            // ��������
    }

    public void LaserWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.LaserOn(number);       // ����� �����
    }
    
    public void GravityWeaponAttack(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.GravityOn(number);     // ���������� �����
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
    public void StaffStartEffect(int type)
    {
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetFloat("Type", type);
        botAi.botAIMeleeWeaponHolder.currentWeapon.GetComponent<Animator>().SetTrigger("StartEffect");
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
