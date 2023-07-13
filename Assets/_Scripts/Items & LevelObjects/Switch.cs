using UnityEngine;

public class Switch : ItemPickUp
{
    SpriteRenderer spriteRenderer;
    bool switched;
    AudioSource audioSource;

    public override void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SwitchActive()
    {
        switched = !switched;
        if (switched)
            spriteRenderer.flipX = true;
        if (!switched)
            spriteRenderer.flipX = false;

        if (audioSource)                    // звук
            audioSource.Play();

        DisableTrigger();                   // вырубаем триггер
    }
}
