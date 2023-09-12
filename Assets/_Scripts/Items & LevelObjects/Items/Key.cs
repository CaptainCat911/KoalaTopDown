using UnityEngine;

public class Key : ItemPickUp
{
    public string keyTextRu;
    public string keyTextEng;
    string keyText;



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
            
        }
        else
        {
            keyText = keyTextRu;
            
        }
    }

    public void PickUpKey()
    {

        GameManager.instance.CreateFloatingMessage("+ 1 " + keyText, Color.white, transform.position);

        GameManager.instance.keys[keyTypeNumber]++;

        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // звук 
            Destroy(sound, 2f);
        }

        Destroy(gameObject);
    }
}
