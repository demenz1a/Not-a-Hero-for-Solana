using UnityEngine;
using UnityEngine.UI;

public class SpriteTransparencyPulse : MonoBehaviour
{
    [SerializeField] private float minAlpha = 0.5f; 
    [SerializeField] private float maxAlpha = 0.6f;  
    [SerializeField] private float speed = 2f;      

    public Image spriteRenderer;
    private float time;

    private void Update()
    {
        time += Time.deltaTime * speed;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(time) + 1f) / 2f);

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}

