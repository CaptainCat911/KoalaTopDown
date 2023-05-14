using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    public bool isPlayerBullet;

    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    [HideInInspector] public int enemyToDamageCount;
    [HideInInspector] public int enemyDamaged;
    public LayerMask layerExplousion;
    public GameObject expEffect;
    public GameObject sparksEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void Explosion()
    {
        if (expEffect)
        {
            GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
            Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек    
        }                                                                 
        Destroy(gameObject);                                                                        // уничтожаем пулю
    }
}
