using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogStore
{
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;              // предложения
	public string[] characterName;          // имя персонажа, который говорит
	public GameObject imageNpc;

}
