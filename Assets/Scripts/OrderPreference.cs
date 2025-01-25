using UnityEngine;
using UnityEngine.UI;

public class OrderPreference : MonoBehaviour
{
    [SerializeField] private Image preferenceImage;
    [SerializeField] private Image border;

    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;

    private Ingredient ingredient;

    public Ingredient Ingredient { get => ingredient; }

    public void SetPreference(Ingredient ing)
    {
        ingredient = ing;
        preferenceImage.sprite = ing.Sprite;
    }

    public void SetSuccess(bool value)
    {
        if (value)
        {
            border.color = successColor;
        }
        else
        {
            border.color = failColor;
        }
    }
}
