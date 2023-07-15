using UnityEngine;

public class HealBox : ItemPickUp
{
    public int healValue;

    [Header("�����")]
    public GameObject audioPickUp;

    public void PickUpHeal()
    {
        GameManager.instance.player.Heal(healValue);
        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // ���� 
            Destroy(sound, 1f);
        }
        Destroy(gameObject);
    }
}
