using UnityEngine;

public class Key : ItemPickUp
{
    [Header("Тип ключа")]
    public int keyTypeNumber;

    [Header("Аудио")]
    public GameObject audioPickUp;

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

        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // звук 
            Destroy(sound, 2f);
        }

        Destroy(gameObject);
    }
}
