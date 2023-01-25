using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    public bool isPlayerBullet;
    public GameObject expEffect;
    public LayerMask layerExplousion;


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void Explosion()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // создаем эффект
        Destroy(effect, 0.5f);                                                                  // уничтожаем эффект через .. сек     
        Destroy(gameObject);                                                                    // уничтожаем пулю
    }
}
