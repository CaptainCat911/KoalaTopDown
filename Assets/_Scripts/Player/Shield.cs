using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Таймер для цветов при уроне
    float timerForColor;        // сколько времени он будет красным
    bool red;                   // красный (-_-)

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Выбор цвета при получении урона и его сброс
        SetColorTimer();
    }


    public void TakeDamage()
    {
        ColorRed(0.15f);                         // делаем спрайт красным
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
