using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;  
    public float speed = 2f;    
    public float attackRange = 1.5f; 
    public float attackCooldown = 2f; 
    public GameObject hitParticles;
    public GameObject killParticles; 
    public AudioClip hitSound, damagedSound, killedSound; 
    private Transform player;  
    private float nextAttackTime = 0f;

    // Gain more max health and attack player
    private void Start()
    {
        currentHealth = maxHealth; 
        player = GameObject.FindGameObjectWithTag("Player").transform;  
    }

    //move towards the player
    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

       // Zombies Attack
    private void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
          
            Debug.Log("Zombie attacks!");
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    //Attack Effects
    private void PlayAttackEffects()
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        Instantiate(hitParticles, transform.position, Quaternion.identity);
    }


   

}
