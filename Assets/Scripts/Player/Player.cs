using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{

    public event Action OnPlayerDeath;
    public event Action<float> OnPlayerTakeDamage; 
    public event Action<float> OnPlayerHeal;
    [SerializeField] private TMP_Text _health;
    
    [SerializeField] float _baseSpeed;
    [SerializeField] float _sprintSpeedModifier;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    
    [SerializeField] float _maxHealth = 100;
    [SerializeField] float _healthRegenerationCooldown;
    [SerializeField] private float _maxStamina = 100;
    [SerializeField] private float _staminaLossPerSecond;
    [SerializeField] private float _staminaRegenPerSecond;
    
    private float _moveSpeed;
    private Vector2 _movementInput;
    
    private float _currentHealth;
    private float _currentStamina;
    private bool _canRegen;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Speed = Animator.StringToHash("speed");
    public static Player Instance;
    private Coroutine _healCoroutine;

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        _canRegen = false;
        if (_healCoroutine != null)
        {
            StopCoroutine(_healCoroutine);
        }
        _healCoroutine = StartCoroutine(HealthCooldown());
        OnPlayerTakeDamage?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
        _health.text = $"Health: {_currentHealth}";

    }
    public void Heal(float health)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + health, 0, _maxHealth);
        OnPlayerHeal?.Invoke(_currentHealth);
        _health.text = $"Health: {_currentHealth}";
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        Heal(0);
    }
    void Update()
    {
        // Movement Input
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        
        // Sprint input
        if (Input.GetKey(KeyCode.LeftShift) && _currentStamina > 0)
        {
            _moveSpeed = _baseSpeed * _sprintSpeedModifier;
            _currentStamina = Mathf.Clamp(_currentStamina - Time.deltaTime * _staminaLossPerSecond, 0, _maxStamina);
        }
        else
        {
            _currentStamina = Mathf.Clamp(_currentStamina + Time.deltaTime * _staminaRegenPerSecond, 0, _maxStamina);
            _moveSpeed = _baseSpeed;
        }

        // Shoot input
        if (Input.GetMouseButtonDown(0))
        {
            
        }
        _animator.SetFloat(Speed, _moveSpeed);
        if (_movementInput.x != 0 || _movementInput.y != 0)
        {
            _animator.SetBool(IsMoving, true);
        }
        else
        {
            _animator.SetBool(IsMoving, false);
        }
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_movementInput.x * _moveSpeed, _movementInput.y * _moveSpeed);
    }

    private IEnumerator HealthCooldown()
    {
        _canRegen = true;
        yield return new WaitForSeconds(_healthRegenerationCooldown);
        if (_canRegen)
        {
            while (_canRegen && _currentHealth < _maxHealth && _currentHealth > 0)
            {
                Heal(_maxHealth / 4);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
