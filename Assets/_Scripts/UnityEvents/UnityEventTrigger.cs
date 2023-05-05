using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    [Header("Параметры")]
    public bool isNPCTrigger;
    public bool isEnemyTrigger;
    public bool isSingleTrigger;
    public UnityEvent interactAction;


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyTrigger)
        {
            if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                interactAction.Invoke();
                if (isSingleTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (isNPCTrigger)
        {
            if (collision.gameObject.TryGetComponent(out NPC npc))
            {
                if (npc.gameObject.name != "NPC_Knight")
                    return;

                interactAction.Invoke();
                if (isSingleTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.gameObject.TryGetComponent<Player>(out Player player))
            {
                interactAction.Invoke();
                if (isSingleTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public virtual void TextToSayPlayer(string text)
    {
        GameManager.instance.player.SayText(text);
    }
}
