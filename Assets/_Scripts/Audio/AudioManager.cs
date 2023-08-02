using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public float trackVolume;
    public float speedTrackChange;
    public float delayTrackChange = 1;
    AudioSource audioSource;
    bool volumeMinus;
    bool volumePlus;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        SetNewTrack(0);
    }
        
    void FixedUpdate()
    {
        if (volumeMinus && audioSource.volume >= 0)      
        {
            audioSource.volume -= speedTrackChange;                // ��������� ���������
        }
        if (volumePlus && audioSource.volume <= 1)
        {
            if (audioSource.volume >= trackVolume)
                return;
            audioSource.volume += speedTrackChange;                // ��������� ���������
        }

    }

    public void SetNewTrack(int number)
    {
        StartCoroutine(SetNewTrackCoroutine(number));
    }

    IEnumerator SetNewTrackCoroutine(int number)
    {        
        volumeMinus = true;                             // ��������� ���������           
        yield return new WaitForSeconds(delayTrackChange);            // ���� 

        volumeMinus = false;
        audioSource.clip = tracks[number];              // ������ ����
        audioSource.Play();
        volumePlus = true;                              // ����������� ���������

        yield return new WaitForSeconds(delayTrackChange);
        volumePlus = false;
    }
}
