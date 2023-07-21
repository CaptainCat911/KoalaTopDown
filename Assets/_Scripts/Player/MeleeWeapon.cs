using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    Player player;
    //WeaponHolder weaponHolder;                          // ссылка на скрипт weaponHolder (для удара)
    WeaponHolderMelee weaponHolderMelee;
    Animator animator;
    public LayerMask layer;                         // слои для битья

    public string weaponName;
    public bool swoard;
    public bool spear;
    public bool hummer;

    [Header("Параметры оружия")]
    public Transform hitBox;                        // положение хитбокса
    public int damage = 10;                         // урон
    public float pushForce = 1;                     // сила толчка
    public float radius = 1;                        // радиус
    public float cooldown = 1f;                     // перезардяка атаки
    float lastAttack;                               // время последнего удара (для перезарядки удара)
    [Space]
    public float capsuleLeght;
    public float capsuleWidht;

    [Header("Параметры поджога")]
    public bool ignite;
    public int damageBurn;
    public float cooldownBurn;
    public float durationBurn;

    [Header("Параметры молний")]
    public bool electro;
    public int damageElectro;
    public float pushForceElectro;
    public float radiusElectro;
    //public float cameraAmplitudeShakeElectro;       // амплитуда
    //public float cameraTimedeShakeElectro;          // длительность

    [Header("Эффекты")]    
    public TrailRenderer trail;                     // треил 
    public GameObject sparksEffect;                 // искры

    [Header("Тряска камеры при ударе")]
    public float cameraAmplitudeShake = 1f; // амплитуда
    public float cameraTimedeShake = 0.1f;  // длительность

    [Header("Звуки")]
    AudioSource audioSource;
    public AudioWeaponMelee audioWeapon;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        player = GameManager.instance.player;
        //weaponHolder = GameManager.instance.player.weaponHolder;            // находим скрипт weaponHolder
        weaponHolderMelee = GameManager.instance.player.weaponHolderMelee;      // находим скрипт weaponHolder
        animator = GetComponentInParent<Animator>();        
    }


    void Update()
    {
        if (weaponHolderMelee.attackHitBoxStart && Time.time - lastAttack > cooldown)          // если готовы атаковать и кд готово
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // присваиваем время атаки
            if (swoard)
                animator.SetTrigger("HitSwoard");
            if (spear)
                animator.SetTrigger("HitSpear");
            if (hummer)
                animator.SetTrigger("HitHummer");

            if (audioWeapon)
            {
                float audioPitch = Random.Range(0.9f, 1.1f);        // рандомный питч
                audioSource.pitch = audioPitch;
                audioSource.clip = audioWeapon.hitStart;            // звук взмаха
                audioSource.Play();                                 
            }
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits;

        if (spear)
        {
            collidersHits = Physics2D.OverlapCapsuleAll(hitBox.position, new Vector2(capsuleLeght, capsuleWidht), CapsuleDirection2D.Horizontal, player.hitBoxPivot.transform.localEulerAngles.z, layer);
        }
        else
        {
            collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layer);     // создаем круг в позиции объекта с радиусом
        }

        
        // Для электро атаки, взрыв (громовой молот)
        if (electro && collidersHits.Length > 0)        
        {
            Collider2D[] collidersHitsElectro = Physics2D.OverlapCircleAll(hitBox.position, radiusElectro, layer);     // создаем круг в позиции объекта с радиусом
            foreach (Collider2D collEl in collidersHitsElectro)
            {
                if (collEl == null)
                {
                    continue;
                }

                if (collEl.gameObject.TryGetComponent<Fighter>(out Fighter fighterEl))
                {
                    Vector2 vec2 = (collEl.transform.position - player.transform.position).normalized;
                    fighterEl.TakeDamage(damageElectro, vec2, pushForceElectro);
                }
            }

            GameObject effect = Instantiate(GameAssets.instance.explousionBlue,
                    hitBox.position, Quaternion.identity);                                      // создаем эффект убийства
            Destroy(effect, 1);                                                                 // уничтожаем эффект через .. сек
            //CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShakeElectro, cameraTimedeShakeElectro);        // тряска камеры
        }

        if (collidersHits.Length > 0)       // если во что-то попали
        {
            if (sparksEffect)
            {
                GameObject effect = Instantiate(sparksEffect, hitBox.position, Quaternion.identity);                                      // создаем эффект убийства
                Destroy(effect, 1);
                CMCameraShake.Instance.ShakeCamera(cameraAmplitudeShake, cameraTimedeShake);    // тряска камеры
            }
            if (audioWeapon)
            {
                float audioPitch = Random.Range(0.9f, 1.1f);        // рандомный питч
                audioSource.pitch = audioPitch;
                audioSource.clip = audioWeapon.hitStart;            // звук попадания
                audioSource.Play();
            }
        }

        // Обычная атака
        foreach (Collider2D coll in collidersHits)
        {
            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                Vector2 vec2 = (coll.transform.position - player.transform.position).normalized;
                fighter.TakeDamage(damage, vec2, pushForce);                
            }
            if (ignite)
            {
                if (coll.gameObject.TryGetComponent<Ignitable>(out Ignitable ignitable))
                {
                    //Vector2 vec2 = (fighter.transform.position - player.transform.position).normalized;
                    ignitable.Ignite(damageBurn, cooldownBurn, durationBurn);
                }                
            }

            collidersHits = null;
        }
    }

    public void TrailOn(int number)
    {
        if (trail)
        {
            if (number == 1)
                trail.emitting = true;
            if (number == 0)
                trail.emitting = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitBox.position, radius);
        //Gizmos.DrawLine(hitBox.position, )
    }
}
