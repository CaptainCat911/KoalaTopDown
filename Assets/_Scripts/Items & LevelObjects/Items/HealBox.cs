using UnityEngine;

public class HealBox : ItemPickUp
{
    public int healValue;

    public void PickUpHeal()
    {
        GameManager.instance.player.Heal(healValue);
        Destroy(gameObject);
    }
}
