using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    private PlayerController _playerController;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    private bool _isDead = false;

    public static event System.Action<int, int> OnHealthChanged;

    public static event System.Action OnPlayerDied;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _playerController = GetComponent<PlayerController>();
        if (_playerController == null)
        {
            Debug.LogError("PlayerController non trovato su questo GameObject!");
        }
    }


    void Start()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    // Update is called once per frame
    public void Heal(int amount)
    {
        if (_isDead) return; 

        _currentHealth += amount;
        
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        Debug.Log($"Player curato. Vita attuale: {_currentHealth}/{_maxHealth}");
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth); 
    }
    public void TakeDamage(int damageAmount)
    {
        if (_isDead) return;

        _currentHealth -= damageAmount;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }

        Debug.Log($"Giocatore ha subito {damageAmount} danni. Vita rimasta: {_currentHealth}");
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (!_isDead && _playerController != null) 
        {
            _playerController.TriggerMeleeAttackAnimation();
        }

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("Sei Morto!");

        OnPlayerDied?.Invoke();
        gameObject.SetActive(false);
    }
}
