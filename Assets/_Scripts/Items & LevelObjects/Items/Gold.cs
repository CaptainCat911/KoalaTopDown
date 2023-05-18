using UnityEngine;

public class Gold : ItemPickUp
{
    public int goldValue;
    Player player;
    bool nearPlayer;

    public override void Awake()
    {
        base.Awake();
        player = GameManager.instance.player;
    }

    void FixedUpdate()
    {
        if (player.withGoldMagnet && nearPlayer)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, player.speedMagnet * Time.deltaTime);   // ���������� ������
    }

    public void MagnerOn()
    {
        nearPlayer = true;
    }

    public void PickUpGold()
    {
        GameManager.instance.gold += goldValue;                                          // + ������
        GameManager.instance.CreateFloatingMessage("+ " + goldValue, Color.yellow,
           GameManager.instance.player.transform.position);                             // ������ ���������
        Destroy(gameObject);
    }
}
