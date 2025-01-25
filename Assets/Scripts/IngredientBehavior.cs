using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientBehavior : MonoBehaviour
{
    private Ingredient ingredient;

    [SerializeField] private TMP_Text nameT;
    [SerializeField] private TMP_Text descriptionT;
    [SerializeField] private TMP_Text typeT;
    [SerializeField] private Image colorImage;

    public Ingredient Ingredient { get => ingredient; private set => ingredient = value; }

    public void SetIngredient(Ingredient ingredient)
    {
        Ingredient = ingredient;

        nameT.text = Ingredient.IngredientName;
        descriptionT.text = Ingredient.GetDescription();

        typeT.text = ingredient.IngredientType.ToString();
        colorImage.color = Color.red;
    }

}
