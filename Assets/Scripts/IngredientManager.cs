using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientManager", menuName = "Ingredients/IngredientManager")]
public class IngredientManager : ScriptableObject
{
    [SerializeField] private IngredientDeck baseDeck;
    [SerializeField] private List<IngredientDeck> decks;
    [SerializeField] private List<TypeSprites> typeSprites;

    public List<IngredientDeck> Decks { get => decks; }
    public IngredientDeck BaseDeck { get => baseDeck; }

    public static IngredientManager Instance { get; private set; }
    public static Dictionary<IngredientLand, Color> LandColors { get; private set; }
    public static Dictionary<IngredientType, Sprite> TypeSprites => Instance.typeSprites.ToDictionary(x => x.type, x => x.sprite);

    public void Init()
    {
        Instance = this;

        LandColors = new Dictionary<IngredientLand, Color>();

        foreach (var deck in Decks)
        {
            if (!LandColors.ContainsKey(deck.Land))
            {
                LandColors.Add(deck.Land, deck.Color);
            }
        }
    }

    public List<Ingredient> GetAllNonBaseIngredients()
    {
        return decks.SelectMany(x => x.Ingredients)
            .Where(x => !BaseDeck.Ingredients.Any(baseIng => x == baseIng))
            .Select(x => x.Ingredient)
            .ToList();
    }

}

[Serializable]
public class TypeSprites
{
    public IngredientType type;
    public Sprite sprite;
}

