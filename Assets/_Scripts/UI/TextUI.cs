using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ������ �� ������
    public Text hp;                 // ���-�� ��

    public int keyCount;            // ���-�� ������ (����� ��������� � ���������)
    public Text key;                // ���-�� ������ (�����)

    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // �����
        key.text = keyCount.ToString("0");
    }

    public void KeyIncrement()
    {
        keyCount++;
    }
}
