using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    Player player;                  // ������ �� ������
    public Text hp;                 // ���-�� ��
    public Text key;                // ���-�� ������
    public Text battery;            // ���-�� �������

    void Start()
    {
        player = GameManager.instance.player;
    }
   
    void Update()                                                   // (����� �������� ����� ����������)
    {
        // HP
        hp.text = player.currentHealth.ToString("0");

        // �����
        key.text = GameManager.instance.keys.ToString("0");

        // �������
        battery.text = GameManager.instance.battery.ToString("0");
    }
}
