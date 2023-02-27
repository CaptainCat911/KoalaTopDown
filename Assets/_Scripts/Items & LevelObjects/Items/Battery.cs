using UnityEngine;

public class Battery : ItemPickUp
{
    public void PickUpBattery()
    {
        GameManager.instance.battery++;
        Destroy(gameObject);
    }
}
