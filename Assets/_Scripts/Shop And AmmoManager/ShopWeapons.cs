using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopWeapons : MonoBehaviour
{
    public TextMeshPro[] m_texts;
    AmmoPackKoala ammoPack;          // ������ �� ������� (��� � ������ � ����� � ������� ����� �����)

    private void Awake()
    {
        ammoPack = GameManager.instance.ammoPack;        
    }


    void Start()
    {
        int i;
        for (i = 1; i < m_texts.Length; i++)
        {
            m_texts[i].text = ammoPack.ammoWeapons[i].goldPriseWeapon.ToString();
        }
        
    }
}
