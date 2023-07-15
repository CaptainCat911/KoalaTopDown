using UnityEngine;

public class Gold : ItemPickUp
{
    public int goldValue;
    public bool noMagnet;
    Player player;
    bool nearPlayer;

    [Header("Аудио")]
    public GameObject audioPickUp;

    public override void Awake()
    {
        base.Awake();
        player = GameManager.instance.player;
    }

    void FixedUpdate()
    {
        if (noMagnet)
            return;
        if (player.withGoldMagnet && nearPlayer)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, player.speedMagnet * Time.deltaTime);   // перемещаем игрока
    }

    public void MagnerOn()
    {
        nearPlayer = true;
    }

    public void PickUpGold()
    {
        GameManager.instance.gold += goldValue;                                          // + золото
        GameManager.instance.CreateFloatingMessage("+ " + goldValue, Color.yellow,
           GameManager.instance.player.transform.position);                             // создаём сообщение

        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // звук взрыва
            Destroy(sound, 1f);
        }
        Destroy(gameObject);
    }
}
