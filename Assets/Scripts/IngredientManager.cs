using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientManager", menuName = "Ingredients/IngredientManager")]
public class IngredientManager : ScriptableObject
{
    [SerializeField] private IngredientDeck baseDeck;
    [SerializeField] private List<IngredientDeck> decks;

    public List<IngredientDeck> Decks { get => decks; }
    public IngredientDeck BaseDeck { get => baseDeck; }
    public static Dictionary<IngredientLand, Color> LandColors { get; private set; }

    public void Init()
    {
        LandColors = new Dictionary<IngredientLand, Color>();

        foreach (var deck in Decks)
        {
            if (!LandColors.ContainsKey(deck.Land))
            {
                LandColors.Add(deck.Land, deck.Color);
            }
        }
    }


}
