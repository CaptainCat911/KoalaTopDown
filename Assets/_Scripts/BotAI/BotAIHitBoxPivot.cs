using UnityEngine;

/// <summary>
/// Пивот для мили оружия, также поворачивается для поворота оружия и флипа
/// </summary>

public class BotAIHitBoxPivot : MonoBehaviour
{
    BotAI botAI;
    //public WeaponHolder weaponHolder;

    void Awake()
    {
        botAI = GetComponentInParent<BotAI>();
        //weaponHolder = GetComponentInParent<WeaponHolder>();
    }

/*    void Update()
    {
        Quaternion qua1 = Quaternion.Euler(0, 0, weaponHolder.aimAngle);                                // создаем этот угол в Quaternion
        transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);       // делаем Lerp между weaponHoder и нашим углом

    }*/

    public void Flip()
    {
        if (botAI.flipLeft)                               // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.y, -1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        else if (botAI.flipRight)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        else
            return;
    }
}
