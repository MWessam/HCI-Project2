using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    private void Die()
    {
        Destroy(gameObject); // مسح العدو
    }
}
