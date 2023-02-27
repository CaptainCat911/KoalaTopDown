using UnityEngine;

public class HealBox : ItemPickUp
{
    public void PickUpHeal()
    {
        GameManager.instance.player.Heal(100);
        Destroy(gameObject);
    }
}
