using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Ingredients/Deck")]
public class IngredientDeck : ScriptableObject
{
    [SerializeField] private List<IngredientObject> ingredients = new();
    [SerializeField] private IngredientLand land = IngredientLand.White;
    [SerializeField] private Color color = Color.white;

    public List<IngredientObject> Ingredients { get => ingredients; }
    public Color Color { get => color; }
    public IngredientLand Land { get => land; }
}
