using UnityEngine;

public class HealBox : ItemPickUp
{
    public int healValue;

    [Header("Аудио")]
    public GameObject audioPickUp;

    public void PickUpHeal()
    {
        GameManager.instance.player.Heal(healValue);
        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // звук 
            Destroy(sound, 1f);
        }
        Destroy(gameObject);
    }
}
