using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("ѕараметры атак босса")]
    [HideInInspector] public bool attackingNow;     // сейчас атакует
    public bool immortalBoss;
    public float cooldownAttack;

    [Header("јтака издалека")]
    public int rangeAttackChance = 100;     // шанс ренж атаки
    public float rangeAttackDistance;       // дистанци€ дл€ атаки
    public int multiAttackChance;           // .. мультиатаки

    [Header("јтака вблизи")]
    public int meleeAttackChance = 100;
    public float meleeAttackDistance;       // дистанци€ дл€ атаки
    public int explousionAttackChance;

    [Header("ќбщие атаки")]
    public int spawnAttackChance;           // .. спауна
    public int gravityChance;               // .. гравитации
    public int laserChance;                 // .. лазера
    public int teleportChance;              // .. телепорта


/*    [Header("ƒистанци€ атаки")]
    public float meleeDistanceToAttack;
    public float rangeDistanceToAttack;*/

    [Header("“екст чата")]
    public string[] textToSay;      // текст дл€ диалога
    int dialogeNumber;              // номер диалога
    bool isTextDone;                // проговорили весь текст


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
            ChatBubble.Create(transform, new Vector3(-1f, 0.2f), textToSay[dialogeNumber], 4f);     // говорим     

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

/*    public void NpcMagazine()
    {
        GameManager.instance.OpenCloseMagazine();
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
