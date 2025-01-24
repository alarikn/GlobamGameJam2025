using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private List<Ingredient> remainingIngredients = new List<Ingredient>();


    public void Start()
    {
        remainingIngredients = CreateBaseDeck();
        Debug.Log("Remaining: " + remainingIngredients.Count);
    }

    public List<Ingredient> CreateBaseDeck()
    {
        var ingredients = new List<Ingredient>();

        foreach (var ingredient in ingredientManager.BaseDeck.Ingredients.Select(x => x.Ingredient))
        {
            for (int i = 0; i < 4; i++)
            {
                ingredients.Add(ingredient);
            }
        }

        return ingredients;
    }
}
