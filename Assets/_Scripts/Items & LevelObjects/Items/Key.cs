using UnityEngine;

public class Key : ItemPickUp
{
    public int keyTypeNumber;
    public void PickUpKey()
    {
        if (keyTypeNumber == 0)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ����", Color.white, transform.position);
        }
        if (keyTypeNumber == 1)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ���������� ����", Color.white, transform.position);
        }
        if (keyTypeNumber == 2)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ������ ����", Color.cyan, transform.position);
        }
        GameManager.instance.keys[keyTypeNumber]++;
        Destroy(gameObject);
    }
}
