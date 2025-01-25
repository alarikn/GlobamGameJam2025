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
    [SerializeField] private TextMeshProUGUI ingredient_Text;

    public int customers_in_a_day = 2;
    public int day = 1;
    private int customer_number = 1;
    private LiquidBehaviour liquidBehaviour;
    [SerializeField] StoreScript storeScript;
    [SerializeField] DayEndScreenScript dayEndScreenScript;

    private void Start()
    {
        liquidBehaviour = GetComponent<LiquidBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter: " + other.gameObject.name);

        if (other.transform.root.gameObject.TryGetComponent(out IngredientBehavior ingredientBehavior))
        {
            if (ingredientBehavior.Ingredient == null)
                return;

            var ing = ingredientBehavior.Ingredient;

            addedIngredients.Add(ing);
            DrinkName();
            inventoryManager.Discard(ing);
            Destroy(ingredientBehavior.gameObject);
            //ingredientBehavior.gameObject.SetActive(false);

            var last = addedIngredients.Count > 3;

            CheckSpecial(ing, last);

            if (ingredientBehavior.Ingredient.AddsColor)
            {
                liquidBehaviour.AddItem(ingredientBehavior.Ingredient.ColorToAdd);
            }
            else
            {
                liquidBehaviour.AddItem();
            }
            cupAnimator.Play("AddIngredient", 0, 0);

            if (addedIngredients.Count > 3)
            {
                CheckScore();
                return;
            }

        }
    }

    public void CheckScore()
    {
        trigger.enabled = false;
        StartCoroutine(CheckScoreRoutine());
    }

    public void CheckSpecial(Ingredient ing, bool last)
    {
        switch (ing.SpecialMove)
        {
            case SpecialMove.Draw:
                inventoryManager.TriggerDraw(ing.Score);
                break;
            case SpecialMove.MindControl:

                break;
            default:
                return;
        }
    }

    public IEnumerator CheckScoreRoutine()
    {
        inventoryManager.DiscardOnTable();

        cupAnimator.Play("CloseLid", 0, 0);

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
        ingredient_Text.text = "";
        cupAnimator.Play("ServeDrink", 0, 0);
        yield return new WaitForSeconds(0.5f);
        current_customer.Visualizer.RemoveCustomerVisuals();
        yield return new WaitForSeconds(0.5f);

        if (customer_number >= customers_in_a_day)
        {
            current_customer.thoughtBubble.gameObject.SetActive(false);
            day++;
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
            current_customer.thoughtBubble.gameObject.SetActive(true);
            customer_number = 1;
        }
        else
        {
            customer_number++;
        }

        addedIngredients.Clear();
        current_customer.thoughtBubble.HideMultipliedScoreText();
        current_customer.newOrder(day);
        trigger.enabled = true;
        inventoryManager.SpawnNewIngredients();
        cupAnimator.Play("Idle", 0, 0);
        liquidBehaviour.ResetLiquid();
    }

    private void DrinkName()
    {
        ingredient_Text.text = "";
        for (int i = 0; i < addedIngredients.Count; i++)
        {
            if (i != 0)
            {
                ingredient_Text.text += " ";
            }
            ingredient_Text.text += addedIngredients[i].CombinationName;
        }

        ingredient_Text.text += " tea";
    }
}
