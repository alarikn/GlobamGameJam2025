using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpawn : MonoBehaviour
{
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();
    Coroutine routine = null;

    public void SpawnIngredients(List<Ingredient> ingredients)
    {
        if (routine == null)
        {
            Debug.Log("Ing: " + ingredients.Count);
            var routine = StartCoroutine(SpawnIngredientsRoutine(ingredients));
        }
    }

    public IEnumerator SpawnIngredientsRoutine(List<Ingredient> ingredients)
    {
        Debug.Log("Ie");
        for (int i = 0; i < ingredients.Count; i++)
        {
            var ingredient = ingredients[i];
            var prefab = ingredient.Prefab ? ingredient.Prefab : defaultPrefab;
            var obj = GameObject.Instantiate(prefab, spawnPositions[i].position, Quaternion.identity);
            obj.GetComponent<IngredientBehavior>().SetIngredient(ingredient);
            yield return new WaitForSeconds(0.5f);
        }
        routine = null;
    }
}
