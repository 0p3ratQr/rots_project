using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MushroomBomb : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _homingDuration = 0.75f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _explodeTriggerName = "Explode";
    [SerializeField] private string _flyingBoolName = "IsFlying";
    [SerializeField] private bool _detachOnLaunch = true;
    [SerializeField] private float _colliderActivationDelay = 0.1f;
    [SerializeField] private Vector2 _positionOffset = new Vector2(0, 0.5f);

    private Collider2D _collider2D;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Transform _target;
    private Transform _ownerRoot;
    private Vector3 _currentTargetPosition;
    private float _launchTime;
    private bool _isLaunched;
    private bool _hasExploded;
    private bool _shouldResetOnEnable = true;
    private bool _isResettingForLaunch = false;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _collider2D.enabled = false;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D != null)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody2D.gravityScale = 0f;
        }
    }

    private void OnEnable()
    {
        if (!_shouldResetOnEnable)
        {
            return;
        }
        
        if (_hasExploded)
        {
            ResetState();
        }
    }

    private void Update()
    {
        if (!_isLaunched || _hasExploded)
        {
            return;
        }

        float flightTime = Time.time - _launchTime;
        if (flightTime >= _lifeTime)
        {
            Explode();
            return;
        }

        if (flightTime <= _homingDuration && _target != null)
        {
            _currentTargetPosition = _target.position;
        }

        Vector3 direction = _currentTargetPosition - transform.position;
        if (direction.sqrMagnitude > 0.0001f)
        {
            Vector3 newPosition = transform.position + direction.normalized * (_speed * Time.deltaTime);
            
            if (_rigidbody2D != null)
            {
                _rigidbody2D.MovePosition(newPosition);
            }
            else
            {
                transform.position = newPosition;
            }
        }
    }

    public void SetPositionWithOffset(Vector3 basePosition)
    {
        transform.position = basePosition + (Vector3)_positionOffset;
    }

    public void DisableShouldResetOnEnable()
    {
        _shouldResetOnEnable = false;
    }

    public void Launch(Transform target, Transform ownerRoot)
    {
        _shouldResetOnEnable = true;
        
        _isResettingForLaunch = true;
        ResetState();
        _isResettingForLaunch = false;

        _target = target;
        _ownerRoot = ownerRoot;
        _currentTargetPosition = target != null ? target.position : transform.position + transform.right;
        _launchTime = Time.time;
        _isLaunched = true;

        
        if (_rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0f;
        }

    
        if (_animator != null && !string.IsNullOrEmpty(_flyingBoolName))
        {
            _animator.SetBool(_flyingBoolName, true);
        }



        if (_detachOnLaunch)
        {
            transform.SetParent(null);
        }

        if (_colliderActivationDelay > 0)
        {
            Invoke(nameof(EnableCollider), _colliderActivationDelay);
        }
        else
        {
            EnableCollider();
        }
    }

    private void EnableCollider()
    {
        if (_collider2D != null && _isLaunched && !_hasExploded)
        {
            _collider2D.enabled = true;

        }
    }

    private void ResetState()
    {
        _hasExploded = false;
        _isLaunched = false;
        _launchTime = 0f;
        
        CancelInvoke();
        
        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }


        if (!_isResettingForLaunch && _spriteRenderer != null)
        {
            _spriteRenderer.enabled = false;
        }

        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
            
            if (!string.IsNullOrEmpty(_flyingBoolName))
            {
                _animator.SetBool(_flyingBoolName, false);
            }
            
        }
    }

    private void Explode()
    {
        if (_hasExploded)
        {
            return;
        }

        _hasExploded = true;
        _collider2D.enabled = false;

        if (_animator != null && !string.IsNullOrEmpty(_flyingBoolName))
        {
            _animator.SetBool(_flyingBoolName, false);
        }

        if (_animator != null && !string.IsNullOrEmpty(_explodeTriggerName))
        {
            _animator.SetTrigger(_explodeTriggerName);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsOwner(Transform other)
    {
        if (_ownerRoot == null || other == null)
        {
            return false;
        }

       
        Transform otherRoot = other.root;
        return other == _ownerRoot || other.IsChildOf(_ownerRoot) || otherRoot == _ownerRoot || otherRoot == _ownerRoot.root;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLaunched || _hasExploded)
        {
            return;
        }

        if (IsOwner(collision.transform))
        {
            return;
        }


    
        if (collision.transform.TryGetComponent(out Player player))
        {
            player.TakeDamage(transform, _damage);
            Explode();
        }

    }

    public void HandleExplosionFinished()
    {
        gameObject.SetActive(false);
    }

    public static explicit operator Vector3(MushroomBomb v)
    {
        throw new NotImplementedException();
    }
}
