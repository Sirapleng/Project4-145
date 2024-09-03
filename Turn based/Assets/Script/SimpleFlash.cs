using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlash : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite flashSprite;
    public float flashDuration = 0.1f;
    private Sprite originalSprite;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.sprite = flashSprite;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.sprite = originalSprite;
    }
}