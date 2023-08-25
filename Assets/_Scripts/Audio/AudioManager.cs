using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;         // �������

    public AudioClip[] tracks;              // ������ ������
    public bool noStartTrack;               // ��� ���������� �����
    public float trackVolume;               // ��������� ������
    public float speedTrackChange;          // �������� �����
    public float delayTrackChange = 1;      // �������� ����� ������
    AudioSource audioSource;
    bool volumeMinus;
    bool volumePlus;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (noStartTrack)
            return;
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

    public void SetTrackNumber(int number)
    {
        audioSource.clip = tracks[number];              // ������ ����
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
    public void StopVolumeTrack()
    {

        volumeMinus = true;
    }

    public void StartVolumeTrack()
    {
/*        if (GameManager.instance.musicOff)
            return;*/
        volumePlus = true;
    }

    public void StartStopTrack(bool status)
    {
        if (status)
            audioSource.Play();
        else
            audioSource.Stop();
    }
}
