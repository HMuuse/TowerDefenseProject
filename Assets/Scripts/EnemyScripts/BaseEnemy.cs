using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public event EventHandler<int> OnEnemyDied;

    [SerializeField]
    protected float speed = 3f;
    public int health;
    [SerializeField]
    protected int pointsWorth;
    protected Transform target;

    [SerializeField]
    protected bool isDead = false;
    protected bool isPaused;

    protected void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Base").transform;
    }

    protected void OnEnable()
    {
        GameManager.Instance.RegisterEnemy(this);
        ResourceManager.Instance.RegisterEnemy(this);
    }

    protected void OnDisable()
    {
        GameManager.Instance.DeregisterEnemy(this);
        ResourceManager.Instance.DeregisterEnemy(this);
    }

    protected void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    protected void GameManager_OnStateChanged(GameManager.GameState newState)
    {
        isPaused = !isPaused;
    }

    protected void Update()
    {
        if (!isDead && !isPaused)
        {
            MoveTowardsTarget();
        }
    }

    protected virtual void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        OnEnemyDied?.Invoke(this, pointsWorth);
        Destroy(gameObject);
    }
}
