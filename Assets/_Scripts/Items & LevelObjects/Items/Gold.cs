using UnityEngine;

public class Gold : ItemPickUp
{
    public int goldValue;
    Player player;

    public override void Awake()
    {
        base.Awake();
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (player.withGoldMagnet)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, player.speedMagnet * Time.deltaTime);   // ���������� ������
    }

    public void PickUpGold()
    {
        GameManager.instance.gold += goldValue;                                          // + ������
        GameManager.instance.CreateFloatingMessage("+ " + goldValue, Color.yellow,
           GameManager.instance.player.transform.position);                             // ������ ���������
        Destroy(gameObject);
    }
}
