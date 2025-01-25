using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreScript : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private List<StoreOption> options = new();
    [SerializeField] private GameObject parent;
    [SerializeField] private CupBehavior cupBehavior;
    public Action OnStoreClose;

    public void OpenStore()
    {
        NewOptions();
        parent.SetActive(true);
    }

    private void NewOptions()
    {
        var ingredients = ingredientManager.GetAllNonBaseIngredients();

        var randomIngredients = ingredients.OrderBy(x => Guid.NewGuid()).Take(3).ToList();

        for (int i = 0; i < randomIngredients.Count(); i++)
        {
            var ing = randomIngredients[i];
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
        inventoryManager.OnStoreClose();
        OnStoreClose?.Invoke();
        cupBehavior.NewCustomer(true);
        MusicManager.Instance.ShopMusic(false);
    }
}
