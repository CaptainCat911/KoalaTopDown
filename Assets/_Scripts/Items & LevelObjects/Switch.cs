using UnityEngine;

public class Switch : ItemPickUp
{
    SpriteRenderer spriteRenderer;
    bool switched;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SwitchActive()
    {
        switched = !switched;
        if (switched)
            spriteRenderer.flipX = true;
        if (!switched)
            spriteRenderer.flipX = false;
        DisableTrigger();
    }
}
