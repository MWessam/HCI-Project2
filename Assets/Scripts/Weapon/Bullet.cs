using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int damage = 20;
    public AudioClip hitSound;
    public event Action OnBulletHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.TryGetComponent(out ZombieController zombieController))
        {
            if (zombieController != null)
            {
                zombieController.TakeDamage(damage);

                //لو العدو مات ضيف نقاط الموت
                if (zombieController.currentHealth <= 0)
                {
                    ScoreManager.instance.AddScore(60);
                }
                else
                {
                    ScoreManager.instance.AddScore(10);
                }
                gameObject.SetActive(false);
                OnBulletHit?.Invoke();
                OnBulletHit = null;

                // AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }
        }

    }
}
