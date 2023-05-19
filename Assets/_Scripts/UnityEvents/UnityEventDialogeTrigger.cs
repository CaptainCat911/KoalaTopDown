using UnityEngine;
using UnityEngine.Events;

public class UnityEventDialogeTrigger : MonoBehaviour
{
    [Header("Параметры")]
    public int dialogeNumber;
    //public bool isNPCTrigger;
    //public bool isEnemyTrigger;
    //public bool isSingleTrigger;
    //public UnityEvent interactAction;


    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
/*        if (isEnemyTrigger)
        {
            if (collision.gameObject.TryGetComponent<Fighter>(out Fighter fighter))
            {
                interactAction.Invoke();
                if (isSingleTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }*/
/*        else if (isNPCTrigger)
        {
            if (collision.gameObject.TryGetComponent(out NPC npc))
            {
                interactAction.Invoke();
                if (isSingleTrigger)
                {
                    Destroy(gameObject);
                }
            }
        }*/

        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            DialogManager.instance.StartEvent(dialogeNumber);
            Destroy(gameObject);            
        }
        
    }

    public virtual void TextToSayPlayer(string text)
    {
        GameManager.instance.player.SayText(text);
    }
}
