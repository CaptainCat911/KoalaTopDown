using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("Параметры атак босса")]
    [HideInInspector] public bool attackingNow;     // сейчас атакует
    public bool mainBoss;                           // для возвращения во времени
    public bool shadowBoss;
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

    [Header("Теневой босс")]
    public Transform[] shadowTeleports;
    bool shadowTeleportReady;
    float i = 0.9f;

    public Transform waitShadowTeleport;

    public EnemySpawner[] enemySpawners;
    bool spawnReady;


    public override void Update()
    {
        if (!lowHp)
        {
            if (currentHealth < maxHealth / 4)
            {
                lowHp = true;
            }
        }

        // Теневой босс
        if (shadowBoss)
        {
            // Первая фаза
            if (currentHealth > maxHealth * 0.6f)
            {
                if (currentHealth < maxHealth * i)          // если здоровье падает на 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;                    
                }
                if (shadowTeleportReady)                    // телепортируем
                {                    
                    WarpBot(shadowTeleports[Random.Range(0, shadowTeleports.Length)]);
                    shadowTeleportReady = false;                    
                }
            }

            // Вторая фаза
            if (currentHealth <= maxHealth * 0.6f && currentHealth > maxHealth * 0.3f)
            {
                if (currentHealth < maxHealth * i)      // если здоровье падает на 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;
                    spawnReady = true;
                }
                if (shadowTeleportReady)                // телепортируем
                {                    
                    WarpBot(waitShadowTeleport);
                    shadowTeleportReady = false;
                    StartCoroutine(BossWait(12));                   
                }
                if (spawnReady)       // спауним
                {
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                    {
                        enemySpawner.enemysHowMuch += 2;
                    }
                    spawnReady = false;
                }
            }

            // Третья фаза
            if (currentHealth <= maxHealth * 0.3f)
            {
                if (currentHealth < maxHealth * i)      // если здоровье падает на 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;
                    spawnReady = true;
                }
                if (shadowTeleportReady)                // телепортируем
                {
                    WarpBot(shadowTeleports[Random.Range(0, shadowTeleports.Length)]);
                    shadowTeleportReady = false;
                }
                if (spawnReady)       // спауним
                {
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                    {
                        enemySpawner.enemysHowMuch += 3;
                    }
                    spawnReady = false;
                }
            }

            // Четвертая фаза
            if (currentHealth <= maxHealth * 0.05f)
            {
                foreach (EnemySpawner enemySpawner in enemySpawners)
                {
                    enemySpawner.enemysHowMuch = 0;
                }
            }
        }

        base.Update();
    }


    public IEnumerator BossWait(float delay)
    {
        SetNeutral(true);
        yield return new WaitForSeconds(delay);                     // задержка
        SetNeutral(false);
    }



    public void Speak()
    {
        if (!isTextDone)                            // если не проговорили весь текст
        {
            ChatBubble.Clear(gameObject);           // очищаем диалог
            ChatBubble.Create(transform, new Vector3(0f, 0.2f), textToSay[dialogeNumber], 4f);     // говорим     

            dialogeNumber++;                        // + к номеру диалога

            if (dialogeNumber >= textToSay.Length)  // если номер диалога последний
            {
                //isTextDone = true;                  // проговорили весь текст
                dialogeNumber = 0;
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
        {
            base.Death();
            if (shadowBoss)
            {
                Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 25, LayerMask.GetMask("Enemy"));     // создаем круг в позиции объекта с радиусом
                foreach (Collider2D coll in collidersHits)
                {
                    if (coll == null)
                    {
                        continue;
                    }

                    if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
                    {                       

                        Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                        fighter.TakeDamage(1000, vec2, 0);
                    }
                    collidersHits = null;
                }
                CMCameraShake.Instance.ShakeCamera(5, 0.3f);            // тряска камеры                
            }

        }
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
