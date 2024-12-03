using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private int health = 1;
    private Transform target;

    [SerializeField]
    private bool isDead = false;
    private bool isPaused;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Base").transform;
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Paused)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    private void Update()
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
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        // Play death animation or effects
        Destroy(gameObject);
    }
}
