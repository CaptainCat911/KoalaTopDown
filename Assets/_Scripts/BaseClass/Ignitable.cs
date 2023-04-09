using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignitable : MonoBehaviour
{
    Fighter figter;
    public ParticleSystem flames;   // Префаб системы частиц огня

    // Поиск цели
    public bool canBurn = true;     // Может ли объект гореть
    
    float lastBurnDamaged;          // время последнего урона
    float cooldownBurn;             // перезардяка
    int burnDamage;                 // Урон от горения
    float burnDuration;             // Длительность горения
    float lastBurn;                 // время последнего горения
    private bool isBurning = false; // Горит ли объект



    private void Awake()
    {
        figter = GetComponent<Fighter>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Ignite(10, 0.5f, 2);
        }

        if (!isBurning)
            return; 

        // Наносим урон
        if (Time.time - lastBurnDamaged > cooldownBurn)      // если кд готово
        {
            lastBurnDamaged = Time.time;
            figter.TakeDamage(burnDamage, Vector2.zero, 0f);
        }

        if (Time.time - lastBurn >= burnDuration)
        {
            isBurning = false;
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
        flames.Play();
    }
}
