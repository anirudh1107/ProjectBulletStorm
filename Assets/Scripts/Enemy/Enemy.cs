using UnityEngine;
using MoreMountains.Feel;
using MoreMountains.Feedbacks;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private EnemyGun _enemyGun;
    [SerializeField]
    private float _shootInterval = 5f;
    [SerializeField]
    private MMF_Player _routineShakeFeedBack;
    [SerializeField]
    private MMF_Player _hitFeedBack;
    [SerializeField]
    private GameObject _infectedVisuals;
    [SerializeField]
    private MMF_Player _entryFeedback;

    public Action Hit;

    private BoxCollider2D _collider;
    private float _nextShootTime;
    private bool _isInfected = false;
    private float healthPoints = 100f;
    private float maxHealthPoints = 100f;
    private float _defaultHitDamage = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        Hit += TriggerHitFeedback;
        Hit += DecrementHealth;
    }

    private void OnDisable()
    {
        Hit -= TriggerHitFeedback;
        Hit -= DecrementHealth;
    }
    void Start()
    {
        _nextShootTime = _shootInterval;
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInfected)
        {
            ShootBullets();
        }
    }

    private void ShootBullets()
    {
        _nextShootTime -= Time.deltaTime;
        if (_nextShootTime <= 0f)
        {
            _nextShootTime = _shootInterval;
            StartCoroutine(ShootEnumerator());
        }
    }

    void TriggerHitFeedback()
    {
        _hitFeedBack.PlayFeedbacks();
    }

    public void Infect()
    {
        if (_isInfected)
        {
            return;
        }
        _collider.enabled = true;
        _infectedVisuals.SetActive(true);
        _isInfected = true;
    }

    public void DisInfect()
    {
        if (!_isInfected)
        {
            return;
        }
        if (LevelManager.Instance != null)
        {
            LevelManager.DisInfect();
        }
        else if (Level2Manager.Instance != null)
        {
            Level2Manager.DisInfect();
        }
        _infectedVisuals.SetActive(false);
        _collider.enabled = false;
        _isInfected = false;
    }

    public void IncrementHealth(float healthToAdd)
    {
        if (healthPoints + healthToAdd > maxHealthPoints)
        {
            healthPoints = maxHealthPoints;
        }
        else
        {
            healthPoints += healthToAdd;
        }
    }

    public void DecrementHealth()
    {
        if (healthPoints - _defaultHitDamage <= 0)
        {
            healthPoints = 0;
            DisInfect();
        }
        else
        {
            healthPoints -= _defaultHitDamage;
        }
    }

    public void TriggerEntryFeedback()
    {
        if (_entryFeedback != null)
        {
            _entryFeedback.PlayFeedbacks();
        }
    }

    private IEnumerator ShootEnumerator()
    {
        _routineShakeFeedBack.PlayFeedbacks();
        yield return new WaitForSeconds(2f);
        _enemyGun.ShootCircularPattern(40, this.transform.position);
    }

}
