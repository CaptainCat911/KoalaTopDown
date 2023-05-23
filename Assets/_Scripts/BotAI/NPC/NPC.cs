using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("Параметры атак босса")]
    [HideInInspector] public bool attackingNow;     // сейчас атакует
    public bool mainBossAttack;                     // для возвращения во времени
    public bool immortalBoss;                       // бессмертный
    public float cooldownAttack;                    // кд атаки
    public bool lowHp;                              // мало хп

    [Header("Атака издалека")]
    public int rangeAttackChance = 100;     // шанс ренж атаки
    public float rangeAttackDistance;       // дистанция для атаки
    public int multiAttackChance;           // .. мультиатаки

    [Header("Атака вблизи")]
    public int meleeAttackChance = 100;
    public float meleeAttackDistance;       // дистанция для атаки
    public int explousionAttackChance;

    [Header("Общие атаки")]
    public int spawnAttackChance;           // .. спауна
    public int gravityChance;               // .. гравитации
    public int laserChance;                 // .. лазера
    public int teleportChance;              // .. телепорта


/*    [Header("Дистанция атаки")]
    public float meleeDistanceToAttack;
    public float rangeDistanceToAttack;*/

    [Header("Текст чата")]
    public string[] textToSay;      // текст для диалога
    int dialogeNumber;              // номер диалога
    bool isTextDone;                // проговорили весь текст


    public override void Update()
    {
        if (!lowHp)
        {
            if (currentHealth < maxHealth / 4)
            {
                lowHp = true;
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
            ChatBubble.Clear(gameObject);           // очищаем диалог если всё проговорили
        }
    }

    /*    public void NpcMagazine()
        {
            GameManager.instance.OpenCloseMagazine();
        }*/

    protected override void Death()
    {
        if (immortalBoss)
            return;
        else
            base.Death();
        //Destroy(gameObject);
    }

    /*    void ClearDialoge()
        {
            ChatBubble[] chats = GetComponentsInChildren<ChatBubble>();
            foreach (ChatBubble chat in chats)
            {
                chat.gameObject.SetActive(false);
            }
        }*/
}
