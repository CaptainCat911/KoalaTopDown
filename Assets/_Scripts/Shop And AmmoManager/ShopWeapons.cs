using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopWeapons : MonoBehaviour
{
    public TextMeshPro[] m_textsRange;
    public TextMeshPro[] m_textsMelee;
    public TextMeshPro[] m_textsBomb;
    AmmoPackKoala ammoPack;          // ссылка на аммопак (тут и оружие и бомбы и покупка этого всего)

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoManager;        
    }


    void Start()
    {
        // для ренж оружия
        int i;
        for (i = 1; i < m_textsRange.Length; i++)
        {
            m_textsRange[i].text = ammoPack.ammoWeapons[i].goldPriseWeapon.ToString();
        }

        // для мили оружия
        int j;
        for (j = 1; j < m_textsMelee.Length; j++)
        {
            m_textsMelee[j].text = ammoPack.ammoMeleeWeapons[j].goldPriseWeapon.ToString();
        }

        // для бомб
        int k;
        for (k = 1; k < m_textsBomb.Length; k++)
        {
            m_textsBomb[k].text = ammoPack.ammoBombs[k].goldPriseWeapon.ToString();
        }
    }
}
