using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;         // �������

    [Header("������")]
    public Player player;                       // ������ �� ������    
    public GameObject gui;                      // ���
    public Dialog dialog;                       // ������ ��������
    public AmmoPackKoala ammoPack;              // ������ �� �������
    public GameObject magazine;                 // �������
    bool openMagazine;                          // ������� ������

    [Header("������� ��������������")]
    public KeyCode keyToUse;                    // ������� ��� ��������
    public KeyCode keyOpenMagazine;

    [Header("��������")]
    public int gold;                            // ������
    public int keys;                            // �����
    public int battery;                         // �������

    public bool isPlayerEnactive;

    //[HideInInspector] public int enemyCount;

    public Animator blackImagesAnim;        // �������� ������ �����
    [HideInInspector] public bool playerAtTarget;



    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(gui);
            //Destroy(floatingTextManager.gameObject);
            //Destroy(hud);
            //Destroy(menu);
            //Destroy(eventSys);


            return;
        }
        // �������� instance (?) ����� ������� � �� ������ �������� ��������� ������� ��������
        instance = this;       
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ChatBubble.Clear(gameObject);
            ChatBubble.Create(player.transform, new Vector3(0.2f, 0.2f), "Hi");
        }
        
        if (Input.GetKeyDown(keyOpenMagazine))
        {
            OpenCloseMagazine();
        }
    }

    public void OpenCloseMagazine()
    {
        isPlayerEnactive = !isPlayerEnactive;
        openMagazine = !openMagazine;
        magazine.SetActive(openMagazine);
    }



    public void StartDialog(int number)
    {
        dialog.StartEvent(number);
    }

    // ׸���� ������
    public void BlackTapes(bool status)
    {
        if (status)
            blackImagesAnim.SetTrigger("In");                   // ��������� ������ ������
        else
            blackImagesAnim.SetTrigger("Out");                  // ������� ������ ������
    }


    public int GetCurrentWeaponIndex()
    {
        int weaponAmmo = player.weaponHolder.currentWeapon.weaponIndexForAmmo;
        return (weaponAmmo);
    }


    // ����������� ������
    public void MovePlayer(Vector2 targetPosition)
    {
        float distance = Vector2.Distance(player.transform.position, targetPosition);
        if (distance > 0.1f)
        {
            player.Move(targetPosition, true);            
        }
        else
        {
            player.Move(targetPosition, false);
            playerAtTarget = true;
        }
    }



    // ���������� ���� ����� � ������ �� ������������ (������ ������)
    public void EnemyResetAndNeutral(bool status)
    {
        if (status)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 20);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.ResetTarget();                                        // ���������� ����
                    botAI.isNeutral = true;                                     // ������ �����������
                }
                collidersHits = null;
            }

        }
        if (!status)
        {
            Collider2D[] collidersHits = Physics2D.OverlapCircleAll(player.transform.position, 25);         // ������� ���� � ������� ������ � �������� (�������� ����� �������� ���� (������ �������� ����� ����))
            foreach (Collider2D coll in collidersHits)
            {
                if (coll == null)
                {
                    continue;
                }

                if (coll.gameObject.TryGetComponent<BotAI>(out BotAI botAI))
                {
                    botAI.isNeutral = false;                                     // ������ �����������
                }
                collidersHits = null;
            }
        }
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)                      // ��������� ��� �������� �����
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}
