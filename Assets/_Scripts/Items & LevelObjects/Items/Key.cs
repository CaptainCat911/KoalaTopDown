using UnityEngine;

public class Key : ItemPickUp
{
    public void PickUpKey()
    {
        GameManager.instance.CreateFloatingMessage("+ 1 ����", Color.yellow, transform.position);
        GameManager.instance.keys++;
        Destroy(gameObject);
    }
}
