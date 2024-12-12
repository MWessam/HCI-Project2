using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    // Player Health Events:
    public event Action OnPlayerDeath;
    
    // Text ui element that updates on health change.
    [SerializeField] private TextMeshProUGUI _healthText;
    
    // Player movement
    // Default speed of player
    [SerializeField] float _baseSpeed;
    // Speed modifier on sprint.
    [SerializeField] float _sprintSpeedModifier;
    // Rigid body to move player.
    [SerializeField] private Rigidbody2D _rb;
    // modified speed.
    // If sprinting: _moveSpeed = _baseSpeed * _sprintSpeedModifier;
    // else: _moveSpeed = _baseSpeed;
    private float _moveSpeed;
    // User input for movement.
    private Vector2 _movementInput;
    // Handle player animation.
    [SerializeField] private Animator _animator;
    
    // Player stats
    // Max health of player.
    [SerializeField] float _maxHealth = 100;
    // How many seconds to wait to start regenerating.
    [SerializeField] float _healthRegenerationCooldown;
    // Max stamina of player.
    // Stamina: Limited sprint time.
    [SerializeField] private float _maxStamina = 100;
    // How many stamina is lost per second when sprinting.
    [SerializeField] private float _staminaLossPerSecond;
    // How many stamina is gained per second when not sprinting.
    [SerializeField] private float _staminaRegenPerSecond;
    // Current health of player.
    private float _currentHealth;
    // Current stamina of player.
    private float _currentStamina;
    // Boolean to start/stop regenerating health.
    private bool _canRegen;
    
    // Animator Keys
    // Moving animation state.
    // PLay moving animation
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    // Speed animation modifier.
    // Speed up moving animation according to speed.
    private static readonly int Speed = Animator.StringToHash("speed");
    
    // Coroutine to wait for health regeneration.
    private Coroutine _healCoroutine;
    
    // Singleton
    public static Player Instance;
    
    // Once the player has started.
    private void Awake()
    {
        Instance = this;
    }
    
    // Once the player has been initialized.
    private void Start()
    {
        _currentHealth = _maxHealth;
        Heal(0);
    }
    // Is run every frame of the game.
    // Time between frames is variable.
    void Update()
    {
        // Debug.Log(Time.deltaTime);
        // Movement Input
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        
        // Sprint input
        // If user is holding shift, and stamina is greater than 0, user can sprint and speed will be greater.
        if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
        {
            _moveSpeed = _baseSpeed * _sprintSpeedModifier;
            // Subtracts the stamina loss per second from the current stamina.
            // Clamp the value to make sure it's always between 0 and the max stamina.
            _currentStamina = Mathf.Clamp(_currentStamina - Time.deltaTime * _staminaLossPerSecond, 0, _maxStamina);
        }
        else
        {
            _moveSpeed = _baseSpeed;
            _currentStamina = Mathf.Clamp(_currentStamina + Time.deltaTime * _staminaRegenPerSecond, 0, _maxStamina);
        }
        
        // speed up the animation according to move speed.
        _animator.SetFloat(Speed, _moveSpeed);
        
        // If user has movement input, then play moving animation.
        if (_movementInput.x != 0 || _movementInput.y != 0)
        {
            _animator.SetBool(IsMoving, true);
        }
        else
        {
            _animator.SetBool(IsMoving, false);
        }
    }
    
    // Is run every 16 ms.
    // Is integrated with unity physics.
    private void FixedUpdate()
    {
        // Move the rigid body player in the direction of player input movement.
        // Move the player by the user speed.
        _rb.linearVelocity = new Vector2(_movementInput.x * _moveSpeed, _movementInput.y * _moveSpeed);
    }

    public void TakeDamage(float damage)
    {
        // Subtracts damage from health while ensuring it's between 0 and max health.
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        _canRegen = false;
        // If heal regeneration coroutine/thread is active
        if (_healCoroutine != null)
        {
            // Disable heal regeneration coroutine/thread.
            StopCoroutine(_healCoroutine);
        }
        // Start health regeneration thread.
        _healCoroutine = StartCoroutine(HealthCooldown());
        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
        _healthText.text = $"Health: {_currentHealth}";

    }
    public void Heal(float health)
    {
        // Add health to current healh while ensuring it's between 0 and maxHealth.
        _currentHealth = Mathf.Clamp(_currentHealth + health, 0, _maxHealth);
        _healthText.text = $"Health: {_currentHealth}";
    }
    private IEnumerator HealthCooldown()
    {
        // Indicate that can regen
        _canRegen = true;
        
        // Wait for health cooldown seconds
        yield return new WaitForSeconds(_healthRegenerationCooldown);
        
        // If player didnt take damage which would turn canRegen to false, then start regenerating health.
        if (_canRegen)
        {
            // While player can regenerate, and health is less than max health and health is greater than 0 
            while (_canRegen && _currentHealth < _maxHealth && _currentHealth > 0)
            {
                Heal(_maxHealth / 4);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
