using UnityEngine;
using TMPro;

public class ShopWeaponControl : MonoBehaviour
{

    public TextMeshPro textWeaponName;  // название оружия
    public string weaponName;
    public string weaponNameEng;


    public TextMeshPro textGold;        // стоимость
    public GameObject eventBuy;
    public GameObject sellText;         // текст продажи
    public GameObject sellTextEng;      // текст продажи


    private void Start()
    {
        if (LanguageManager.instance.eng)
        {
            textWeaponName.text = weaponNameEng;
        }
        else
        {
            textWeaponName.text = weaponName;
        }
    }

    public void WeaponBuyed()
    {
        eventBuy.SetActive(false);

        if (LanguageManager.instance.eng)
        {
            sellTextEng.SetActive(true);
        }
        else
        {
            sellText.SetActive(true);
        }        
    }
}
