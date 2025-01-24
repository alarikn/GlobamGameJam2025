using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Ingredients/Deck")]
public class IngredientDeck : ScriptableObject
{
    [SerializeField] private List<IngredientObject> ingredients = new();
    public List<IngredientObject> Ingredients { get => ingredients; }
}
