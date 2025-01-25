using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkNameGenerator : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private DrinkName drinkName;
    [SerializeField] private DrinkName tea_drinkName;

    private List<GameObject> drinkNames = new List<GameObject>();

    void Start()
    {
        DrinkName new_tea_drinkName = Instantiate(drinkName, parent.transform);
        new_tea_drinkName.setWord("Tea", new Color(0.8f, 0.7f, 0.55f));
        tea_drinkName = new_tea_drinkName;
        tea_drinkName.gameObject.SetActive(false);
    }

    public void addedIngredient(Ingredient ingredient)
    {
        DrinkName new_drinkName = Instantiate(drinkName, parent.transform);
        new_drinkName.setWord(ingredient.CombinationName, IngredientManager.LandColors[ingredient.IngredientLand]);
        drinkNames.Add(new_drinkName.gameObject);

        tea_drinkName.transform.SetAsLastSibling();
        tea_drinkName.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    public void clearWords()
    {
        for (int i = drinkNames.Count - 1; i >= 0; i--)
        {
            Destroy(drinkNames[i]);
        }
        drinkNames.Clear();

        tea_drinkName.gameObject.SetActive(false);
    }
}
