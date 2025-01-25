using System;
using UnityEngine;
using UnityEngine.UI;

public class StoreOption : MonoBehaviour
{
    [SerializeField] private IngredientUI ingredientUI;
    [SerializeField] private Image storeImage;

    public Action<Ingredient> OnSelect;

    private Ingredient ingredient;

    public void SetOption(Ingredient ingredient)
    {
        this.ingredient = ingredient;
        if (ingredient.Sprite != null)
            storeImage.sprite = ingredient.Sprite;
        ingredientUI.SetUI(ingredient);
    }

    public void SelectOption()
    {
        OnSelect?.Invoke(ingredient);
        Debug.Log("Selected: " + ingredient.IngredientName);
        ingredient = null;
        OnSelect = null;
    }

}
