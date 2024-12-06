using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int damage = 20;
    public AudioClip hitSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);

                //لو العدو مات ضيف نقاط الموت
                if (enemyHealth.IsDead())
                {
                    ScoreManager.instance.AddScore(60);
                }
                else
                {
                    ScoreManager.instance.AddScore(10);
                }

                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
        }

        Destroy(gameObject); 
    }
}
