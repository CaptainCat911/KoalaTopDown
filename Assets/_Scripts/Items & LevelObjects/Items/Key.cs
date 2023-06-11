using UnityEngine;

public class Key : ItemPickUp
{
    public int keyTypeNumber;
    public void PickUpKey()
    {
        if (keyTypeNumber == 0)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 Ключ", Color.white, transform.position);
        }
        if (keyTypeNumber == 1)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 Магический ключ", Color.white, transform.position);
        }
        if (keyTypeNumber == 2)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 Темный ключ", Color.cyan, transform.position);
        }
        GameManager.instance.keys[keyTypeNumber]++;
        Destroy(gameObject);
    }
}
