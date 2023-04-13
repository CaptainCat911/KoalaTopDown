using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour
{
    Fighter figter;
    BotAI botAI;
    public ParticleSystem flames;   // Префаб системы частиц огня

    // Поиск цели
    public bool canBurn = true;     // Может ли объект гореть
    public float slow = 0.2f;         // замедление при получении урона    
    float lastBurnDamaged;          // время последнего урона
    float cooldownBurn;             // перезардяка
    int burnDamage;                 // Урон от горения
    float burnDuration;             // Длительность горения
    float lastBurn;                 // время последнего горения
    bool isBurning = false;         // Горит ли объект



    private void Awake()
    {
        figter = GetComponent<Fighter>();
        botAI = GetComponent<BotAI>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {

        if (!isBurning)
            return; 

        // Наносим урон
        if (Time.time - lastBurnDamaged > cooldownBurn)         // если кд готово
        {
            lastBurnDamaged = Time.time;                        // время последнего урона огнём
            figter.TakeDamage(burnDamage, Vector2.zero, 0f);    // наносим урон
            if (botAI)                                          // если это бот
            {
                botAI.SlowSpeed(slow);                          // замедляем
            }
        }

        if (Time.time - lastBurn >= burnDuration)               // если горение закончилось
        {
            isBurning = false;
            if (flames)
                flames.Stop();
        }
    }

    private void FixedUpdate()
    {
        
    }

    public void Ignite(int damage, float cooldown, float duration)
    {
        if (!canBurn)
            return;

        isBurning = true;
        burnDamage = damage;
        cooldownBurn = cooldown;
        burnDuration = duration;
        lastBurn = Time.time;
        if(flames)
            flames.Play();
    }
}
