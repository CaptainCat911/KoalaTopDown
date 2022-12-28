using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb2D;
    CapsuleCollider2D capsuleCollider2D;

    [Header("Параметры")]
    public bool isAlive = true;     // жив
    public int currentHealth;       
    public int maxHealth;    
    GameObject floatinText;         // текст чата
    public bool isPlayerOrNPC;      // игрок или нпс (для отображение урона)

    public bool hpBarOn;            // хп бар включен
    HpBar hpBar;                    // хп бар
    

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        floatinText = GameAssets.instance.floatingText;
        if (hpBarOn)
        {
            hpBar = GetComponentInChildren<HpBar>();
        }
    }

    public virtual void Start()
    {
        if (gameObject.TryGetComponent(out Player player) || gameObject.TryGetComponent(out NPC npc))
        {
            isPlayerOrNPC = true;
        }

        if (hpBarOn)
        {
            hpBar.SetMaxHealth(maxHealth);            
        }
    }

       
    public virtual void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        
        currentHealth -= dmg;
        if (isPlayerOrNPC)
            ShowDamageOrHeal("-" + dmg.ToString(), true);
        else
            ShowDamageOrHeal(dmg.ToString(), true);

        rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);

        //Death
        if (currentHealth <= 0)
            {
                currentHealth = 0;                
                Death();
            }

        if (hpBarOn)
            hpBar.SetHealth(currentHealth);
    }




    public virtual void Heal(int healingAmount)
    {
        if (currentHealth == maxHealth)
            return;

        currentHealth += healingAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        ShowDamageOrHeal("+" + healingAmount.ToString() + " hp", false);

        //GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.5f);
        //GameManager.instance.OnHitpointChange();
    }


    void ShowDamageOrHeal(string text, bool damaged)
    {
        int floatType = Random.Range(0, 3);
        GameObject textPrefab = Instantiate(floatinText, transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMesh>().text = text;

        if (damaged)
        {
            if (isPlayerOrNPC)
                textPrefab.GetComponentInChildren<TextMesh>().color = Color.red;
            else
                textPrefab.GetComponentInChildren<TextMesh>().color = Color.white;
        }
        else
        {
            textPrefab.GetComponentInChildren<TextMesh>().color = Color.green;
        }

        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", floatType);
    }

    protected virtual void Death()
    {
        isAlive = false;
        capsuleCollider2D.enabled = false;
        
        //Debug.Log(transform.name + " died.");
    }
}