 using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashBlick : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damageableObject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.2f;

    private float _blinkTimer;
    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;
    private bool _isBlinking;

    private void Awake() 
    {


        _spriteRenderer = GetComponent<SpriteRenderer>();


        _defaultMaterial = _spriteRenderer.material;


        _isBlinking = true;
    }


    private void Start() 
    {
        
        if (damageableObject is Player player)
        {

            player.OnFlashBlink += DamageableObject_OnFlashBlink; 
        }

    }
    private void DamageableObject_OnFlashBlink(object sender, System.EventArgs e)
    {

        SetBlinkingMaterial();
    }

    private void Update()
    {
        if (!_isBlinking)
            return;

        _blinkTimer += Time.deltaTime;


        if (_blinkTimer >= 0)
        {

            Set_defaultMaterial();
        }
    }

    public void SetBlinkingMaterial()
    {


        _blinkTimer = -blinkDuration;


        if (blinkMaterial == null)
        {

            return;
        }

        _spriteRenderer.material = blinkMaterial;

    }

    public void Set_defaultMaterial()
    {


        if (_defaultMaterial == null)
        {

            return;
        }

        _spriteRenderer.material = _defaultMaterial;
    }

    public void StopBlinking()
    {


        _isBlinking = false;
        Set_defaultMaterial();
    }

    private void OnDestroy()
    {
        if (damageableObject is Player player)
        {

            player.OnFlashBlink -= DamageableObject_OnFlashBlink;
        }
    }
}
