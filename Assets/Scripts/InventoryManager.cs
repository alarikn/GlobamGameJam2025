using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventorySpawn spawner;
    [SerializeField] private StoreScript storeScript;
    [SerializeField] private Customer customer;

    [SerializeField] private List<Ingredient> currentDeck = new List<Ingredient>();
    [SerializeField] private int spawnCount = 4;

    private List<Ingredient> remainingIngredients = new List<Ingredient>();
    private List<Ingredient> discardedIngredients = new List<Ingredient>();
    public List<Ingredient> CurrentDeck { get => currentDeck; }

    private void Awake()
    {
        ingredientManager.Init();
    }

    public void Start()
    {
        currentDeck = CreateBaseDeck();

        ShuffleIntoRemainingCards(CurrentDeck);

        SpawnNewIngredients();

        //storeScript.OnStoreClose += OnStoreClose;

        customer.newOrder(1); // Called here, since it needs the base deck
        AudioManager.Instance.PlaySoundEffect("SpawnIngredients");
    }

    private void ShuffleIntoRemainingCards(List<Ingredient> ingredients)
    {
        remainingIngredients.AddRange(ingredients.OrderBy(x => Guid.NewGuid()));
    }

    public List<Ingredient> CreateBaseDeck()
    {
        var ingredients = new List<Ingredient>();

        foreach (var ingredient in ingredientManager.BaseDeck.Ingredients.Select(x => x.Ingredient))
        {
            for (int i = 0; i < 3; i++)
            {
                ingredients.Add(ingredient);
            }
        }

        return ingredients;
    }

    public void AddDeckIngredient(Ingredient new_ingredient)
    {
        CurrentDeck.Add(new_ingredient);
    }

    public void OnStoreClose()
    {
        remainingIngredients.Clear();
        discardedIngredients.Clear();
        ShuffleIntoRemainingCards(CurrentDeck);
    }

    public void SpawnNewIngredients()
    {
        var remainingShitInScene = GameObject.FindObjectsByType(typeof(IngredientBehavior), FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var sc in remainingShitInScene.Select(x => (IngredientBehavior)x))
        {
            Destroy(sc.gameObject);
        }

        SpawnNewIngredients(spawnCount);
    }

    public void SpawnNewIngredients(int count, bool draw = false)
    {
        var spawnedItems = new List<Ingredient>();

        for (int i = 0; i < count; i++)
        {
            if (remainingIngredients.Count <= 0)
            {
                if (discardedIngredients.Count == 0)
                {
                    Debug.Log("Discarded deck shuffle " + discardedIngredients.Count());
                    ShuffleIntoRemainingCards(discardedIngredients);
                }
                else
                {
                    Debug.Log("Current deck shuffle " + currentDeck.Count());
                    ShuffleIntoRemainingCards(currentDeck);
                }
                discardedIngredients.Clear();
            }
            var last = remainingIngredients.LastOrDefault();
            spawnedItems.Add(last);
            if (remainingIngredients.Count == 1)
                remainingIngredients.Clear();
            else
                remainingIngredients.RemoveAt(remainingIngredients.Count - 1);
        }

        if (draw)
        {
            spawner.DrawIngredients(spawnedItems);
        }
        else
        {
            spawner.SpawnIngredients(spawnedItems);
        }
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

            DiscardAndDestroy(ing);
        }
        spawner.Spawned.Clear();
    }

    private void DiscardAndDestroy(IngredientBehavior ing)
    {
        Discard(ing.Ingredient);
        Destroy(ing.gameObject);
    }

    public void TriggerDraw(int count)
    {
        var spawned = spawner.Spawned.Where(x => x != null);

        if (count > spawnCount)
            count = spawned.Count();

        if (count <= 0)
            return;

        foreach (var s in spawned.OrderBy(x => Guid.NewGuid()).Take(count))
            DiscardAndDestroy(s);

        SpawnNewIngredients(count, true);
    }
}
