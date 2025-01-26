using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBehavior : MonoBehaviour
{
    private Ingredient ingredient;

    [SerializeField] private IngredientUI ingredientUI;

    public Ingredient Ingredient { get => ingredient; private set => ingredient = value; }

    public void SetIngredient(Ingredient ingredient)
    {
        Ingredient = ingredient;
        ingredientUI.SetUI(Ingredient);
    }
}
