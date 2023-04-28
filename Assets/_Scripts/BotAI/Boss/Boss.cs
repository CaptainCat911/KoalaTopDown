using UnityEngine;
using UnityEngine.Events;

public class Boss : BotAI
{
    public string[] textToSay;      // ����� ��� �������
    int dialogeNumber;              // ����� �������
    bool isTextDone;                // ����������� ���� �����

    [Header("��������� ���� �����")]
    public bool immortalBoss;
    public float cooldownAttack;

    [Header("����� ��������")]
    public int rangeAttackChance = 100;     // ���� ���� �����
    public int multiAttackChance;           // .. �����������

    [Header("����� ������")]
    public int meleeAttackChance = 100;
    public int explousionAttackChance;

    [Header("����� �����")]
    public int spawnAttackChance;           // .. ������
    public int gravityChance;               // .. ����������
    public int laserChance;                 // .. ������
    public int teleportChance;              // .. ���������

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
        if (!isTextDone)                            // ���� �� ����������� ���� �����
        {
            ChatBubble.Clear(gameObject);           // ������� ������
            ChatBubble.Create(transform, new Vector3(-1f, 1.5f), textToSay[dialogeNumber], 4f);     // �������     

            dialogeNumber++;                        // + � ������ �������

            if (dialogeNumber >= textToSay.Length)  // ���� ����� ������� ���������
            {
                isTextDone = true;                  // ����������� ���� �����
            }
        }
        else
        {
            ChatBubble.Clear(gameObject);           // ������� ������ ���� �� �����������
        }
    }

/*    protected override void Death()
    {
        base.Death();

        //GameManager.instance.enemyCount--;                                                          // -1 � �������� ������

        CMCameraShake.Instance.ShakeCamera(deathCameraShake, 0.2f);                                 // ������ ������
        if (deathEffect)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);  // ������� ������ ��������
            Destroy(effect, 1);                                                                     // ���������� ������ ����� .. ���
        }
        animator.SetTrigger("Death");               // ������ 
        spriteRenderer.color = Color.white;         // ��������� ����� �� �����
        hpBarGO.SetActive(false);                   // ������� �� ���
        agent.ResetPath();                          // ���������� ����        

        if (itemToSpawn)
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);          // ������� �������

        botAIMeleeWeaponHolder.HideWeapons();       // ������ ������
        botAIRangeWeaponHolder.HideWeapons();
        animatorWeapon.animator.enabled = false;    // ��������� �������� ������
        //animatorWeapon.animator.StopPlayback();
        //gameObject.layer = LayerMask.NameToLayer("Item");                            // ���� ������ ����

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
