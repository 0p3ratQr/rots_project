using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MushroomVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;
    [SerializeField] private MushroomBomb _mushroomBomb;

    [Header("Animator Parameters")]
    [SerializeField] private string _isRunningParam = "IsRunning";
    [SerializeField] private string _speedMultiplierParam = "ChasingSpeedMultiplier";
    [SerializeField] private string _meleeTrigger = "Attack";
    [SerializeField] private string _rangeTrigger = "RangeAttack";
    [SerializeField] private string _takeHitTrigger = "TakeHit";
    [SerializeField] private string _deathBool = "IsDie";

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _enemyAI.OnEnemyAttack += HandleMeleeAttack;
        _enemyAI.OnEnemyRangeAttack += HandleRangeAttack;
        _enemyEntity.OnTakeHit += HandleTakeHit;
        _enemyEntity.OnDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttack -= HandleMeleeAttack;
        _enemyAI.OnEnemyRangeAttack -= HandleRangeAttack;
        _enemyEntity.OnTakeHit -= HandleTakeHit;
        _enemyEntity.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        _animator.SetBool(_isRunningParam, _enemyAI.IsRunning);
        _animator.SetFloat(_speedMultiplierParam, _enemyAI.GetRoamingAnimationSpeed());
    }

    private void HandleMeleeAttack(object sender, EventArgs e)
    {
        _animator.SetTrigger(_meleeTrigger);
    }

    private void HandleRangeAttack(object sender, EventArgs e)
    {
        _enemyAI.SetRangeAttacking(true);
        _animator.SetTrigger(_rangeTrigger);
    }

    private void HandleTakeHit(object sender, EventArgs e)
    {
        _animator.SetTrigger(_takeHitTrigger);
    }

    private void HandleDeath(object sender, EventArgs e)
    {
        _animator.SetBool(_deathBool, true);
        _spriteRenderer.sortingOrder = -1;
        _enemyShadow.SetActive(false);
    }

    public void TriggerAttackAnimationTurnOff()
    {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn()
    {
        _enemyEntity.PolygonColliderTurnOn();
    }

    // Animation event
    public void LaunchRangeAttack()
    {
        if (_mushroomBomb == null)
        {
            return;
        }
    
        Transform playerTransform = Player.Instance != null ? Player.Instance.transform : null;
        

        if (!_mushroomBomb.gameObject.activeSelf)
        {
            _mushroomBomb.gameObject.SetActive(true);
        }

        SpriteRenderer spriteRenderer = _mushroomBomb.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }


        MushroomBomb bombComponent = _mushroomBomb.GetComponent<MushroomBomb>();
        if (bombComponent != null)
        {
            bombComponent.DisableShouldResetOnEnable();
        }
        _mushroomBomb.SetPositionWithOffset(transform.position);
        _mushroomBomb.Launch(playerTransform, transform);
        
        _enemyAI.SetRangeAttacking(false);
    }
}
