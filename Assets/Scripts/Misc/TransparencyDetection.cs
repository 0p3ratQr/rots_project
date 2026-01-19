using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TransparencyDetection : MonoBehaviour
{
     private const float FULL_NON_TRANSPARENT = 1.0f;
    [Range(0f, 1f)]
    [SerializeField] private float transparencyAmount = 0.8f;
    [SerializeField] private float fadeTime = 0.5f;
    SpriteRenderer _spriteRenderer;

   

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log($"[TransparencyDetection] Awake: Component initialized. Transparency amount: {transparencyAmount}, Fade time: {fadeTime}");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log($"[TransparencyDetection] OnTriggerEnter2D: Collider detected - {collider.gameObject.name}");
        if (collider.gameObject.GetComponent<Player>())
        {
            Debug.Log($"[TransparencyDetection] Player detected!");
           if(collider is CapsuleCollider2D)
           {
               Debug.Log($"[TransparencyDetection] CapsuleCollider2D detected. Starting fade to transparency: {transparencyAmount}");
               StopAllCoroutines();
               StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, transparencyAmount));
           }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Debug.Log($"[TransparencyDetection] OnTriggerExit2D: Collider left - {collider.gameObject.name}");
        if (collider.gameObject.GetComponent<Player>())
        {
            Debug.Log($"[TransparencyDetection] Player left!");
            if(collider is CapsuleCollider2D)
            {
                Debug.Log($"[TransparencyDetection] CapsuleCollider2D left. Starting fade to full opacity: {FULL_NON_TRANSPARENT}");
                StopAllCoroutines();
                StartCoroutine(FadeRoutine(_spriteRenderer, fadeTime, _spriteRenderer.color.a, FULL_NON_TRANSPARENT));
            }
        }
    }

    private IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startTransparency, float targetTransparency)
    {
        Debug.Log($"[TransparencyDetection] FadeRoutine started: duration={fadeTime}s, from alpha={startTransparency:F3} to alpha={targetTransparency:F3}");
        float elapsedTime = 0f; //время от начала прозрачности
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startTransparency, targetTransparency,  elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, newAlpha);
            Debug.Log($"[TransparencyDetection] Fading... progress: {(elapsedTime / fadeTime) * 100:F1}%, current alpha: {newAlpha:F3}");
            yield return null; // каждый кадр выполняется хуйня
        }
        
        // Ensure final alpha is set exactly
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetTransparency);
        Debug.Log($"[TransparencyDetection] FadeRoutine completed. Final alpha: {targetTransparency:F3}");
    }
}
