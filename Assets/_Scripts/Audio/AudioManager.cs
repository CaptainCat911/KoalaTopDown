using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;         // инстанс

    public AudioClip[] tracks;              // список треков
    public bool noStartTrack;               // без стартового трека
    public float trackVolume;               // громкость треков
    public float speedTrackChange;          // скорость смены
    public float delayTrackChange = 1;      // задержка перед сменой
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
            audioSource.volume -= speedTrackChange;                // уменьшаем громкость
        }
        if (volumePlus && audioSource.volume <= 1)
        {
            if (audioSource.volume >= trackVolume)
                return;
            audioSource.volume += speedTrackChange;                // уменьшаем громкость
        }
    }

    public void SetNewTrack(int number)
    {
        StartCoroutine(SetNewTrackCoroutine(number));
    }

    public void SetTrackNumber(int number)
    {
        audioSource.clip = tracks[number];              // меняем трек
    }

    IEnumerator SetNewTrackCoroutine(int number)
    {        
        volumeMinus = true;                             // уменьшаем громкость           
        yield return new WaitForSeconds(delayTrackChange);            // ждем 

        volumeMinus = false;
        audioSource.clip = tracks[number];              // меняем трек
        audioSource.Play();
        volumePlus = true;                              // увеличиваем громкость

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
