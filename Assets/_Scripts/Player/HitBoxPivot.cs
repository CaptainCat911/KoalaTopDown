using UnityEngine;

/// <summary>
/// Пивот для мили оружия, также поворачивается для поворота оружия и флипа
/// </summary>

public class HitBoxPivot : MonoBehaviour
{
    Player player;
    public WeaponHolder weaponHolder;

    public Shield shield;       // (временно флип эффекта щита здесь)

    void Start()
    {
        player = GameManager.instance.player;
        //weaponHolder = GetComponentInParent<WeaponHolder>();
    }
   
    void Update()
    {
        if (!GameManager.instance.isPlayerEnactive && player.noAim)
        {
            Quaternion qua1 = Quaternion.Euler(0, 0, weaponHolder.aimAngle);                                // создаем этот угол в Quaternion
            transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);       // делаем Lerp между weaponHoder и нашим углом
        }        
        else
        {
            
            if (player.rightFlip)
            {                
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            }
            if (player.leftFlip)
            {                
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 180), Time.fixedDeltaTime * 15);   // делаем Lerp между weaponHoder и нашим углом
            }
        }
    }

    public void Flip()
    {
        shield.Flip();

        if (player.leftFlip)                               // разворот налево
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);     // поворачиваем оружие через scale
        }
        if (player.rightFlip)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);     // поворачиваем оружие через scale
        }        
    }
}
