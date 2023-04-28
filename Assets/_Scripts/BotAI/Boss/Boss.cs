using UnityEngine;
using UnityEngine.Events;

public class Boss : BotAI
{
    public string[] textToSay;      // текст дл€ диалога
    int dialogeNumber;              // номер диалога
    bool isTextDone;                // проговорили весь текст

    [Header("ѕараметры атак босса")]
    public bool immortalBoss;
    public float cooldownAttack;

    [Header("јтака издалека")]
    public int rangeAttackChance = 100;     // шанс ренж атаки
    public int multiAttackChance;           // .. мультиатаки

    [Header("јтака вблизи")]
    public int meleeAttackChance = 100;
    public int explousionAttackChance;

    [Header("ќбщие атаки")]
    public int spawnAttackChance;           // .. спауна
    public int gravityChance;               // .. гравитации
    public int laserChance;                 // .. лазера
    public int teleportChance;              // .. телепорта

    [HideInInspector] public bool attackingNow;



    public override void Update()
    {
        if (immortalBoss)
        {
            if (currentHealth < maxHealth / 10)
            {
                currentHealth = maxHealth / 10;
            }
        }

        base.Update();
    }

    public void Speak()
    {
        if (!isTextDone)                            // если не проговорили весь текст
        {
            ChatBubble.Clear(gameObject);           // очищаем диалог
            ChatBubble.Create(transform, new Vector3(-1f, 1.5f), textToSay[dialogeNumber], 4f);     // говорим     

            dialogeNumber++;                        // + к номеру диалога

            if (dialogeNumber >= textToSay.Length)  // если номер диалога последний
            {
                isTextDone = true;                  // проговорили весь текст
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);           // очищаем диалог если всЄ проговорили
        }
    }

/*    protected override void Death()
    {
        base.Death();

        //GameManager.instance.enemyCount--;                                                          // -1 к счЄтчику врагов

        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                                 // тр€ска камеры
        if (deathEffect)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // создаем эффект убийства
            Destroy(effect, 1);                                                                     // уничтожаем эффект через .. сек
        }
        animator.SetTrigger("Death");               // тригер 
        spriteRenderer.color = Color.white;         // возвращем цвета на белый
        hpBarGO.SetActive(false);                   // убираем хп бар
        agent.ResetPath();                          // сбрасываем путь        

        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);          // создаем предмет

        botAIMeleeWeaponHolder.HideWeapons();       // пр€чем оружи€
        botAIRangeWeaponHolder.HideWeapons();
        animatorWeapon.animator.enabled = false;    // отключаем аниматор оружи€
        //animatorWeapon.animator.StopPlayback();
        //gameObject.layer = LayerMask.NameToLayer("Item");                            // слой самого бота

        Invoke("AfterDeath", 0.8f);
    }*/

    /*    protected override void Death()
        {
            base.Death();
            Destroy(gameObject);
        }*/

    /*    void ClearDialoge()
        {
            ChatBubble[] chats = GetComponentsInChildren<ChatBubble>();
            foreach (ChatBubble chat in chats)
            {
                chat.gameObject.SetActive(false);
            }
        }*/
}
