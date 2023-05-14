using UnityEngine;
using UnityEngine.Events;

public class NPC : BotAI
{
    [Header("��������� ���� �����")]
    [HideInInspector] public bool attackingNow;     // ������ �������
    public bool immortalBoss;
    public float cooldownAttack;

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
            ChatBubble.Create(transform, new Vector3(-1f, 0.2f), textToSay[dialogeNumber], 4f);     // �������     

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
