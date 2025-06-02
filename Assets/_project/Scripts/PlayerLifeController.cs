using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    private bool _isDead = false;

    public static event System.Action<int, int> OnHealthChanged;

    public static event System.Action OnPlayerDied;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }


    void Start()
    {
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
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
