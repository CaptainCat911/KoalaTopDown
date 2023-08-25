using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originColor;
    AudioSource audioSource;

    // ��� ����� ������
    [HideInInspector] public bool needFlip;             // ����� ���� (��� ������ � ������)    
    [HideInInspector] public bool leftFlip;             // ������ �����
    [HideInInspector] public bool rightFlip = true;     // ������ ������

    // ������ ��� ������ ��� �����
    float timerForColor;                        // ������� ������� �� ����� �������
    bool red;                                   // ������� (-_-)
    public ParticleSystem effectParticles;      // ������ ������� ������ (�����)

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
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
        
        float audioPitch = Random.Range(0.8f, 1.2f);                            // ��������� ����
        audioSource.pitch = audioPitch;                                         // ������������� ����        
        audioSource.Play();                                                     // �������������        
    }



    public void Flip()
    {
        // ���� ������� (����� ������� �� �����������)
        if (GameManager.instance.player.leftFlip)                               // �������� ������
        {
            //Quaternion qua1 = new Quaternion.Euler(effectParticles.transform.localRotation.x, -60, effectParticles.transform.localRotation.z);     // ������������ ������ ����� scale
            //effectParticles.transform.localRotation = qua1;

            //effectParticles.transform.localRotation = Quaternion.Lerp(pivot.transform.rotation, qua1, Time.fixedDeltaTime * 15);

            effectParticles.transform.localRotation = Quaternion.Lerp(effectParticles.transform.localRotation, Quaternion.Euler(effectParticles.transform.localRotation.x, effectParticles.transform.localRotation.y, 60), Time.fixedDeltaTime * 100);
        }
        if (GameManager.instance.player.rightFlip)
        {
            effectParticles.transform.localRotation = Quaternion.Lerp(effectParticles.transform.localRotation, Quaternion.Euler(effectParticles.transform.localRotation.x, effectParticles.transform.localRotation.y, -60), Time.fixedDeltaTime * 100);
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
        spriteRenderer.color = Color.white;
        red = true;
    }
    void ColorWhite()
    {
        spriteRenderer.color = originColor;
        red = false;
    }
}
