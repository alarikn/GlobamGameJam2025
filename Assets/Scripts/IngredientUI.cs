using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameT;
    [SerializeField] private TMP_Text descriptionT;
    [SerializeField] private TMP_Text funnyDescriptionT;
    [SerializeField] private TMP_Text typeT;
    [SerializeField] private Image colorImage;

    public void SetUI(Ingredient ingredient)
    {
        nameT.text = ingredient.IngredientName;
        descriptionT.text = ingredient.GetDescription();
        typeT.text = ingredient.IngredientType.ToString();
        funnyDescriptionT.text = ingredient.FunnyDescription;

        colorImage.sprite = IngredientManager.TypeSprites[ingredient.IngredientType];
        //colorImage.color = IngredientManager.LandColors[ingredient.IngredientLand];
    }
}
