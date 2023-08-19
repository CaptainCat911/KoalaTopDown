using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("��������� ���� �����")]
    [HideInInspector] public bool attackingNow;     // ������ �������
    public bool mainBoss;                           // ��� ����������� �� �������
    public bool shadowBoss;
    public bool immortalBoss;                       // �����������
    public float cooldownAttack;                    // �� �����
    public bool lowHp;                              // ���� ��

    [Header("����� ��������")]
    public int rangeAttackChance = 100;     // ���� ���� �����
    public float rangeAttackDistance;       // ��������� ��� �����
    public int multiAttackChance;           // .. �����������

    [Header("����� ������")]
    public int meleeAttackChance = 100;
    public float meleeAttackDistance;       // ��������� ��� �����
    public int explousionAttackChance;

    [Header("����� �����")]
    public int spawnAttackChance;           // .. ������
    public int gravityChance;               // .. ����������
    public int laserChance;                 // .. ������
    public int teleportChance;              // .. ���������


/*    [Header("��������� �����")]
    public float meleeDistanceToAttack;
    public float rangeDistanceToAttack;*/

    [Header("����� ����")]
    public string[] textToSay;      // ����� ��� �������
    int dialogeNumber;              // ����� �������
    bool isTextDone;                // ����������� ���� �����

    [Header("������� ����")]
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

        // ������� ����
        if (shadowBoss)
        {
            // ������ ����
            if (currentHealth > maxHealth * 0.6f)
            {
                if (currentHealth < maxHealth * i)          // ���� �������� ������ �� 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;                    
                }
                if (shadowTeleportReady)                    // �������������
                {                    
                    WarpBot(shadowTeleports[Random.Range(0, shadowTeleports.Length)]);
                    shadowTeleportReady = false;                    
                }
            }

            // ������ ����
            if (currentHealth <= maxHealth * 0.6f && currentHealth > maxHealth * 0.3f)
            {
                if (currentHealth < maxHealth * i)      // ���� �������� ������ �� 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;
                    spawnReady = true;
                }
                if (shadowTeleportReady)                // �������������
                {                    
                    WarpBot(waitShadowTeleport);
                    shadowTeleportReady = false;
                    StartCoroutine(BossWait(12));                   
                }
                if (spawnReady)       // �������
                {
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                    {
                        enemySpawner.enemysHowMuch += 2;
                    }
                    spawnReady = false;
                }
            }

            // ������ ����
            if (currentHealth <= maxHealth * 0.3f)
            {
                if (currentHealth < maxHealth * i)      // ���� �������� ������ �� 10%
                {
                    i -= 0.1f;
                    shadowTeleportReady = true;
                    spawnReady = true;
                }
                if (shadowTeleportReady)                // �������������
                {
                    WarpBot(shadowTeleports[Random.Range(0, shadowTeleports.Length)]);
                    shadowTeleportReady = false;
                }
                if (spawnReady)       // �������
                {
                    foreach (EnemySpawner enemySpawner in enemySpawners)
                    {
                        enemySpawner.enemysHowMuch += 3;
                    }
                    spawnReady = false;
                }
            }

            // ��������� ����
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
        yield return new WaitForSeconds(delay);                     // ��������
        SetNeutral(false);
    }



    public void Speak()
    {
        if (!isTextDone)                            // ���� �� ����������� ���� �����
        {
            ChatBubble.Clear(gameObject);           // ������� ������
            ChatBubble.Create(transform, new Vector3(0f, 0.2f), textToSay[dialogeNumber], 4f);     // �������     

            dialogeNumber++;                        // + � ������ �������

            if (dialogeNumber >= textToSay.Length)  // ���� ����� ������� ���������
            {
                //isTextDone = true;                  // ����������� ���� �����
                dialogeNumber = 0;
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);           // ������� ������ ���� �� �����������
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
                Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, 25, LayerMask.GetMask("Enemy"));     // ������� ���� � ������� ������� � ��������
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
                CMCameraShake.Instance.ShakeCamera(5, 0.3f);            // ������ ������                
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
