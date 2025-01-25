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

    [SerializeField] private Animator cupAnimator;

    [SerializeField] private Customer current_customer;

    [SerializeField] private DayEndScreenScript dayEndScreenScript;
    public int customers_in_a_day = 2;
    private int day = 1;
    private int customer_number = 1;

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

        cupAnimator.Play("CloseLid",0,0);

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

        yield return new WaitForSeconds(1.0f);
        cupAnimator.Play("ServeDrink", 0, 0);
        yield return new WaitForSeconds(0.5f);
        current_customer.Visualizer.RemoveCustomerVisuals();
        yield return new WaitForSeconds(0.5f);

        if (customer_number >= customers_in_a_day)
        {
            current_customer.thoughtBubble.gameObject.SetActive(false);
            day++;
            dayEndScreenScript.gameObject.SetActive(true);
            dayEndScreenScript.EndDay(day);
        }
        else
        {
            NewCustomer(false);
        }
    }

    public void NewCustomer(bool new_day)
    {
        if (new_day)
        {
            dayEndScreenScript.gameObject.SetActive(false);
            current_customer.thoughtBubble.gameObject.SetActive(true);
            customer_number = 1;
        }
        else
        {
            customer_number++;
        }

        addedIngredients.Clear();
        current_customer.thoughtBubble.HideMultipliedScoreText();
        current_customer.newOrder();
        trigger.enabled = true;
        inventoryManager.SpawnNewIngredients();
        cupAnimator.Play("Idle", 0, 0);
    }
}
