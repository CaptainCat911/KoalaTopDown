using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    // �������

    AudioSource audioSource;
    public AudioClip startTikTak;
    public AudioClip[] tracks;              // ������ ������
    public AudioClip bossTrack;

    public bool noStartTrack;               // ��� ���������� �����
    public float trackVolume;               // ��������� ������
    public float speedTrackChange;          // �������� �����
    public float delayTrackChange = 1;      // �������� ����� ������
    bool volumeMinus;
    bool volumePlus;
    int i;
    bool musicGo;
    bool bossTrackGo;

    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (noStartTrack)
            return;
        SetNewTrack(startTikTak);                           // ��� ���
        audioSource.loop = true;
    }
        
    void FixedUpdate()
    {
/*        if (Application.isFocused)
            Debug.Log("Focused!");*/

        if (volumeMinus && audioSource.volume >= 0)      
        {
            audioSource.volume -= speedTrackChange;         // ��������� ���������
        }
        if (volumePlus && audioSource.volume <= 1)
        {
            if (audioSource.volume >= trackVolume)
                return;
            audioSource.volume += speedTrackChange;         // ��������� ���������
        }

        if (!audioSource.isPlaying && musicGo && Application.isFocused)
        {
            //if (!Application.isFocused)
            Debug.Log("NextTrackUpdate");
            musicGo = false;
            SetNextTrack();
        }
    }


    public void SetNextTrack()
    {
        if (bossTrackGo)            // ���� ������ ���� ����� ��������������
            return;
        SetNewTrack(tracks[i]);     // �������� ���� �� ���������
        audioSource.loop = false;
        i++;                        // + �������
        if (i >= tracks.Length)
        {
            i = 0;
        }
    }

    public void SetBossTrack()
    {
        SetNewTrack(bossTrack);
        bossTrackGo = true;
        audioSource.loop = true;
    }


    public void SetNewTrack(AudioClip track)                // ������� ����� ����
    {
/*        if (GameManager.instance.musicOff && number != 0)
            return;*/
        StartCoroutine(SetNewTrackCoroutine(track));        // ��������� ��������
    }
    IEnumerator SetNewTrackCoroutine(AudioClip track)       // ��������
    {
        volumeMinus = true;                                 // ��������� ���������           
        yield return new WaitForSeconds(delayTrackChange);  // ���� 
        volumeMinus = false;
        audioSource.clip = track;                           // ������ ����
        audioSource.Play();
        musicGo = true;                                     // ����� ������ (��� ����� �����)
        volumePlus = true;                                  // ����������� ���������
        yield return new WaitForSeconds(delayTrackChange);
        volumePlus = false;
    }







    public void SetTrackNumber(int number)
    {
        audioSource.clip = tracks[number];              // ������ ����
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
        {
            musicGo = true;
            audioSource.Play();
        }
        else
        {
            musicGo = false;
            audioSource.Stop();
        }
    }
}
