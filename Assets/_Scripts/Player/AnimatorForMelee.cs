using UnityEngine;

public class AnimatorForMelee : MonoBehaviour
{

    WeaponHolderMelee weaponHolderMelee;
    private void Start()
    {
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
