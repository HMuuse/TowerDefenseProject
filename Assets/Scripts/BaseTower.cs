using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField]
    protected LayerMask enemyLayer;
    [SerializeField]
    protected float range = 5f;
    [SerializeField]
    protected float fireRate = 1f;
    [SerializeField]
    protected int damage = 10;

    protected float attackCooldown = 0f;
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected GameObject bulletProjectilePrefab;
    [SerializeField]
    protected Transform shootPoint;

    protected int currentLevel = 1;
    protected int maxLevel = 5;

    // Update is called once per frame
    private void Update()
    {
        FindTarget();
        RotateToTarget();

        if (attackCooldown <= 0f && target != null)
        {
            Shoot();
            attackCooldown = 1f / fireRate; // Reset cooldown based on fire rate
        }

        attackCooldown -= Time.deltaTime; // Decrease cooldown over time
    }


    protected void FindTarget()
    {
        // Only check objects in the enemy layer
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            BaseEnemy enemyScript = collider.GetComponent<BaseEnemy>();
            if (enemyScript != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = collider.transform;
                }
            }
        }

        target = nearestEnemy;
    }

    protected void RotateToTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    protected void Shoot()
    {
        BaseEnemy enemy = target.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            GameObject bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPoint.transform.position, Quaternion.identity);
            BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
            bulletProjectile.SetUp(enemy.gameObject.transform.position);

            enemy.TakeDamage(damage);
        }
    }

    public virtual void UpgradeTower(int extraDamage, float extraRange, float reducedCooldown)
    {
        if (currentLevel != maxLevel)
        {
            damage += extraDamage;
            range += extraRange;
            fireRate -= reducedCooldown;
            currentLevel++;
        }
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
