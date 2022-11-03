using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    public GameObject expEffect;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // ������� ������
        Destroy(effect, 1);                                                                     // ���������� ������ ����� .. ���     
        Destroy(gameObject);                                                                    // ���������� ����
    }
}
