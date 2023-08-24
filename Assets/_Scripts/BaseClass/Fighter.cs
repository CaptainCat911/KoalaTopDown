using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fighter : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2D;
    [HideInInspector] public CapsuleCollider2D capsuleCollider2D;

    [Header("Параметры")]
    public bool isAlive = true;             // жив здоров
    public bool isImmortal;                 // бессмертен (хп не опускаются ниже 1)
    public int currentHealth;
    public int maxHealth;
    public bool noAgro;
    GameObject floatinText;                 // текст чата
    public bool player;
    public bool isGoodPerson;               // игрок или нпс (для отображения урона)
    public bool bossHP;                     // хп босса (ссылаемся на гуи)

    [Header("Время неуязвимости")]
    public bool withTimeNoDamage;
    public float timeNoDamage;                     // перезардяка атаки
    float lastNoDamage;                               // время последнего удара (для перезарядки удара)

    float lastHealMinus;
    float cooldownHealMinus = 1f;

    // Хп бар
    public bool hpBarOn;                            // хп бар включен                                    
    public GameObject hpBarGO;                      // хп бар объект
    HpBar hpBar;                                    // хп бар скрипт
    

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        floatinText = GameAssets.instance.floatingText;

        if (hpBarOn && !bossHP)
        {
            hpBarGO = transform.GetChild(0).gameObject;         // находим хп бар (ставлю его в начало иерархии)
        }

/*        if (gameObject.TryGetComponent(out Player player) || gameObject.TryGetComponent(out NPC npc))
        {
            isGoodPerson = true;
        }*/
    }

    public virtual void Start()
    {
        if (hpBarOn)
        {
            if (bossHP)
            {
                hpBar = hpBarGO.GetComponent<HpBar>();
                hpBar.SetMaxHealth(maxHealth);                  // устанавливаем макс хп 
                hpBarGO.SetActive(false);                       // прячем хп бар, пока не получим урон
            }
            else
            {
                hpBar = GetComponentInChildren<HpBar>();        // находим скрипт хп бара
                hpBar.SetMaxHealth(maxHealth);                  // устанавливаем макс хп            
                hpBarGO.SetActive(false);                       // прячем хп бар, пока не получим урон
            }
        }
    }

    public virtual void Update()
    {
        // если здоровья больше, чем максимальное здоровье
        if (currentHealth > maxHealth)
        {
            if (Time.time - lastHealMinus > cooldownHealMinus)
            {
                lastHealMinus = Time.time;
                currentHealth -= 1;
                TextUI.instance.UpdateHealthText(false, false);
            }
        }
    }



    public virtual void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        if (!isAlive)
            return;

/*        if (withTimeNoDamage && Time.time - lastNoDamage > timeNoDamage)          // 
        {
            //Debug.Log("Attack!");
            lastNoDamage = Time.time;
        }*/

        rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);       // толчек

        if (dmg == 0)                                               // если урон 0 - не отображаем
            return;

        if (hpBarOn)
        {
            if (currentHealth == maxHealth)
                hpBarGO.SetActive(true);                            // включаем хп бар при получении урона
        }

        currentHealth -= dmg;

        // создаем циферки урона
        if (GameManager.instance.showDamage)
        {
            if (isGoodPerson)
                ShowDamageOrHeal("-" + dmg.ToString(), true);
            else
                ShowDamageOrHeal(dmg.ToString(), true);
        }

        //Death
        if (currentHealth <= 0)
        {
            if (isImmortal)
            {
                currentHealth = 1;
                return;
            }
            currentHealth = 0;                
            Death();
        }

        if (hpBarOn)
            hpBar.SetHealth(currentHealth);             // записываем кол-во хп

        if (player)
        {
            TextUI.instance.UpdateHealthText(true, true);
        }
    }




    public virtual void Heal(int healingAmount)
    {
/*        if (currentHealth == maxHealth)
            return;*/

        currentHealth += healingAmount;

        /*        if (currentHealth > maxHealth)
                    currentHealth = maxHealth;*/
        //ShowDamageOrHeal("+" + healingAmount.ToString() + " hp", false);
        
        GameManager.instance.CreateFloatingMessage("+ " + healingAmount + " hp", Color.green, transform.position);              // создаём сообщение о лечении

        if (player)
        {
            TextUI.instance.UpdateHealthText(false, true);
        }

        //GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.5f);
        //GameManager.instance.OnHitpointChange();
    }


    void ShowDamageOrHeal(string text, bool damaged)
    {
        int floatType = Random.Range(0, 3);
        GameObject textPrefab = Instantiate(floatinText, transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMeshPro>().text = text;

        if (damaged)
        {
            if (isGoodPerson)
                textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.red;
            else
                textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.white;
        }
        else
        {
            textPrefab.GetComponentInChildren<TextMeshPro>().color = Color.green;
        }

        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", floatType);
    }

    protected virtual void Death()
    {
        isAlive = false;
        if (capsuleCollider2D)
            capsuleCollider2D.enabled = false;
        
        //Debug.Log(transform.name + " died.");
    }
}