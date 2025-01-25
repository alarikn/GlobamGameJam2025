using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpawn : MonoBehaviour
{
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private List<Transform> spawnPositions = new();
    [SerializeField] private List<IngredientBehavior> spawned = new();

    Coroutine routine = null;

    public List<IngredientBehavior> Spawned { get => spawned; }

    public void SpawnIngredients(List<Ingredient> ingredients)
    {
        if (routine == null)
        {
            var routine = StartCoroutine(SpawnIngredientsRoutine(ingredients));
        }
    }

    public IEnumerator SpawnIngredientsRoutine(List<Ingredient> ingredients)
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            var ingredient = ingredients[i];
            var prefab = ingredient.Prefab ? ingredient.Prefab : defaultPrefab;
            var obj = GameObject.Instantiate(prefab, spawnPositions[i].position, Quaternion.identity);
            var ing = obj.GetComponent<IngredientBehavior>();
            ing.SetIngredient(ingredient);
            Spawned.Add(ing);
            yield return new WaitForSeconds(0.5f);
        }
        routine = null;
    }
}
