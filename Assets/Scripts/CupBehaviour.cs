using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private DrinkNameGenerator drinkNameGenerator;

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
        if (other.transform.root.gameObject.TryGetComponent(out IngredientBehavior ingredientBehavior))
        {
            if (ingredientBehavior.Ingredient == null)
                return;

            if (addedIngredients.Count >= 4)
                return;

            var ing = ingredientBehavior.Ingredient;

            var last = addedIngredients.Count + 1 > 3;
            CheckSpecial(ing, last);

            addedIngredients.Add(ing);
            drinkNameGenerator.addedIngredient(ing);
            inventoryManager.Discard(ing);
            Destroy(ingredientBehavior.gameObject);

            //Update these yes
            if (ingredientBehavior.Ingredient.AddsColor)
            {
                liquidBehaviour.AddItem(ingredientBehavior.Ingredient.ColorToAdd, ingredientBehavior.Ingredient.IngredientType);
            }
            else
            {
                liquidBehaviour.AddItem(ingredientBehavior.Ingredient.IngredientType);
            }
            cupAnimator.Play("AddIngredient", 0, 0);

            if (last)
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

    public void CheckSpecial(Ingredient ing, bool last)
    {
        switch (ing.SpecialMove)
        {
            case SpecialMove.MindControl:
                current_customer.TriggerMindControl(ing.Score);
                break;
            case SpecialMove.Toxic:
                foreach (var added in addedIngredients)
                {
                    added.IngredientLand = ing.LandTarget;
                }
                break;
            default:
                break;
        }

        if (!last)
        {
            switch (ing.SpecialMove)
            {
                case SpecialMove.Draw:
                    inventoryManager.TriggerDraw(ing.Score);
                    break;
            }
        }

    }

    public IEnumerator CheckScoreRoutine()
    {
        var wait = new WaitForSeconds(0.35f);
        inventoryManager.DiscardOnTable();

        cupAnimator.Play("CloseLid", 0, 0);

        var scoring = (add: 0, multi: 1);

        foreach (var ing in addedIngredients.Where(x => x.SpecialMove != SpecialMove.None))
        {
            /*
            switch (ing.SpecialMove)
            {
                case SpecialMove.Toxic:
                    addedIngredients[0].IngredientLand = ing.LandTarget;
                    yield return wait;
                    break;
            }
            */
        }

        foreach (var ing in addedIngredients)
        {
            ing.EvaluatePoints(addedIngredients, out int add, out int multi);
            scoring.add += add;
            scoring.multi += multi;

            scoringT.text = $"{scoring.add} x {scoring.multi}";
            yield return wait;
        }

        var score = scoring.add * scoring.multi;
        scoringT.text = $"{scoring.add} x {scoring.multi} = {score}";

        yield return wait;

        yield return StartCoroutine(current_customer.checkOrder(addedIngredients, score));

        yield return new WaitForSeconds(1.0f);
        drinkNameGenerator.clearWords();
        cupAnimator.Play("ServeDrink", 0, 0);
        yield return wait;
        current_customer.Visualizer.RemoveCustomerVisuals();
        yield return wait;

        if (customer_number >= customers_in_a_day)
        {
            current_customer.ThoughtBubble.gameObject.SetActive(false);
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
            current_customer.ThoughtBubble.gameObject.SetActive(true);
            customer_number = 1;
        }
        else
        {
            customer_number++;
        }

        addedIngredients.Clear();
        current_customer.ThoughtBubble.HideMultipliedScoreText();
        current_customer.newOrder(day);
        inventoryManager.SpawnNewIngredients();
        trigger.enabled = true;
        AudioManager.Instance.PlaySoundEffect("SpawnIngredients");
        cupAnimator.Play("Idle", 0, 0);
        liquidBehaviour.ResetLiquid();
    }
}
