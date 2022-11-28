using UnityEngine;

public class BotAIWeapon : MonoBehaviour
{
    BotAI botAI;      
    Animator animator;

    public string weaponName;
    public Transform hitBox;
    public int damage = 10;                             // ����
    public float pushForce = 1;                         // ���� ������
    public float radius = 1;                            // ������
    public float cooldown = 1f;                         // ����������� �����
    float lastAttack;                                   // ����� ���������� ����� (��� ����������� �����)
    [HideInInspector] public LayerMask layerHit;        // ���� ��� ����� (����� �� �����)

    // ����� 
    public TrailRenderer trail;

    void Start()
    {
        botAI = GetComponentInParent<BotAI>();        
        animator = GetComponentInParent<Animator>();
        layerHit = botAI.layerHit;
    }


    void Update()
    {
        if (botAI.readyToAttack && Time.time - lastAttack > cooldown)          // ���� ������ ��������� � �� ������
        {
            //Debug.Log("Attack!");
            lastAttack = Time.time;                             // ����������� ����� �����
            animator.SetTrigger("Hit");
        }
    }

    public void MeleeAttack()
    {
        Collider2D[] collidersHits = Physics2D.OverlapCircleAll(hitBox.position, radius, layerHit);     // ������� ���� � ������� ������� � ��������
        foreach (Collider2D coll in collidersHits)
        {
            if (coll == null)
            {
                continue;
            }

            if (coll.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                fighter.TakeDamage(damage);
                Vector2 vec2 = (coll.transform.position - botAI.transform.position).normalized;
                fighter.rb2D.AddForce(vec2 * pushForce, ForceMode2D.Impulse);
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
    }
}
