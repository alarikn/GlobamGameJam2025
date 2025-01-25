using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoreScript : MonoBehaviour
{
    [SerializeField] private IngredientManager ingredientManager;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private GameObject parent;
    private Ingredient option_1_Ingredient;
    private Ingredient option_2_Ingredient;
    private Ingredient option_3_Ingredient;
    [SerializeField] private Button option_1_button;
    [SerializeField] private Button option_2_button;
    [SerializeField] private Button option_3_button;

    public void openStore()
    {
        newOptions();

        parent.SetActive(true);
    }

    private void newOptions()
    {
        List<Ingredient> possible_ingredients = new List<Ingredient>();
        // for (int i = 0; i < ingredientManager.Decks.Count; i++)
        // {
        //     possible_ingredients.AddRange(ingredientManager.Decks[i].Ingredients.Select(x => x.Ingredient));
        // }
        possible_ingredients.AddRange(ingredientManager.BaseDeck.Ingredients.Select(x => x.Ingredient));

        int index = Random.Range(0, possible_ingredients.Count);
        option_1_Ingredient = possible_ingredients[index];
        possible_ingredients.RemoveAt(index);
        option_1_button.transform.GetChild(0).GetComponent<Image>().sprite = option_1_Ingredient.Sprite;

        index = Random.Range(0, possible_ingredients.Count);
        option_2_Ingredient = possible_ingredients[index];
        possible_ingredients.RemoveAt(index);
        option_2_button.transform.GetChild(0).GetComponent<Image>().sprite = option_2_Ingredient.Sprite;

        index = Random.Range(0, possible_ingredients.Count);
        option_3_Ingredient = possible_ingredients[index];
        possible_ingredients.RemoveAt(index);
        option_3_button.transform.GetChild(0).GetComponent<Image>().sprite = option_3_Ingredient.Sprite;
    }

    public void selectIngredient1()
    {
        inventoryManager.AddIngredient(option_1_Ingredient);

        closeStore();
    }

    public void selectIngredient2()
    {
        inventoryManager.AddIngredient(option_2_Ingredient);

        closeStore();
    }

    public void selectIngredient3()
    {
        inventoryManager.AddIngredient(option_3_Ingredient);

        closeStore();
    }

    private void closeStore()
    {
        parent.SetActive(false);
    }
}
