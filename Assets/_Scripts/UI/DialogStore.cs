using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogStore
{
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;              // �����������
	public string[] characterName;          // ��� ���������, ������� �������
	public GameObject imageNpc;
	public UnityEvent interactAction;
}
