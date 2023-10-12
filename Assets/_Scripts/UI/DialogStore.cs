using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogStore
{
	public string name;
	[TextArea(3, 20)]
	public string[] sentences;              // �����������
	[TextArea(3, 20)]
	public string[] sentencesEng;           // ����������� ��� ���� ������
	public string[] characterName;          // ��� ���������, ������� �������
	public GameObject imageNpc;             // ������� ���, � ������� ��� ��������
	//public float delayBeforeAwake;
	public UnityEvent awakeInteractAction;	// ����� �� ������ �������
	public UnityEvent goInteractAction;		// ����� ��� ������ �������
	public UnityEvent interactAction;		// ����� � ����� �������
	public Transform targetToDialoge;       // �����, ���� ���� ����� ������
	public float dialogeDelay;				// �������� ����� �������
}
