using System;
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
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string ingredientName;
    [SerializeField] private IngredientType ingredientType;
    [SerializeField] private IngredientLand ingredientLand;
    [SerializeField] private string funnyDescription;

    public GameObject Prefab { get => prefab; }
    public string IngredientName { get => ingredientName; }
    public IngredientType IngredientType { get => ingredientType; }
    public IngredientLand IngredientLand { get => ingredientLand; }
    public string FunnyDescription { get => funnyDescription; }
    public Sprite Sprite { get => sprite; }
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
