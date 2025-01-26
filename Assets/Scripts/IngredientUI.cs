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
    [SerializeField] private TMP_Text baseScoreT;

    [SerializeField] private Color[] blackLandColors;
    [SerializeField] private Color[] greenLandColors;
    [SerializeField] private Color[] whiteLandColors;
    [SerializeField] private TextMeshProUGUI[] texts;
    [SerializeField] private Image[] DarkImages;
    [SerializeField] private Image[] mediumImages;
    [SerializeField] private Image[] lightsImages;

    public void SetUI(Ingredient ingredient)
    {
        nameT.text = ingredient.IngredientName;
        descriptionT.text = ingredient.GetDescription();
        typeT.text = ingredient.IngredientType.ToString();
        funnyDescriptionT.text = ingredient.FunnyDescription;
        baseScoreT.text = ingredient.Score.ToString();

        colorImage.sprite = IngredientManager.TypeSprites[ingredient.IngredientType];
        //colorImage.color = IngredientManager.LandColors[ingredient.IngredientLand];

        int id = -1;
        switch (ingredient.IngredientLand)
        {
            case IngredientLand.Black: id = 0; break;
            case IngredientLand.Green: id = 1; break;
            case IngredientLand.White: id = 2; break;
        }
        SetUIColors(id);
    }

    public void SetUIColors(int id)
    {
        foreach(var t in texts)
        {
            if (id == 2) t.color = Color.black;
        }

        foreach(var dImage in DarkImages)
        {
            switch (id)
            {
                case 0: dImage.color = blackLandColors[0]; break;
                case 1: dImage.color = greenLandColors[0]; break;
                case 2: dImage.color = whiteLandColors[0]; break;
            }
        }
        foreach (var mImage in mediumImages)
        {
            switch (id)
            {
                case 0: mImage.color = blackLandColors[1]; break;
                case 1: mImage.color = greenLandColors[1]; break;
                case 2: mImage.color = whiteLandColors[1]; break;
            }
        }
        foreach (var lImage in lightsImages)
        {
            switch (id)
            {
                case 0: lImage.color = blackLandColors[2]; break;
                case 1: lImage.color = greenLandColors[2]; break;
                case 2: lImage.color = whiteLandColors[2]; break;
            }
        }
    }
}
