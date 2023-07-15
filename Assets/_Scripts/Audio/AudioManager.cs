using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public float speedTrackChange;
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
        if (volumeMinus && audioSource.volume > 0)      
        {
            audioSource.volume -= speedTrackChange;                // уменьшаем громкость
        }
        if (volumePlus && audioSource.volume <1)
        {
            audioSource.volume += speedTrackChange;                // уменьшаем громкость
        }

    }

    public void SetNewTrack(int number)
    {
        StartCoroutine(SetNewTrackCoroutine(number));
    }

    IEnumerator SetNewTrackCoroutine(int number)
    {        
        volumeMinus = true;                             // уменьшаем громкость           
        yield return new WaitForSeconds(1f);          // ждем 

        volumeMinus = false;
        audioSource.clip = tracks[number];              // меняем трек
        audioSource.Play();
        volumePlus = true;                              // увеличиваем громкость

        yield return new WaitForSeconds(1f);
        volumePlus = false;
    }
}
