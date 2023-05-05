using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    public int shieldHp;                // текущее хп щита
    public int shieldHpMax;             // максимальное хп щита
    [HideInInspector] public bool shieldOn;     // состояние щита
    SpriteRenderer spriteRenderer;

    public bool withExplousion;
    public LayerMask layerExplousion;
    public int damage = 100;            // урон атаки
    public float radius = 10f;          // радиус атаки
    public float cooldown = 1f;         // перезардяка атаки
    float lastAttack;                   // время последнего удара (для перезарядки удара)

    //GameObject floatinText;

    private void Awake()
    {
        //floatinText = GameAssets.instance.floatingText;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (shieldHp >= 25 && !shieldOn)
        {
            ShieldOnOff(true);
        }       
        if (shieldOn)
        {
            if (Time.time - lastAttack > cooldown && withExplousion)                  // если готовы атаковать и кд готово
            {
                lastAttack = Time.time;                             // присваиваем время атаки
                Explosion();
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        shieldHp -= dmg;
        ShowDamageShield("-" + dmg.ToString());
        
        if (shieldHp <= 0)
        {
            shieldHp = 0;            
            ShieldOnOff(false);
        }
    }

    public void HealShield(int heal)
    {
        shieldHp += heal;
        ShowDamageShield("+" + heal.ToString() + " shield");
    }

    void ShowDamageShield(string text)
    {
        GameManager.instance.CreateFloatingMessage(text, Color.blue, transform.position);
    }

    void ShieldOnOff(bool on)
    {
        if (on)
        {
            spriteRenderer.enabled = true;
            shieldOn = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            shieldOn = false;
        }
    }

    public void Explosion()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(transform.position, radius, layerExplousion);     // создаем круг в позиции объекта с радиусом
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - transform.position).normalized;
                fighter.TakeDamage(damage, vec2, 30);
            }
            collidersHits = null;
        }
    }
}
