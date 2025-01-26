using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkNameGenerator : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private DrinkName drinkName;
    [SerializeField] private DrinkName tea_drinkName;

    private List<DrinkName> drinkNames = new List<DrinkName>();

    void Start()
    {
        tea_drinkName.Rescale();
    }

    public void addedIngredient(Ingredient ingredient)
    {
        DrinkName new_drinkName = Instantiate(drinkName, parent.transform);
        new_drinkName.setWord(ingredient);
        drinkNames.Add(new_drinkName);

        tea_drinkName.transform.SetAsLastSibling();
        tea_drinkName.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    public void clearWords()
    {
        for (int i = drinkNames.Count - 1; i >= 0; i--)
        {
            Destroy(drinkNames[i].gameObject);
        }
        drinkNames.Clear();

        tea_drinkName.gameObject.SetActive(false);
    }

    public void ChangeColors()
    {
        foreach (DrinkName drinkName in drinkNames)
        {
            drinkName.UpdateColor();
        }
    }

    public void ShakeName(int id)
    {
        drinkNames[id].Shake();
    }
}
