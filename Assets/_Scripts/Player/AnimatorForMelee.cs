using UnityEngine;

public class AnimatorForMelee : MonoBehaviour
{
    Animator animator;
    WeaponHolderMelee weaponHolderMelee;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponHolderMelee = GetComponentInChildren<WeaponHolderMelee>();
    }


    public void CurrentWeaponAttack()
    {
        weaponHolderMelee.currentWeapon.MeleeAttack();
    }

    public void TrailStatus(int number)
    {
        weaponHolderMelee.currentWeapon.TrailOn(number);
    }
}
