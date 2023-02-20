using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider slider;                  // слайдер
    RectTransform rectTransform;    // область
    public Gradient gradient;       // цвет (градиент)
    public Image fill;              // заливка
    public float sizeHp;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;               // значение всей шкалы
        slider.value = health;                  // значение заливки
        rectTransform.SetWidth(sizeHp);         // установить размер области 
        fill.color = gradient.Evaluate(1f);     // цвет заливки
    }


    public void SetHealth(int health)
    {
        slider.value = health;                                      // значение заливки
        fill.color = gradient.Evaluate(slider.normalizedValue);     // цвет заливки
    }
}