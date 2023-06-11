using UnityEngine;
using TMPro;

public class ShopWeaponControl : MonoBehaviour
{

    public TextMeshPro textGold;
    public GameObject eventBuy;
    public GameObject sellText;


    public void WeaponBuyed()
    {
        eventBuy.SetActive(false);
        sellText.SetActive(true);
    }
}
