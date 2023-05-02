using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // ��� ����� ������
    [HideInInspector] public bool needFlip;             // ����� ���� (��� ������ � ������)    
    [HideInInspector] public bool leftFlip;             // ������ �����
    [HideInInspector] public bool rightFlip = true;     // ������ ������

    // ������ ��� ������ ��� �����
    float timerForColor;        // ������� ������� �� ����� �������
    bool red;                   // ������� (-_-)
    public ParticleSystem effectParticles;  // ������ ������� ������ (�����)

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // ����� ����� ��� ��������� ����� � ��� �����
        SetColorTimer();
    }


    public void TakeDamage()
    {
        ColorRed(0.15f);                         // ������ ������ �������
        effectParticles.Play();
    }



    public void Flip()
    {
        // ���� ������� (����� ������� �� �����������)
        if (GameManager.instance.player.leftFlip)                               // �������� ������
        {
            effectParticles.transform.localScale = new Vector3(effectParticles.transform.localScale.x, -1, effectParticles.transform.localScale.z);     // ������������ ������ ����� scale
        }
        if (GameManager.instance.player.rightFlip)
        {
            effectParticles.transform.localScale = new Vector3(effectParticles.transform.localScale.x, 1, effectParticles.transform.localScale.z);     // ������������ ������ ����� scale
        }
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
