using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        HealthPack,     
        DamageBoost,    
        FireRateBoost   
    }

    [Header("Power-Up Settings")]
    public PowerUpType type; // Imposta questo nell'Inspector per ogni prefab di power-up

    [Header("Effect Values")]
    [SerializeField] private int _healAmount = 25; 
    [SerializeField] private float _damageMultiplier = 1.5f; 
    [SerializeField] private float _boostDuration = 10f; 
    [SerializeField] private float _fireRateDecrease = 0.1f;

    
    // [SerializeField] private GameObject _pickupEffectPrefab;

    private void Start()
    {
        Destroy(this.gameObject, 5);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLifeController playerLife = other.GetComponent<PlayerLifeController>();
            PlayerShooterController playerShooter = other.GetComponent<PlayerShooterController>();
           

            if (playerLife == null || playerShooter == null)
            {
                Debug.LogError("Player manca di PlayerLifeController o PlayerShooterController!");
                Destroy(gameObject); 
                return;
            }

            ApplyEffect(playerLife, playerShooter);

            // Effetto visivo/sonoro di raccolta 
            // if (_pickupEffectPrefab != null) Instantiate(_pickupEffectPrefab, transform.position, Quaternion.identity);
            // Play a pickup sound

            Destroy(gameObject); 
        }
    }

    private void ApplyEffect(PlayerLifeController playerLife, PlayerShooterController playerShooter)
    {
        switch (type)
        {
            case PowerUpType.HealthPack:
                if (playerLife != null)
                {
                    playerLife.Heal(_healAmount);
                    Debug.Log($"Player curato di {_healAmount} HP.");
                }
                break;

            case PowerUpType.DamageBoost:
                if (playerShooter != null)
                {
                    playerShooter.ApplyDamageBoost(_damageMultiplier, _boostDuration);
                    Debug.Log($"Player ha ricevuto un Damage Boost x{_damageMultiplier} per {_boostDuration}s.");
                }
                break;

            case PowerUpType.FireRateBoost:
                if (playerShooter != null)
                {
                    // Riusiamo il metodo che avevi, magari rinominandolo per chiarezza
                    playerShooter.ApplyFireRateBoost(_fireRateDecrease, _boostDuration);
                    Debug.Log($"Player ha ricevuto un Fire Rate Boost.");
                }
                break;
        }
    }
}
