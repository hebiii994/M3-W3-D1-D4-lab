using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Rigidbody2D _rb;
    private Vector2 _dir;
    private Vector2 _lastMoveDirection = Vector2.down;

    [Header("Animation Settings")]
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Color _originalSpriteColor;

    [Header("Attack Settings")]
    public bool isAttacking = false;
    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;

        if (_animator == null)
        {
            Debug.LogError("Componente Animator mancante sul Giocatore!");
        }
        if (_spriteRenderer == null)
        {
            Debug.LogError("Componente SpriteRenderer mancante sul Giocatore!");
        }
        else
        {
            _originalSpriteColor = _spriteRenderer.color;
        }
        if (_rb == null)
        {
            Debug.LogError("Componente Rigidbody2D mancante sul Giocatore!");
        }
        if (_mainCamera == null) Debug.LogError("Main Camera non trovata! Assicurati che la tua camera principale abbia il tag 'MainCamera'.");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {       
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _dir = new Vector2(h, v);

        if (_dir.sqrMagnitude > 1)
        {
            _dir.Normalize();
        }

        bool isMoving = _dir.sqrMagnitude > 0;
        _animator.SetBool("isMoving", isMoving);

        Vector2 facingDirectionForAnimation;

        if (isMoving)
        {
            _lastMoveDirection = _dir;
            facingDirectionForAnimation = _dir;

            if (h > 0) _spriteRenderer.flipX = false;
            else if (h < 0) _spriteRenderer.flipX = true;
        }
        else
        {
            facingDirectionForAnimation = _lastMoveDirection;
            
            if (_lastMoveDirection.x > 0.01f) _spriteRenderer.flipX = false;
            else if (_lastMoveDirection.x < -0.01f) _spriteRenderer.flipX = true;
        }

        _animator.SetFloat("moveX", Mathf.Abs(facingDirectionForAnimation.x));
        _animator.SetFloat("moveY", facingDirectionForAnimation.y);

        if (Input.GetMouseButtonDown(0)) 
        {
            if (PlayerShooterController.instance != null && PlayerShooterController.instance.CanShoot())
            {
                
                Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = transform.position.z; 
                Vector2 shootDirection = (mouseWorldPosition - transform.position).normalized;

               
                _lastMoveDirection = shootDirection;
                if (shootDirection.x > 0.01f) _spriteRenderer.flipX = false;
                else if (shootDirection.x < -0.01f) _spriteRenderer.flipX = true;

                _animator.SetFloat("moveX", Mathf.Abs(shootDirection.x));
                _animator.SetFloat("moveY", shootDirection.y);

                PerformRangedAttack(shootDirection);
            }
        }

    }

    public void SetDamageBoostVisual(bool isActive)
    {
        if (_spriteRenderer == null) return; 

        if (isActive)
        {
            _spriteRenderer.color = Color.red; 
            Debug.Log("Player visivamente rosso per Damage Boost.");
        }
        else
        {
            _spriteRenderer.color = _originalSpriteColor; 
            Debug.Log("Player tornato al colore originale.");
        }
    }

    private void FixedUpdate()
    {
        if (isAttacking)
        {
            _rb.velocity = Vector2.zero;
            return;
        }
        _rb.MovePosition(_rb.position + _dir * (_speed * Time.fixedDeltaTime));
    }

    void PerformRangedAttack(Vector2 direction)
    {
        isAttacking = true;
        _animator.SetTrigger("RangedAttack");
        PlayerShooterController.instance.ShootInDirection(direction);
        Debug.Log("Ranged Attack verso: " + direction);
    }

    
    public void TriggerMeleeAttackAnimation()
    {
        if (isAttacking) return; 

        isAttacking = true;
        
        _animator.SetTrigger("MeleeAttack");
        Debug.Log("Melee Attack Animation Triggered!");
        
    }

    public void AttackFinished()
    {
        isAttacking = false;
    }
}
