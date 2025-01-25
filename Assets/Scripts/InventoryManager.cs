using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventorySpawn spawner;
    [SerializeField] private List<Ingredient> currentDeck = new List<Ingredient>();

    private List<Ingredient> remainingIngredients = new List<Ingredient>();

    private void Awake()
    {
        ingredientManager.Init();
    }

    public void Start()
    {
        currentDeck = CreateBaseDeck();
        Debug.Log("Remaining: " + currentDeck.Count);

        remainingIngredients = currentDeck.OrderBy(x => Random.value).ToList();

        spawnNewIngredients();
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

    public void addIngredient(Ingredient new_ingredient)
    {
        currentDeck.Add(new_ingredient);
    }

    public void spawnNewIngredients()
    {
        var spawnedItems = new List<Ingredient>();

        for (int i = 0; i < 4; i++)
        {
            var last = remainingIngredients[remainingIngredients.Count - 1];
            spawnedItems.Add(last);
            remainingIngredients.RemoveAt(remainingIngredients.Count - 1);
        }

        spawner.SpawnIngredients(spawnedItems);
    }
}
