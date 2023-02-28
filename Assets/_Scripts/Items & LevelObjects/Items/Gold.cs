using UnityEngine;

public class Gold : ItemPickUp
{
    public int goldValue;

    public void PickUpGold()
    {
        GameManager.instance.gold += goldValue;

        int floatType = Random.Range(0, 3);
        GameObject textPrefab = Instantiate(GameAssets.instance.floatingText, transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMesh>().text = "+ " + goldValue;
        textPrefab.GetComponentInChildren<TextMesh>().color = Color.yellow;
        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", floatType);
        Destroy(gameObject);
    }
}
