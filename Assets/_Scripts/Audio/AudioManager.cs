using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;    // инстанс

    AudioSource audioSource;
    public AudioClip startTikTak;
    public AudioClip[] tracks;              // список треков
    public AudioClip bossTrack;

    public bool noStartTrack;               // без стартового трека
    public float trackVolume;               // громкость треков
    public float speedTrackChange;          // скорость смены
    public float delayTrackChange = 1;      // задержка перед сменой
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
        SetNewTrack(startTikTak);                           // тик так
        audioSource.loop = true;
    }
        
    void FixedUpdate()
    {
/*        if (Application.isFocused)
            Debug.Log("Focused!");*/

        if (volumeMinus && audioSource.volume >= 0)      
        {
            audioSource.volume -= speedTrackChange;         // уменьшаем громкость
        }
        if (volumePlus && audioSource.volume <= 1)
        {
            if (audioSource.volume >= trackVolume)
                return;
            audioSource.volume += speedTrackChange;         // уменьшаем громкость
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
        if (bossTrackGo)            // если играет трек босса возвраащааемся
            return;
        SetNewTrack(tracks[i]);     // выбираем трек из плейлиста
        audioSource.loop = false;
        i++;                        // + счетчик
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


    public void SetNewTrack(AudioClip track)                // выбрать новый трек
    {
/*        if (GameManager.instance.musicOff && number != 0)
            return;*/
        StartCoroutine(SetNewTrackCoroutine(track));        // запускаем коротину
    }
    IEnumerator SetNewTrackCoroutine(AudioClip track)       // коротина
    {
        volumeMinus = true;                                 // уменьшаем громкость           
        yield return new WaitForSeconds(delayTrackChange);  // ждем 
        volumeMinus = false;
        audioSource.clip = track;                           // меняем трек
        audioSource.Play();
        musicGo = true;                                     // пошла музыка (для смены трека)
        volumePlus = true;                                  // увеличиваем громкость
        yield return new WaitForSeconds(delayTrackChange);
        volumePlus = false;
    }







    public void SetTrackNumber(int number)
    {
        audioSource.clip = tracks[number];              // меняем трек
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
