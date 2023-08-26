using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorWhiteScreen : MonoBehaviour
{
    public void EventStart(int number)
    {
        ArenaManager.instance.StartEvent(number);
    }
}
