using UnityEngine;

public class Gold : ItemPickUp
{
    public int goldValue;

    public void PickUpGold()
    {
        GameManager.instance.gold += goldValue;                                          // + ������
        GameManager.instance.CreateFloatingMessage("+ " + goldValue, Color.yellow,
           GameManager.instance.player.transform.position);                             // ������ ���������
        Destroy(gameObject);
    }
}
