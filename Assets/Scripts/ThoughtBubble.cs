using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI requiredScoreT;
    [SerializeField] private TextMeshProUGUI score_text;
    [SerializeField] private GameObject score_text_end_pos;

    [SerializeField] private Transform orderParent;
    [SerializeField] private GameObject preferredOrderPrefab;
    [SerializeField] private List<OrderPreference> orderPreferences;
    [SerializeField] private Customer customer;

    public List<Ingredient> OrderPreference => OrderPreferences.Select(x => x.Ingredient).ToList();
    public List<OrderPreference> OrderPreferences { get => orderPreferences; set => orderPreferences = value; }

    public int FinalScore { get; private set; }

    //[SerializeField] private 

    int required_score = 0;

    public void newOrder(List<Ingredient> ingredients, int score)
    {
        for (int i = 0; i < OrderPreferences.Count; i++)
        {
            Destroy(OrderPreferences[i].gameObject);
        }
        OrderPreferences.Clear();

        foreach (var ing in ingredients)
        {
            var obj = Instantiate(preferredOrderPrefab, orderParent);
            var preference = obj.GetComponent<OrderPreference>();
            OrderPreferences.Add(preference);
            preference.SetPreference(ing);
        }

        required_score = score;
        requiredScoreT.text = required_score.ToString();
    }

    public IEnumerator CheckOrder(List<Ingredient> addedIngredients, float base_score, CustomerVisualizer customerVisualizer)
    {
        yield return new WaitForSeconds(0.5f);
        //score_text.text = base_score.ToString();
        //score_text.gameObject.SetActive(true);
        float multiplied_score = base_score;
        var preferences = OrderPreference;

        // Multiply the score
        for (int i = 0; i < preferences.Count; i++)
        {
            var orderObj = orderPreferences[i];
            var preferenceIngredient = preferences[i];

            bool wasUsed = addedIngredients.Any(x => x.IngredientName == preferenceIngredient.IngredientName);

            float current_multiplier = 2f;

            if (wasUsed)
            {
                // Multiply score
                multiplied_score *= current_multiplier;
                current_multiplier = 1f + ((current_multiplier - 1f) / 2f);

                //score_text.transform.position = orderObj.transform.position;
                //score_text.text = multiplied_score.ToString();

                yield return new WaitForSeconds(0.5f);
                ScoreManager.Instance.AddPreferenceMulti(current_multiplier);
            }
            orderObj.SetSuccess(wasUsed);
        }

        //score_text.transform.position = score_text_end_pos.transform.position;

        yield return new WaitForSeconds(1f);

        ScoreManager.Instance.FinalScoreTMP.text = multiplied_score.ToString();
        // Change color for the score text
        if (multiplied_score >= required_score)
        {
            requiredScoreT.color = Color.green;

            customerVisualizer.CustomerHappy();
        }
        else
        {
            requiredScoreT.color = Color.red;

            customerVisualizer.CustomerDisappointed();
        }

        FinalScore = (int)multiplied_score;
    }

    public void ResetColor()
    {
        requiredScoreT.color = Color.black;
    }

    public void HideMultipliedScoreText()
    {
        //score_text.gameObject.SetActive(false);
    }
}
