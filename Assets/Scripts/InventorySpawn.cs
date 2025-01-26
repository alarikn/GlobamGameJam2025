using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySpawn : MonoBehaviour
{
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private List<Transform> spawnPositions = new();
    [SerializeField] private List<IngredientBehavior> spawned = new();
    [SerializeField] private Transform drawSpawn;
    [SerializeField] private float spawnDelay = 0.5f;
    [SerializeField] private CupBehavior cupBehavior;

    Coroutine routine = null;

    public List<IngredientBehavior> Spawned { get => spawned; }

    public void DrawIngredients(List<Ingredient> ingredients)
    {
        SpawnIngredients(ingredients, drawSpawn);
    }

    public void SpawnIngredients(List<Ingredient> ingredients, Transform spawn = null)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        routine = StartCoroutine(SpawnIngredientsRoutine(ingredients, spawn));
    }

    public IEnumerator SpawnIngredientsRoutine(List<Ingredient> ingredients, Transform spawn = null)
    {
        cupBehavior.Trigger.enabled = false;
        for (int i = 0; i < ingredients.Count; i++)
        {
            var ingredient = ingredients[i];
            var prefab = ingredient.Prefab ? ingredient.Prefab : defaultPrefab;

            var currentSpawn = spawn;
            if (spawn == null)
                currentSpawn = spawnPositions[i];

            var obj = GameObject.Instantiate(prefab, currentSpawn.position, Quaternion.identity);
            var ing = obj.GetComponent<IngredientBehavior>();
            ing.SetIngredient(ingredient);
            Spawned.Add(ing);
            yield return new WaitForSeconds(spawnDelay);
        }
        cupBehavior.Trigger.enabled = true;
        routine = null;
    }
}
