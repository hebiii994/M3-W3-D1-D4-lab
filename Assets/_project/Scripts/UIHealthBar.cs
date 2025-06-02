using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Image _healthBarFillImage;

    private void OnEnable()
    {
        
        PlayerLifeController.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        
        PlayerLifeController.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        
        float fillAmount = (float)currentHealth / maxHealth;

        
        _healthBarFillImage.fillAmount = fillAmount;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
