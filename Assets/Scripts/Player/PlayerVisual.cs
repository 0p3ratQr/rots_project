
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


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _flashBlick = GetComponent<FlashBlick>();
    }

private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
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

        spriteRenderer.flipX = mousePos.x < playerScreenPosition.x;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Player_OnPlayerDeath;
    }
}
