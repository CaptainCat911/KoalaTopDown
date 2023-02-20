using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Slider slider;                  // �������
    RectTransform rectTransform;    // �������
    public Gradient gradient;       // ���� (��������)
    public Image fill;              // �������
    public float sizeHp;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;               // �������� ���� �����
        slider.value = health;                  // �������� �������
        rectTransform.SetWidth(sizeHp);         // ���������� ������ ������� 
        fill.color = gradient.Evaluate(1f);     // ���� �������
    }


    public void SetHealth(int health)
    {
        slider.value = health;                                      // �������� �������
        fill.color = gradient.Evaluate(slider.normalizedValue);     // ���� �������
    }
}