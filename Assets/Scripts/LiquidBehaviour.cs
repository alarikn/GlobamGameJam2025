using UnityEngine;

public class LiquidBehaviour : MonoBehaviour
{
    [SerializeField] private Renderer LiquidRenderer;
    [SerializeField] private float slerpDuration;

    private int itemsAdded = 0;
    private float currentTime = 0;
    private float startValue = -0.5f;
    private float endValue = -0.5f;

    private float foamStartValue = 0.4f;
    private float foamEndValue = 0.4f;

    private bool bottomColorAdded = false;
    private bool topColorAdded = false;

    [ColorUsageAttribute(true, true)] private Color bottomColor;
    [ColorUsageAttribute(true, true)] private Color topColor;
    [ColorUsageAttribute(true, true)] private Color bufferColor;

    [ColorUsageAttribute(true, true), SerializeField] private Color DefaultColor;

    [SerializeField] private ParticleSystem steamParticles;
    [SerializeField] private ParticleSystem starParticles;
    [SerializeField] private ParticleSystem bubbleParticles;

    private void Start()
    {
        ResetLiquid();
    }

    public void ResetLiquid()
    {
        LiquidRenderer.material.SetVector("_FillAmount", new Vector3(0, -0.5f, 0));
        itemsAdded = 0;
        currentTime = 0;
        startValue = -0.5f;
        endValue = -0.5f;
        foamStartValue = 0.4f;
        foamEndValue = 0.4f;
        bottomColorAdded = false;
        topColorAdded = false;
        bufferColor = DefaultColor;

        LiquidRenderer.material.SetColor("_Tint", DefaultColor);
        LiquidRenderer.material.SetColor("_TopColor", DefaultColor * 2);
        LiquidRenderer.material.SetColor("_RimColor", DefaultColor * 2);
        LiquidRenderer.material.SetColor("_FoamColor", DefaultColor * 2);
    }

    public void AddItem(Color color, IngredientType ingredientType)
    {
        PlayEffect(ingredientType);
        itemsAdded++;
        RiseLiquid();

        if (!bottomColorAdded)
        {
            bottomColorAdded = true;
            bottomColor = color;
            LiquidRenderer.material.SetColor("_Tint", color);
            LiquidRenderer.material.SetColor("_TopColor", color * 2);
            LiquidRenderer.material.SetColor("_RimColor", color * 2);
            LiquidRenderer.material.SetColor("_FoamColor", color );
            return;
        }

        if (bottomColorAdded)
        {
            bufferColor = bottomColor;
        }

        if (!topColorAdded)
        {
            if (color == bottomColor) return;
            topColorAdded = true;
            topColor = color;
            return;
        }

        if(bottomColorAdded && topColorAdded)
        {
            bottomColor = topColor;
        }
    }

    public void AddItem(IngredientType ingredientType)
    {
        PlayEffect(ingredientType);
        itemsAdded++;
        RiseLiquid();

        if (bottomColorAdded)
        {
            bufferColor = bottomColor;
        }

        if (bottomColorAdded && topColorAdded)
        {
            bottomColor = topColor;
        }
    }

    private void PlayEffect(IngredientType ingredientType)
    {
        AudioManager.Instance.PlaySoundEffect("AddIngredient");
        switch (ingredientType)
        {
            case IngredientType.Tea: AudioManager.Instance.PlaySoundEffect("Steam"); steamParticles.Play(); break;
            case IngredientType.Bubble: AudioManager.Instance.PlaySoundEffect("Bubbles"); bubbleParticles.Play(); break;
            case IngredientType.Flavor: AudioManager.Instance.PlaySoundEffect("Stars"); starParticles.Play(); break;
            case IngredientType.Milk: AudioManager.Instance.PlaySoundEffect("Splash"); break;
            default: break;
        }
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        float t = Mathf.Clamp01(currentTime / slerpDuration);
        float currentValue = Mathf.Lerp(startValue, endValue, t);
        LiquidRenderer.material.SetVector("_FillAmount", new Vector3(0, currentValue, 0));

        float currentFoamValue = Mathf.Lerp(foamStartValue, foamEndValue, t);
        LiquidRenderer.material.SetFloat("_FoamWidth", currentFoamValue);

        if (bottomColorAdded && !topColorAdded)
        {
            var lerpedColor2 = Color.Lerp(bufferColor, bottomColor, t);
            LiquidRenderer.material.SetColor("_Tint", lerpedColor2);
            LiquidRenderer.material.SetColor("_TopColor", lerpedColor2 * 2);
        }

        if (topColorAdded)
        {
            var lerpedColor = Color.Lerp(bottomColor, topColor, t);
            LiquidRenderer.material.SetColor("_TopColor", lerpedColor * 2);
            LiquidRenderer.material.SetColor("_FoamColor", lerpedColor);
        }
        
    }

    private void RiseLiquid()
    {
        float liquidToBe = 0.1f + (0.4f * (itemsAdded - 1));
        startValue = endValue;
        endValue = liquidToBe;

        float foamToBe = 0.4f + (0.2f * (itemsAdded - 1));
        foamStartValue = foamEndValue;
        foamEndValue = foamToBe;

        currentTime = 0;
    }
}
