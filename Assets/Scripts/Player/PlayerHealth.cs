using UnityEngine;
using Michsky.MUIP;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private SliderManager _healthBar;

    private float _currentHealth;
    private float _maxHealth = 100f;
    private float _defaultHitDamage = 10f;
    private Slider _healthSlider; 

    public static Action onHit;


    private void OnEnable()
    {
        onHit += HitPlayer;

    }

    private void OnDisable()
    {
        onHit -= HitPlayer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = _maxHealth;
        _healthSlider = _healthBar.GetComponent<Slider>();
        _healthSlider.value = _currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitPlayer()
    {
        _currentHealth -= _defaultHitDamage;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        _healthSlider.value = _currentHealth;
        if (_currentHealth <= 0)
        {
            // Handle player death
            LevelManager.Instance.GameOverDialog();
        }
    }
}
