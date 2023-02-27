using UnityEngine;

public class Gold : ItemPickUp
{
    public void PickUpGold(int goldValue)
    {
        GameManager.instance.gold += goldValue;
        Destroy(gameObject);
    }
}
