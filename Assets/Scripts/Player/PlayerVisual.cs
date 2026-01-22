
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FlashBlick))]
public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private FlashBlick _flashBlick;
    private static readonly int Die = Animator.StringToHash("IsDie");
    private static readonly int Running = Animator.StringToHash("IsRunning");

    private static readonly int Attack = Animator.StringToHash("IsAttack");


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlick = GetComponent<FlashBlick>();
    }

    private void Start()
    {
   
        Player.Instance.OnPlayerAttack += Player_OnPlayerAttack;
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
       
    }
    
    private void Player_OnPlayerAttack(object sender, System.EventArgs e)
    {
 
        animator.SetBool(Attack, true);
    }
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(Die, true);
        _flashBlick.StopBlinking();
    }
    private void Update()
    {
            animator.SetBool(Running, Player.Instance.IsRunning());
            if (Player.Instance.IsAlive())
                 AdjustPlayerFacingDirection();
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerScreenPosition = Player.Instance.GetPlayerPosition();

        bool isFacingLeft = mousePos.x < playerScreenPosition.x;
        spriteRenderer.flipX = isFacingLeft;
        
        // Отражаем коллайдер атаки в соответствии с направлением
        Player.Instance.FlipAttackCollider(isFacingLeft);
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
    private void OnEnable()
    {
        // Подписка перемещена в Start() где Player.Instance уже инициализирован
    }

    private void OnDisable()
    {
        if (Player.Instance == null) return;
        Player.Instance.OnPlayerAttack -= Player_OnPlayerAttack;
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }

    public void TriggerEndAttackAnimation()
    {
        Debug.Log("PlayerVisual.TriggerEndAttackAnimation() called");
        animator.SetBool(Attack, false);
        Player.Instance.AttackColliderTurnOff();
    }

    public void AttackWindowOpen()
    {
        Debug.Log("PlayerVisual.AttackWindowOpen() called");
        Player.Instance.AttackWindowOpen();
    }

    public void AttackWindowClose()
    {
        Debug.Log("PlayerVisual.AttackWindowClose() called");
        Player.Instance.AttackWindowClose();
    }
}
