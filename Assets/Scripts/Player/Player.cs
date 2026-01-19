using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(KnockBack))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;
    private Rigidbody2D rb;
    private KnockBack _knockBack;
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;      
    [Header("Dash Settings")]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCooldownTime = 0.25f;
  private float _minMovingSpeed = 0.1f;
  private bool _isRunning = false;

  private int _currentHealth;
  private bool _canTakeDamage;
  private bool _isAlive;
  private float _initialMovingSpeed;
  private bool _isDashing;
  private Camera _mainCamera;

    Vector2 inputVector;


    private void Start()
    {
        _currentHealth = maxHealth;
        _canTakeDamage = true;
        _isAlive = true;
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += Player_OnPlayerDash;
        trailRenderer.emitting = false;
    }

    private void Player_OnPlayerDash(object sender, System.EventArgs e)
    {
        Dash();
    }

    private void Dash()
    {
        if (!_isDashing)
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        _isDashing = true;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;  
        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false;
        movingSpeed = _initialMovingSpeed;

        yield return new WaitForSeconds(dashCooldownTime);
        _isDashing = false;
        
    }
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
        ActiveWeapon.Instance.GetActiveWeapon().Attack();
    }

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();
        _knockBack = GetComponent<KnockBack>();

        _mainCamera = Camera.main;
        _initialMovingSpeed = movingSpeed;

    }
    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (_knockBack.IsGettingKnockedBack) 
        return;

         HandleMovement();
    
    }
    public bool IsAlive()
    {
        return _isAlive;
    }

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (_canTakeDamage && _isAlive)
        {
        _canTakeDamage = false;
         _currentHealth = Math.Max(0, _currentHealth - damage);
        Debug.Log("Player Health: " + _currentHealth);
        _knockBack.GetKnocedBack(damageSource);

        StartCoroutine(DamageRecoveryCoroutine());
        OnFlashBlink?.Invoke(this, EventArgs.Empty);

        }
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0 && _isAlive)
        {
            _isAlive = false;
            _canTakeDamage = false;
            _knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);

        }
    }
    private IEnumerator DamageRecoveryCoroutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }
    private void HandleMovement()
    {
        // inputVector = inputVector.normalized; //Приводит длинну вектора к 1, чтобы не было ускорения по диагонали
        rb.MovePosition(rb.position + inputVector * (Time.fixedDeltaTime * movingSpeed));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return _isRunning;
    }

    public Vector3 GetPlayerPosition()
    {
        Vector3 playerScreenPosition = _mainCamera.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
    }

    // Vector2 inputVector = new Vector2(0,0);
    //     if (Input.GetKey(KeyCode.W))
    //     {
    //         inputVector.y = 1f;
    //     }
    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         inputVector.y = -1f;
    //     }
    //     if (Input.GetKey(KeyCode.A))
    //     {
    //         inputVector.x = -1f;    
    //     }
    //     if (Input.GetKey (KeyCode.D))
    //     {
    //         inputVector.x = 1f;
    //     }
}