using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DrinkName : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Ingredient ingredient = null;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private IngredientUI ingredientUI;

    [SerializeField] private UnityEvent OnHover;
    [SerializeField] private UnityEvent OnHoverExit;

    [SerializeField] private float scaling_speed = 4f;
    [SerializeField] private RectTransform my_RectTransform;
    [SerializeField] private RectTransform background_RectTransform;

    public void setWord(Ingredient added_ingredient)
    {
        ingredient = added_ingredient;
        text.text = ingredient.CombinationName;
        background.color = IngredientManager.LandColors[ingredient.IngredientLand];
        ingredientUI.SetUI(ingredient);
    }

    void FixedUpdate()
    {
        Rescale();
    }

    public void Rescale()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, scaling_speed * Time.fixedDeltaTime); // Goes to a scale of Vector3.one
        my_RectTransform.sizeDelta = background_RectTransform.sizeDelta * transform.localScale.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnPointerEnter");

        OnHover?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("OnPointerExit");

        OnHoverExit?.Invoke();
    }

    public void UpdateColor()
    {
        background.color = IngredientManager.LandColors[ingredient.IngredientLand];
    }
}
