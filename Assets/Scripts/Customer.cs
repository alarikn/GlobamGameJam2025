using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer : MonoBehaviour
{
    //[SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private ThoughtBubble thoughtBubble;
    [SerializeField] private int lastScore;

    private CustomerVisualizer visualizer;
    public CustomerVisualizer Visualizer { get { return visualizer; } }
    public ThoughtBubble ThoughtBubble { get => thoughtBubble; }

    [SerializeField] private int how_many_days_for_more_preferences = 3;
    public int required_score = 0;

    public bool Failed { get; private set; }

    public void Start()
    {
        visualizer = GetComponent<CustomerVisualizer>();
    }

    public void newOrder(int day)
    {
        Failed = false;
        visualizer.SpawnCustomerVisuals();
        thoughtBubble.ResetColor();

        // Create a list of the possible Ingredients
        List<Ingredient> possible_ingredients = new List<Ingredient>();
        possible_ingredients.AddRange(inventoryManager.CurrentDeck);

        int how_many_preferences = Mathf.Clamp(1 + (int)(day / how_many_days_for_more_preferences), 1, 4);

        var preferred_ingredients = new List<Ingredient>();
        // Randomly select preferred Ingredients
        for (int i = 0; i < how_many_preferences; i++)
        {
            int index = Random.Range(0, possible_ingredients.Count);
            preferred_ingredients.Add(possible_ingredients[index]);
            possible_ingredients.RemoveAt(index);
        }

        // Get a random required score
        required_score = Random.Range((day - 1) * 10, day * 20);

        if (lastScore > required_score)
        {
            var mov = Math.Clamp(Random.value * 0.75f, 0.35f, 0.75f);
            var tow = Mathf.Lerp((float)required_score, (float)lastScore, mov);
            tow += required_score / 10;
            required_score = (int)tow;

            Debug.Log("Bussin");
        }

        // Update thought bubble with the new order
        ThoughtBubble.newOrder(preferred_ingredients, required_score);
    }

    public IEnumerator checkOrder(List<Ingredient> addedIngredients, int base_score)
    {
        // Update thought bubble visuals
        yield return StartCoroutine(ThoughtBubble.CheckOrder(addedIngredients, base_score, visualizer));
        lastScore = ThoughtBubble.FinalScore;
        if (ThoughtBubble.FinalScore < required_score)
        {
            Failed = true;
        }
    }


    public void TriggerMindControl(int count)
    {
        var randomIngredients = inventoryManager.CurrentDeck.OrderBy(x => Guid.NewGuid()).ToList();
        var preference = ThoughtBubble.OrderPreference;
        int index = Random.Range(0, preference.Count);

        if (count > preference.Count)
            count = preference.Count;

        for (int i = 0; i < count; i++)
        {
            var rand = randomIngredients[i];
            var ing = ThoughtBubble.OrderPreferences[i].Ingredient;

            var next = 0;

            while (rand.IngredientName == ing.IngredientName || next >= randomIngredients.Count)
            {
                rand = randomIngredients[next];
                next++;
            }

            ThoughtBubble.OrderPreferences[i].SetPreference(rand);
        }
    }
}
