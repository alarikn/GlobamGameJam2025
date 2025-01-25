using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventorySpawn spawner;
    [SerializeField] private List<Ingredient> currentDeck = new List<Ingredient>();
    [SerializeField] private int spawnCount = 4;

    private List<Ingredient> remainingIngredients = new List<Ingredient>();
    private List<Ingredient> discardedIngredients = new List<Ingredient>();

    private void Awake()
    {
        ingredientManager.Init();
    }

    public void Start()
    {
        currentDeck = CreateBaseDeck();
        Debug.Log("Remaining: " + currentDeck.Count);

        ShuffleIntoRemainingCards(currentDeck);

        SpawnNewIngredients();
    }

    private void ShuffleIntoRemainingCards(List<Ingredient> ingredients)
    {
        remainingIngredients = ingredients.OrderBy(x => Random.value).ToList();
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

    public void AddIngredient(Ingredient new_ingredient)
    {
        currentDeck.Add(new_ingredient);
    }

    public void SpawnNewIngredients()
    {
        var spawnedItems = new List<Ingredient>();

        for (int i = 0; i < spawnCount; i++)
        {
            if (remainingIngredients.Count == 0)
            {
                Debug.Log("Deck empty");
                ShuffleIntoRemainingCards(discardedIngredients);
                discardedIngredients.Clear();
            }
            var last = remainingIngredients[remainingIngredients.Count - 1];
            spawnedItems.Add(last);
            remainingIngredients.RemoveAt(remainingIngredients.Count - 1);
        }

        spawner.SpawnIngredients(spawnedItems);
    }

    public void Discard(Ingredient ingredient)
    {
        discardedIngredients.Add(ingredient);
    }

    public void DiscardOnTable()
    {
        foreach (var ing in spawner.Spawned)
        {
            if (ing == null)
                continue;

            Discard(ing.Ingredient);
            Destroy(ing.gameObject);
        }
        spawner.Spawned.Clear();
    }
}
