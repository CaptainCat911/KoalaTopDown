using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogStore
{
	public string name;
	[TextArea(3, 10)]
	public string[] sentences;              // предложения
	public string[] characterName;          // имя персонажа, который говорит
	public GameObject imageNpc;             // портрет нпс, с которым идёт разговор
	//public float delayBeforeAwake;
	public UnityEvent awakeInteractAction;	// ивент до начала диалога
	public UnityEvent goInteractAction;		// ивент при старте диалога
	public UnityEvent interactAction;		// ивент в конце диалога
	public Transform targetToDialoge;		// место, куда надо дойти игроку
}
