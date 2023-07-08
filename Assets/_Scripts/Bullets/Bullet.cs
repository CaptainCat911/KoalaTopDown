using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;
    AudioSource audioSource;

    public bool isPlayerBullet;

    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    [HideInInspector] public int enemyToDamageCount;
    [HideInInspector] public int enemyDamaged;

    [Header("������� ������")]
    public LayerMask layerExplousion;
    public GameObject expEffect;
    public GameObject sparksEffect;

    [Header("�����")]
    public AudioProjectile audioProjectile;

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
            GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // ������� ������
            Destroy(effect, 0.5f);                                                                  // ���������� ������ ����� .. ���    
        }
        
        Destroy(gameObject);                                                                        // ���������� ����
    }
}
