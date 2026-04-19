using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 20;
    public OnBoard onBoard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 dir)
    {
        health -= damage;
        Debug.Log("Enemy Hit!");
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        onBoard.momentum += dir;
    }
}
