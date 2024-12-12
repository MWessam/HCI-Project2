using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieController : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;  
    public float speed = 2f;
    public float baseSpeed = 2f;
    public float baseHealth;
    public float attackRange = 1.5f; 
    public float attackCooldown = 2f;
    public AmmoBox AmmoBoxPrefab;
    public GameObject hitParticles;
    public GameObject killParticles; 
    public AudioClip hitSound, damagedSound, killedSound;
    [SerializeField] Animator _animator;
    private Player player;  
    private float nextAttackTime = 0f;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackTwo = Animator.StringToHash("attackTwo");
    private static readonly int AttackOne = Animator.StringToHash("attackOne");
    private bool _isRunning;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    // Gain more max health and attack player
    private void Start()
    {
        currentHealth = maxHealth; 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  
    }

    public void Spawn(ZombieModifier modifier)
    {
        speed = baseSpeed * modifier.SpeedModifier;
        maxHealth = baseHealth * modifier.HealthModifier;
        if (speed > 5)
        {
            _isRunning = true;
        }
        currentHealth = maxHealth; 

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (Random.Range(1, 11) % 10 == 0)
            {
                var ammoBox = Instantiate(AmmoBoxPrefab, transform.position, Quaternion.identity);
                ammoBox.ammoAmount = Random.Range(30, 100);
            }
            OnZombieDead?.Invoke();
        }
    }
    //move towards the player
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (_isRunning)
        {
            _animator.SetBool(IsMoving, true);
            _animator.SetBool(IsRunning, false);
        }
        else
        {
            _animator.SetBool(IsRunning, true);
            _animator.SetBool(IsMoving, false);
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            AttackPlayer();
            _animator.SetBool(IsMoving, false);
            _animator.SetBool(IsRunning, false);
        }
        var direction =  transform.position - Player.Instance.transform.position;
        // Calculate the target angle (in degrees)
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a smooth rotation towards the target angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle + 90);
        transform.rotation = targetRotation;
    }

       // Zombies Attack
    private void AttackPlayer()
    {
        if (Time.time >= nextAttackTime)
        {
          
            Debug.Log("Zombie attacks!");
            nextAttackTime = Time.time + attackCooldown;
            player.TakeDamage(25);
            if (Random.Range(0, 100) % 2 == 0)
            {
                _animator.SetTrigger(AttackOne);
            }
            else
            {
                _animator.SetTrigger(AttackTwo);
            }
        }
    }

    //Attack Effects
    private void PlayAttackEffects()
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
        Instantiate(hitParticles, transform.position, Quaternion.identity);
    }


    public Action OnZombieDead;
}
