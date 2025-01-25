using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CupBehavior : MonoBehaviour
{
    [SerializeField] private List<Ingredient> addedIngredients = new();
    [SerializeField] private Collider trigger;
    [SerializeField] private TMP_Text scoringT;
    [SerializeField] private InventoryManager inventoryManager;

    [SerializeField] private Customer current_customer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter: " + other.gameObject.name);

        if (other.transform.root.gameObject.TryGetComponent(out IngredientBehavior ingredientBehavior))
        {
            if (ingredientBehavior.Ingredient == null)
                return;

            addedIngredients.Add(ingredientBehavior.Ingredient);
            inventoryManager.Discard(ingredientBehavior.Ingredient);
            Destroy(ingredientBehavior.gameObject);
            //ingredientBehavior.gameObject.SetActive(false);

            if (addedIngredients.Count > 3)
            {
                CheckScore();
            }
        }
    }

    public void CheckScore()
    {
        trigger.enabled = false;
        StartCoroutine(CheckScoreRoutine());
    }

    public IEnumerator CheckScoreRoutine()
    {
        inventoryManager.DiscardOnTable();

        var scoring = (add: 0, multi: 1);
        foreach (var ing in addedIngredients)
        {
            ing.EvaluatePoints(addedIngredients, out int add, out int multi);
            scoring.add += add;
            scoring.multi += multi;

            scoringT.text = $"{scoring.add} x {scoring.multi}";
            yield return new WaitForSeconds(0.5f);
        }

        var score = scoring.add * scoring.multi;
        scoringT.text = $"{scoring.add} x {scoring.multi} = {score}";

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(current_customer.checkOrder(addedIngredients, score));

        yield return new WaitForSeconds(1.5f);
        current_customer.Visualizer.RemoveCustomerVisuals();
        yield return new WaitForSeconds(0.5f);


        addedIngredients.Clear();
        current_customer.newOrder();
        trigger.enabled = true;
        inventoryManager.SpawnNewIngredients();
    }

    public void newCustomer(Customer new_customer)
    {
        current_customer = new_customer;
    }
}
