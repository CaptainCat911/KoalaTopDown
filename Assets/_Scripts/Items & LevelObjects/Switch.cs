using UnityEngine;

public class Switch : ItemPickUp
{
    SpriteRenderer spriteRenderer;
    bool switched;

    public override void Awake()
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
