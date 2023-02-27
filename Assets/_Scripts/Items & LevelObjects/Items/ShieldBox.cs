using UnityEngine;

public class ShieldBox : ItemPickUp
{
    public void PickUpShieldHeal()
    {
        GameManager.instance.player.shield.HealShield(25);
        Destroy(gameObject);
    }
}
