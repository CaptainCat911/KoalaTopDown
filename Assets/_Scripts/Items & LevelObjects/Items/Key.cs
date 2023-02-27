using UnityEngine;

public class Key : ItemPickUp
{
    public void PickUpKey()
    {
        GameManager.instance.keys++;
        Destroy(gameObject);
    }
}
