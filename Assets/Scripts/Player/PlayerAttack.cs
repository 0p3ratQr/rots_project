using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2;
    
    private Collider2D _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<Collider2D>();
        _attackCollider.enabled = false;
    }

    public void AttackWindowOpen()
    {
        _attackCollider.enabled = true;
        Debug.Log("Attack window opened");
    }

    public void AttackWindowClose()
    {
        _attackCollider.enabled = false;
        Debug.Log("Attack window closed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(damageAmount);
        }
    }
}
