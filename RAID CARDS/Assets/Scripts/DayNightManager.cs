using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager instance;

    [Header("Ссылки")]
    public Transform npc;       // НПС, расстояние которого отслеживается
    public Transform pointA;    // Точка "ночь"
    public Transform pointB;    // Точка "день"

    [Header("Свет")]
    public Light2D globalLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.1f, 0.2f, 0.5f);

    [Header("Фон (два спрайта)")]
    public SpriteRenderer dayBackground;
    public SpriteRenderer nightBackground;

    [Header("Солнце и Луна")]
    public Transform sun;
    public Transform moon;
    public float sunYDay = 0f;
    public float sunYNight = -3f;
    public float moonYDay = 3f;
    public float moonYNight = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (npc == null || pointA == null || pointB == null) return;

        float totalDist = Vector2.Distance(pointA.position, pointB.position);
        float currentDist = Mathf.Clamp01(Vector2.Distance(npc.position, pointA.position) / totalDist);

        float nightFactor = 1f - currentDist;

        UpdateLighting(nightFactor);
        UpdateBackground(nightFactor);
        UpdateSunAndMoon(nightFactor);
    }

    private void UpdateLighting(float factor)
    {
        globalLight.color = Color.Lerp(dayColor, nightColor, factor);
    }

    private void UpdateBackground(float factor)
    {
        if (nightBackground != null)
        {
            Color c = nightBackground.color;
            c.a = Mathf.Lerp(0f, 1f, factor);
            nightBackground.color = c;
        }

        if (dayBackground != null)
        {
            Color c = dayBackground.color;
            c.a = Mathf.Lerp(1f, 0f, factor);
            dayBackground.color = c;
        }
    }

    private void UpdateSunAndMoon(float factor)
    {
        if (sun != null)
        {
            Vector3 pos = sun.position;
            pos.y = Mathf.Lerp(sunYDay, sunYNight, factor);
            sun.position = pos;
        }

        if (moon != null)
        {
            Vector3 pos = moon.position;
            pos.y = Mathf.Lerp(moonYDay, moonYNight, factor);
            moon.position = pos;
        }
    }
}
