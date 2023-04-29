using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // ������ ��� ������ ��� �����
    float timerForColor;        // ������� ������� �� ����� �������
    bool red;                   // ������� (-_-)

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();
    }


    public void TakeDamage()
    {
        ColorRed(0.15f);                         // ������ ������ �������
    }


    // ����� ������ ��� �����
    void SetColorTimer()
    {
        if (timerForColor > 0)                  // ������ ��� ����������� �����
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
