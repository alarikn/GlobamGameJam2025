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
        Debug.Log("trigger enter");

        if (other.gameObject.TryGetComponent(out IngredientBehavior ingredientBehavior))
        {
            addedIngredients.Add(ingredientBehavior.Ingredient);
            ingredientBehavior.gameObject.SetActive(false);

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
        var scoring = (add: 0, multi: 1);
        foreach (var ing in addedIngredients)
        {
            ing.EvaluatePoints(addedIngredients, out int add, out int multi);
            scoring.add += add;
            scoring.multi += multi;

            scoringT.text = $"{scoring.add} x {scoring.multi}";
            yield return new WaitForSeconds(0.5f);
        }

        scoringT.text = $"{scoring.add} x {scoring.multi} = {scoring.add * scoring.multi}";

        yield return new WaitForSeconds(0.5f);

        current_customer.checkOrder(addedIngredients, scoring.add * scoring.multi);

        yield return new WaitForSeconds(2f);

        addedIngredients.Clear();
        current_customer.newOrder();
        trigger.enabled = true;
        inventoryManager.spawnNewIngredients();
    }

    public void newCustomer(Customer new_customer)
    {
        current_customer = new_customer;
    }
}
