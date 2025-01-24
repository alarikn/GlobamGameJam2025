using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientManager", menuName = "Ingredients/IngredientManager")]
public class IngredientManager : ScriptableObject
{
    [SerializeField] private IngredientDeck baseDeck;
    [SerializeField] private List<IngredientDeck> decks;

    public List<IngredientDeck> Decks { get => decks; }
    public IngredientDeck BaseDeck { get => baseDeck; }
}
