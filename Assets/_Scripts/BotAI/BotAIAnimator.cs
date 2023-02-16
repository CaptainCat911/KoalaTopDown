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
        botAIMeleeWeaponHolder.currentWeapon.MeleeAttack();         // ���� �����
    }

    public void RangeWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.RangeAttack();         // ���� �����
    }

    public void SpawnWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.SpawnAttack();         // ����� �����
    }

    public void ExplousionWeaponAttack()
    {
        botAIMeleeWeaponHolder.currentWeapon.ExplousionAttack();    // �����
    }




    public void TrailStatus(int number)
    {
        botAIMeleeWeaponHolder.currentWeapon.TrailOn(number);       // ����� ������
    }

    public void PivotStatus(int number)
    {
        botAi.PivotZero(number);            // ���������� �������� ������ �� �����
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
