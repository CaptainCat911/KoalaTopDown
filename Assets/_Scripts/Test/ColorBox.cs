using UnityEngine;

public class ColorBox : Fighter
{
    SpriteRenderer spriteRenderer;
    public float cubeRate;                  // как часто можно делать рывок 
    public float forceCube;                 // сила толчка
    float nextTimeToSwap;                   // когда в следующий раз готов рывок
    bool changeVector;



    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (!changeVector)
        {
            rb2D.AddForce(transform.right * forceCube, ForceMode2D.Impulse);
        }
        if (changeVector)
        {
            rb2D.AddForce(-transform.right * forceCube, ForceMode2D.Impulse);
        }

        if (Time.time >= nextTimeToSwap)
        {
            nextTimeToSwap = Time.time + 1f / cubeRate;                 // вычисляем кд
            changeVector = !changeVector;
        }
    }

    public override void TakeDamage(int dmg, Vector2 vec2, float pushForce)
    {
        base.TakeDamage(dmg, vec2, pushForce);
        SetColor();
    }
   public void SetColor()
    {
        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        spriteRenderer.color = randomColor;
        //Debug.Log(randomColor);        
    }
}
