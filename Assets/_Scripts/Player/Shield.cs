using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Для флипа игрока
    [HideInInspector] public bool needFlip;             // нужен флип (для игрока и оружия)    
    [HideInInspector] public bool leftFlip;             // оружие слева
    [HideInInspector] public bool rightFlip = true;     // оружие справа

    // Таймер для цветов при уроне
    float timerForColor;        // сколько времени он будет красным
    bool red;                   // красный (-_-)
    public ParticleSystem effectParticles;  // префаб системы частиц (искры)

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Выбор цвета при получении урона и его сброс
        SetColorTimer();
    }


    public void TakeDamage()
    {
        ColorRed(0.15f);                         // делаем спрайт красным
        effectParticles.Play();
    }



    public void Flip()
    {
        // Флип эффекта (потом сделать по нормальному)
        if (GameManager.instance.player.leftFlip)                               // разворот налево
        {
            effectParticles.transform.localScale = new Vector3(effectParticles.transform.localScale.x, -1, effectParticles.transform.localScale.z);     // поворачиваем оружие через scale
        }
        if (GameManager.instance.player.rightFlip)
        {
            effectParticles.transform.localScale = new Vector3(effectParticles.transform.localScale.x, 1, effectParticles.transform.localScale.z);     // поворачиваем оружие через scale
        }
    }


    // Смена цветов при уроне
    void SetColorTimer()
    {
        if (timerForColor > 0)                  // таймер для отображения урона
            timerForColor -= Time.deltaTime;
        if (red && timerForColor <= 0)
            ColorWhite();
    }
    void ColorRed(float time)
    {
        timerForColor = time;
        spriteRenderer.color = Color.blue;
        red = true;
    }
    void ColorWhite()
    {
        spriteRenderer.color = Color.white;
        red = false;
    }
}
