using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackKoala : MonoBehaviour
{
    public AmmoPackStore[] ammoWeapons;     // ������ �� ������ � ��������� (�������� � �������)

    public void BuyAmmo(int index)
    {
        if (index == 1)
        {
            ammoWeapons[index].allAmmo += 10;
        }
        if (index == 2)
        {
            ammoWeapons[index].allAmmo += 20;
        }
    }
}
