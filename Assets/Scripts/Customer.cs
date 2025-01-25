using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public List<IngredientObject> possible_ingredients = new List<IngredientObject>();
    public ThoughtBubble thoughtBubble;

    public int preferred_ingredient_count = 4;
    private List<Ingredient> preferred_ingredients = new List<Ingredient>();
    public int required_score = 0;

    public void Start()
    {
        newOrder();
    }

    public void newOrder()
    {
        // Create a local list of the Ingredients
        List<Ingredient> possible_ingredient_local = new List<Ingredient>(possible_ingredients.Select(x => x.Ingredient));

        // Randomize preferred Ingredients
        for (int i = 0; i < preferred_ingredient_count; i++)
        {
            int index = Random.Range(0, possible_ingredient_local.Count);
            preferred_ingredients.Add(possible_ingredient_local[index]);
            possible_ingredient_local.RemoveAt(index);
        }

        // Randomize a required score
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

        return multiplied_score;
    }
}
