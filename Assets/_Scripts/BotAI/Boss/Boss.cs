using UnityEngine;
using UnityEngine.Events;

public class Boss : BotAI
{
    public string[] textToSay;      // ����� ��� �������
    int dialogeNumber;              // ����� �������
    bool isTextDone;                // ����������� ���� �����


    public void Speak()
    {
        if (!isTextDone)                            // ���� �� ����������� ���� �����
        {
            ChatBubble.Clear(gameObject);           // ������� ������
            ChatBubble.Create(transform, new Vector3(-1f, 1f), textToSay[dialogeNumber]);     // �������     

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
