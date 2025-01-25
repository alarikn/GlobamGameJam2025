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

    public GameObject Prefab { get => prefab; }
    public string IngredientName { get => ingredientName; }
    public IngredientType IngredientType { get => ingredientType; }
    public IngredientLand IngredientLand { get => ingredientLand; }
    public string FunnyDescription { get => funnyDescription; }
    public Sprite Sprite { get => sprite; }
    public IngredientType IngredientTarget { get => ingredientTarget; }
    public IngredientLand LandTarget { get => landTarget; }


    public void EvaluatePoints(List<Ingredient> ingredients, out int add, out int multi)
    {
        add = 0;
        multi = 0;

        int count = 0;

        switch (powerType)
        {
            case PowerType.Ingredient:
                count = ingredients.Where(x => x.IngredientType == IngredientTarget).Count();
                break;
            case PowerType.Land:
                count = ingredients.Where(x => x.IngredientLand == LandTarget).Count();
                break;
        }
        Debug.Log("Count: " + count);

        var value = count * score;
        Debug.Log("value: " + count);

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
