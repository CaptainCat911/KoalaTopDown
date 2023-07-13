using UnityEngine;

public class Key : ItemPickUp
{
    [Header("��� �����")]
    public int keyTypeNumber;

    [Header("�����")]
    public GameObject audioPickUp;

    public void PickUpKey()
    {
        if (keyTypeNumber == 0)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ����", Color.white, transform.position);
        }
        if (keyTypeNumber == 1)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ���������� ����", Color.white, transform.position);
        }
        if (keyTypeNumber == 2)
        {
            GameManager.instance.CreateFloatingMessage("+ 1 ������ ����", Color.cyan, transform.position);
        }
        GameManager.instance.keys[keyTypeNumber]++;

        if (audioPickUp)
        {
            GameObject sound = Instantiate(audioPickUp, transform.position, Quaternion.identity);      // ���� 
            Destroy(sound, 2f);
        }

        Destroy(gameObject);
    }
}
