using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private List<StoreOption> options = new();
    [SerializeField] private GameObject parent;

    public void OpenStore()
    {
        NewOptions();
        parent.SetActive(true);
    }

    private void NewOptions()
    {
        var ingredients = ingredientManager.GetAllNonBaseIngredients();

        var randomIngredients = ingredients.OrderBy(x => Random.value).Take(3);

        for (int i = 0; i < randomIngredients.Count(); i++)
        {
            var ing = ingredients[i];
            var option = options[i];
            option.SetOption(ing);
            option.OnSelect += Select;
        }
    }

    public void Select(Ingredient ingredient)
    {
        inventoryManager.AddDeckIngredient(ingredient);
        CloseStore();
    }

    private void CloseStore()
    {
        parent.SetActive(false);
    }
}
