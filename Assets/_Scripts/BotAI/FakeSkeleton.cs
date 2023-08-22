using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSkeleton : MonoBehaviour
{
    Player player;
    SpriteRenderer spriteRenderer;
    Animator animator;


    public bool withChat;
    public bool mage;
    public bool warrior;
    
    bool startPozorChat;
    float cooldownPozorChat;            // перезардяка атаки
    float lastPozorChat;                // время последнего удара (для перезарядки удара)
    public string[] bubbleTexts;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        player = GameManager.instance.player;

        if (mage)
        {
            animator.SetBool("Mage", true);
        }
        if (warrior)
        {
            animator.SetBool("Warrior", true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDirection = player.transform.position - transform.position;           // угол между целью и pivot оружия          
        float targetAnglePivot = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;     // находим угол в градусах             

        if (Mathf.Abs(targetAnglePivot) > 90)
        {
            spriteRenderer.flipX = true;
        }
        if (Mathf.Abs(targetAnglePivot) <= 90)
        {
            spriteRenderer.flipX = false;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, GameManager.instance.player.transform.position);
        if (distanceToPlayer <= 6)
        {
            startPozorChat = true;
        }
        else
        {
            startPozorChat = false;
        }

        cooldownPozorChat = Random.Range(4, 7);             // рандомный кд

        if (startPozorChat && withChat && Time.time - lastPozorChat > cooldownPozorChat)    // если игрок рядом и враг с чатом
        {
            lastPozorChat = Time.time;
            SayText(bubbleTexts[Random.Range(0, bubbleTexts.Length)]);                      // выдаём рандомну фразу
        }
    }

    public void SayText(string text)
    {
        ChatBubble.Clear(gameObject);
        ChatBubble.Create(transform, new Vector3(0.2f, 0.2f), text, 4f);
    }
}
