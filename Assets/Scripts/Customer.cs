using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    public float checkOrder(List<Ingredient> used_ingredients, int base_score)
    {
        float multiplied_score = base_score;
        float[] ingredient_multipliers = new float[preferred_ingredients.Count];
        for (int i = 0; i < ingredient_multipliers.Length; i++)
        {
            ingredient_multipliers[i] = 2f;
        }
        List<int> correct_ingredient_indexes = new List<int>();

        // Multiply the score
        foreach (Ingredient ingredient in used_ingredients)
        {
            for (int i = 0; i < preferred_ingredients.Count; i++)
            {
                if (ingredient == preferred_ingredients[i])
                {
                    multiplied_score *= ingredient_multipliers[i];
                    ingredient_multipliers[i] = 1f + ((ingredient_multipliers[i] - 1f) / 2f);
                    if (!correct_ingredient_indexes.Contains(i))
                    {
                        correct_ingredient_indexes.Add(i);
                    }
                }
            }
        }

        // Update thought bubble visuals
        thoughtBubble.checkOrder(correct_ingredient_indexes, multiplied_score);

        print("base score was " + base_score + " and multiplied score is " + multiplied_score);

        return multiplied_score;
    }
}
