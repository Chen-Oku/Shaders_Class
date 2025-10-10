using System.Collections;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [Header("Renderer / Material")]
    [Tooltip("Renderer that uses the Shader Graph material. If empty, will try GetComponentInChildren<Renderer>()")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Shader property names")]
    [Tooltip("Name of the color property exposed by your shader graph for the fresnel color (example: _FresnelColor)")]
    [SerializeField] private string fresnelColorProperty = "_FresnelColor";
    [Tooltip("Name of the float property controlling fresnel intensity/strength (example: _FresnelIntensity)")]
    [SerializeField] private string fresnelIntensityProperty = "_FresnelIntensity";

    [Header("Flash settings")]
    [Tooltip("Color to apply to the shader's FresnelColor property when hit (will lerp from original to this).")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    // Instance of the material used at runtime (renderer.material returns an instance)
    private Material materialInstance;
    private Coroutine flashCoroutine;

    void Awake()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponentInChildren<Renderer>();

        if (targetRenderer != null)
        {
            // renderer.material gives an instance, safe to modify per-object
            materialInstance = targetRenderer.material;
        }
        else
        {
            Debug.LogWarning("TakeDamage: No Renderer assigned/found. Assign targetRenderer in the inspector.", this);
        }
    }

    /// <summary>
    /// Llamar desde el sistema de salud cuando el jugador reciba daño.
    /// </summary>
    /// <param name="amount">cantidad de daño (no usada por el efecto, pero útil para integración)</param>
    public void ReceiveDamage(float amount)
    {
        StartFlash();
    }

    /// <summary>
    /// Inicia el efecto de fresnel en rojo.
    /// </summary>
    public void StartFlash()
    {
        if (materialInstance == null) return;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        float elapsed = 0f;

        // Read original values if the properties exist
        Color originalColor = Color.black;
        float originalIntensity = 0f;
        bool hasColor = materialInstance.HasProperty(fresnelColorProperty);
        bool hasIntensity = materialInstance.HasProperty(fresnelIntensityProperty);

        if (hasColor) originalColor = materialInstance.GetColor(fresnelColorProperty);
        if (hasIntensity) originalIntensity = materialInstance.GetFloat(fresnelIntensityProperty);

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flashDuration);
            float curveVal = intensityCurve != null ? intensityCurve.Evaluate(t) : (1f - t);

            if (hasColor)
                materialInstance.SetColor(fresnelColorProperty, Color.Lerp(originalColor, hitColor, curveVal));

            if (hasIntensity)
                materialInstance.SetFloat(fresnelIntensityProperty, Mathf.Lerp(originalIntensity, 1f, curveVal));

            yield return null;
        }

        // Restore original values
        if (hasColor)
            materialInstance.SetColor(fresnelColorProperty, originalColor);

        if (hasIntensity)
            materialInstance.SetFloat(fresnelIntensityProperty, originalIntensity);

        flashCoroutine = null;
    }

    // Optional: expose a debug key to test in Play mode (works in Editor only)
    #if UNITY_EDITOR
    void Update()
    {
        // Press 'K' to test the flash while in play mode
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartFlash();
        }
    }
    #endif

    /*
     Notes / Integration tips:
     - Asegúrate de que tu Shader Graph exponga dos propiedades (por ejemplo):
         Color _FresnelColor
         Float _FresnelIntensity
       y que esas sean los nombres que pongas en 'fresnelColorProperty' y 'fresnelIntensityProperty'.
     - Si tu shader usa otros nombres, cámbialos aquí o en el inspector.
     - Si tienes muchos objetos que deben parpadear con el mismo material, considera usar
       MaterialPropertyBlock para no instanciar materiales (más eficiente). Si quieres, puedo
       añadir una versión que use MaterialPropertyBlock.
    */
}
