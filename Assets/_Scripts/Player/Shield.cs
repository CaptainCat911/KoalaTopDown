using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originColor;
    AudioSource audioSource;

    // Для флипа игрока
    [HideInInspector] public bool needFlip;             // нужен флип (для игрока и оружия)    
    [HideInInspector] public bool leftFlip;             // оружие слева
    [HideInInspector] public bool rightFlip = true;     // оружие справа

    // Таймер для цветов при уроне
    float timerForColor;                        // сколько времени он будет красным
    bool red;                                   // красный (-_-)
    public ParticleSystem effectParticles;      // префаб системы частиц (искры)

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
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
        
        float audioPitch = Random.Range(0.8f, 1.2f);                            // рандомный питч
        audioSource.pitch = audioPitch;                                         // устанавливаем питч        
        audioSource.Play();                                                     // воспроизводим        
    }



    public void Flip()
    {
        // Флип эффекта (потом сделать по нормальному)
        if (GameManager.instance.player.leftFlip)                               // разворот налево
        {
            //Quaternion qua1 = new Quaternion.Euler(effectParticles.transform.localRotation.x, -60, effectParticles.transform.localRotation.z);     // поворачиваем оружие через scale
            //effectParticles.transform.localRotation = qua1;

            //effectParticles.transform.localRotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 15);

            effectParticles.transform.localRotation = Quaternion.Lerp(effectParticles.transform.localRotation, Quaternion.Euler(effectParticles.transform.localRotation.x, effectParticles.transform.localRotation.y, 60), Time.fixedDeltaTime * 100);
        }
        if (GameManager.instance.player.rightFlip)
        {
            effectParticles.transform.localRotation = Quaternion.Lerp(effectParticles.transform.localRotation, Quaternion.Euler(effectParticles.transform.localRotation.x, effectParticles.transform.localRotation.y, -60), Time.fixedDeltaTime * 100);
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
        spriteRenderer.color = Color.white;
        red = true;
    }
    void ColorWhite()
    {
        spriteRenderer.color = originColor;
        red = false;
    }
}
