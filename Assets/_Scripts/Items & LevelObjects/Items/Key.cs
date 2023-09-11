using UnityEngine;

public class Key : ItemPickUp
{
    public string keyTextRu;
    public string keyTextEng;
    string keyText;

    public string magicKeyTextRu;
    public string magicKeyEng;
    string magicKeyText;

    [Header("Тип ключа")]
    public int keyTypeNumber;

    [Header("Аудио")]
    public GameObject audioPickUp;

    public override void Start()
    {
        base.Start();

        if (LanguageManager.instance.eng)
        {
            keyText = keyTextEng;
            magicKeyText = magicKeyEng;
        }
        else
        {
            keyText = keyTextRu;
            magicKeyText = magicKeyTextRu;
        }
    }

    public void PickUpKey()
    {
        if (keyTypeNumber == 0)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 " + keyText, Color.white, transform.position);
        }
        if (keyTypeNumber == 1)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 " + magicKeyText, Color.white, transform.position);
        }
/*        if (keyTypeNumber == 2)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 Темный ключ", Color.cyan, transform.position);
        }*/
        GameManager.instance.keys[keyTypeNumber]++;

        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // звук 
            Destroy(sound, 2f);
        }

        Destroy(gameObject);
    }
}
