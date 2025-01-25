using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Customer : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    public ThoughtBubble thoughtBubble;

    private CustomerVisualizer visualizer;
    public CustomerVisualizer Visualizer { get { return visualizer; } }

    public int preferred_ingredient_count = 4;
    private List<Ingredient> preferred_ingredients = new List<Ingredient>();
    public int required_score = 0;

    public void Start()
    {
        visualizer = GetComponent<CustomerVisualizer>();
        newOrder();
    }

    public void newOrder()
    {
        visualizer.SpawnCustomerVisuals();
        preferred_ingredients.Clear();

        // Create a list of the possible Ingredients
        List<Ingredient> possible_ingredients = new List<Ingredient>();
        // for (int i = 0; i < ingredientManager.Decks.Count; i++)
        // {
        //     possible_ingredients.AddRange(ingredientManager.Decks[i].Ingredients.Select(x => x.Ingredient));
        // }
        possible_ingredients.AddRange(ingredientManager.BaseDeck.Ingredients.Select(x => x.Ingredient));

        // Randomly select preferred Ingredients
        for (int i = 0; i < preferred_ingredient_count; i++)
        {
            int index = Random.Range(0, possible_ingredients.Count);
            preferred_ingredients.Add(possible_ingredients[index]);
            possible_ingredients.RemoveAt(index);
        }

        // Get a random required score
        required_score = Random.Range(0, 100); // CURRENLTY GETS A RANDOM required_score BETWEEN 0 AND 100

        // Update thought bubble with the new order
        List<Sprite> ingredient_Sprites = new List<Sprite>(preferred_ingredients.Select(x => x.Sprite));
        thoughtBubble.newOrder(ingredient_Sprites, required_score);
    }

    public IEnumerator checkOrder(List<Ingredient> used_ingredients, int base_score)
    {
        // Update thought bubble visuals
        yield return StartCoroutine(thoughtBubble.CheckOrder(used_ingredients, preferred_ingredients, base_score, visualizer));
    }
}
