using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;    

    public bool isPlayerBullet;    

    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    [HideInInspector] public int enemyToDamageCount;
    [HideInInspector] public int enemyDamaged;

    [Header("Эффекты взрыва")]
    public LayerMask layerExplousion;
    public GameObject expEffect;
    public GameObject sparksEffect;

    [Header("Портальная пушка")]
    public bool portalBullet;           // портальный патрон    

    [Header("Аудио")]
    public GameObject audioExplousion;
    public AudioProjectile audioProjectile;
    AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void Explosion()
    {
        if (expEffect)
        {
            GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
            Destroy(effect, 1f);                                                                  // уничтожаем эффект через .. сек    
        }
        if (audioExplousion)
        {
            GameObject sound = Instantiate(audioExplousion, transform.position, Quaternion.identity);      // звук взрыва
            Destroy(sound, 1f);
        }

        if (portalBullet)
            Destroy(gameObject, 0.15f);
        else
            Destroy(gameObject);                                                                        // уничтожаем пулю
    }
}
