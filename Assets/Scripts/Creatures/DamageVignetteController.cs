using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageVignetteController : MonoBehaviour
{
    public static DamageVignetteController Instance;
    public Player player;

    [Header("Post Processing")]
    private Volume volume;
    private Vignette vignette;

    [Header("Stat")]
    [SerializeField] private int currentDamagePoints = 0;
    [SerializeField] private float timeSinceLastHit = 0f;
    [SerializeField] private bool isRecovering = false;
    [SerializeField] private float liveIntensityValue = 0f;

    private float maxIntensity = 0.5f;
    private float targetIntensity = 0f;
    private float lastDamageTime;
    private float recoveryStartTime;

    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();

        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet(out Vignette tmpVignette))
            {
                vignette = tmpVignette;
                vignette.intensity.Override(0f);
            }
        }
    }

    void Update()
    {
        if (vignette == null) return;

        liveIntensityValue = vignette.intensity.value;

        if (currentDamagePoints > 0)
        {
            timeSinceLastHit = Time.time - lastDamageTime;

            if (!isRecovering && timeSinceLastHit >= 5f)
            {
                isRecovering = true;
                recoveryStartTime = Time.time;
            }

            if (isRecovering)
            {
                float progress = (Time.time - recoveryStartTime) / 5f;

                if (progress <= 1f)
                {
                    float startIntensity = (currentDamagePoints / 3f) * maxIntensity;
                    float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

                    vignette.intensity.Override(Mathf.Lerp(startIntensity, 0f, smoothProgress));
                }
                else
                {
                    currentDamagePoints = 0;
                    vignette.intensity.Override(0f);
                    isRecovering = false;
                    timeSinceLastHit = 0f;
                }
            }
            else
            {
                targetIntensity = (currentDamagePoints / 3f) * maxIntensity;
                float nextIntensity = Mathf.MoveTowards(vignette.intensity.value, targetIntensity, Time.deltaTime * 2f);

                vignette.intensity.Override(nextIntensity);
            }
        }
        else
        {
            timeSinceLastHit = 0f;
        }
    }

    public void TakeDamage()
    {
        if (currentDamagePoints >= 3) return;

        currentDamagePoints++;
        lastDamageTime = Time.time;
        isRecovering = false;


        if (currentDamagePoints >= 3)
        {
            player.OnDespawn();
        }
    }
}