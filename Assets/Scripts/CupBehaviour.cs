using System.Collections.Generic;
using UnityEngine;

public class CupBehavior : MonoBehaviour
{
    [SerializeField] private List<Ingredient> addedIngredients = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");

        if (other.gameObject.TryGetComponent(out IngredientBehavior ingredientBehavior))
        {
            addedIngredients.Add(ingredientBehavior.Ingredient);
            ingredientBehavior.gameObject.SetActive(false);
        }
    }

}
