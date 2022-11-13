using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour
{
    [Header("���������")]
    public bool isEnemyTrigger;
    public bool isSingleTrigger;
    public UnityEvent interactAction;


    private void OnTriggerEnter2D(Collider2D collision)
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

    public void TextToSayPlayer(string text)
    {
        GameManager.instance.player.SayText(text);
    }
}
