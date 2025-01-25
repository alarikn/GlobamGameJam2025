using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Ingredients/Ingredient")]
public class IngredientObject : ScriptableObject
{
    [SerializeField] Ingredient ingredient = new();
    public Ingredient Ingredient { get => ingredient; set => ingredient = value; }
}


[Serializable]
public class Ingredient
{
    [Header("Assets")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;

    [Header("Description")]
    [SerializeField] private string ingredientName;
    [SerializeField] private string combination_name;
    [SerializeField] private IngredientType ingredientType;
    [SerializeField] private IngredientLand ingredientLand;
    [SerializeField] private string funnyDescription;

    [Header("Power")]
    [SerializeField] private PowerType powerType;
    [SerializeField] private CountType countType;
    [SerializeField] private IngredientType ingredientTarget;
    [SerializeField] private IngredientLand landTarget;
    [SerializeField] private int score;
    [SerializeField] private SpecialMove specialMove;

    [Header("Color")]
    [SerializeField] private bool addsColor;
    [SerializeField, ColorUsageAttribute(true, true)] private Color colorToAdd;

    public GameObject Prefab { get => prefab; }
    public string IngredientName { get => String.IsNullOrEmpty(ingredientName) ? "Default" : ingredientName; }
    public string CombinationName { get => String.IsNullOrEmpty(combination_name) ? "Default" : combination_name; }
    public IngredientType IngredientType { get => ingredientType; }
    public IngredientLand IngredientLand { get => ingredientLand; }
    public string FunnyDescription { get => funnyDescription; }
    public Sprite Sprite { get => sprite; }
    public IngredientType IngredientTarget { get => ingredientTarget; }
    public IngredientLand LandTarget { get => landTarget; }
    public SpecialMove SpecialMove { get => specialMove; }
    public int Score { get => score; }
    public bool AddsColor { get => addsColor; }
    [ColorUsageAttribute(true, true)] public Color ColorToAdd { get => colorToAdd; }

    public void EvaluatePoints(List<Ingredient> ingredients, out int add, out int multi)
    {
        add = Score;
        multi = 0;
        int count = 0;

        switch (SpecialMove)
        {
            case SpecialMove.Draw:
                return;
            default:
                break;
        }

        switch (powerType)
        {
            case PowerType.Ingredient:
                count = ingredients.Where(x => x.IngredientType == IngredientTarget).Count();
                break;
            case PowerType.Land:
                count = ingredients.Where(x => x.IngredientLand == LandTarget).Count();
                break;
        }

        var value = count * Score;

        switch (countType)
        {
            case CountType.Add:
                add = value;
                break;
            case CountType.Multi:
                multi = value;
                break;
        }
    }

    public string GetDescription(bool skipSpecial = false)
    {
        if (!skipSpecial && SpecialMove != SpecialMove.None)
        {
            switch (SpecialMove)
            {
                case SpecialMove.Variety:
                    return $"If all ingredients are different <b>land</b>.\n" + GetScoreString();
                case SpecialMove.Draw:
                    return $"<b>Trash {Score}</b> random ingredients and <b>take {Score}</b> ingredients";
            }
        }

        string desc = string.Empty;

        desc = "For each <b>";

        switch (powerType)
        {
            case PowerType.Ingredient:
                desc += IngredientTarget.ToString();
                break;
            case PowerType.Land:
                desc += LandTarget.ToString();
                break;
        }

        desc += "</b> ingredient ";

        desc += GetScoreString();

        return desc;
    }

    private string GetScoreString()
    {
        switch (countType)
        {
            case CountType.Add:
                return $"add <b>{Score}</b> score";
            case CountType.Multi:
                return $"add <b>{Score}</b> score multiplier";
            default:
                return string.Empty;
        }

    }
}

public enum PowerType
{
    Ingredient,
    Land
}

public enum CountType
{
    Add,
    Multi
}

public enum SpecialMove
{
    None = 0,
    Variety,
    Draw,
    MindControl
}

public enum IngredientType
{
    Bubble = 0,
    Tea = 1,
    Milk = 2,
    Flavor = 3
}

public enum IngredientLand
{
    White = 0,
    Green = 1,
    Black = 2
}
