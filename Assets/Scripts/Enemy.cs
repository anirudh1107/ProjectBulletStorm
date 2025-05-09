using UnityEngine;
using MoreMountains.Feel;
using MoreMountains.Feedbacks;
using System;

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

    public Action Hit;

    private float _nextShootTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        Hit += TriggerHitFeedback;
    }

    private void OnDisable()
    {
        Hit -= TriggerHitFeedback;
    }
    void Start()
    {
        _nextShootTime = _shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        _nextShootTime -= Time.deltaTime;
        if (_nextShootTime <= 0f)
        {
            _routineShakeFeedBack.PlayFeedbacks();
            _enemyGun.ShootCircularPattern(24);
            _nextShootTime = _shootInterval;
        }
    }

    void TriggerHitFeedback()
    {
        _hitFeedBack.PlayFeedbacks();
    }

    
}
