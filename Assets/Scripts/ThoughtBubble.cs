using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    public RectTransform image_panel;
    public GameObject image_prefab;
    public TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI score_text;
    [SerializeField] private GameObject score_text_end_pos;

    private List<Image> instantated_images = new List<Image>();
    int required_score = 0;

    public void newOrder(List<Sprite> sprites, int score)
    {
        // Destroy all previous images
        for (int i = instantated_images.Count - 1; i >= 0; i--)
        {
            Destroy(instantated_images[i].transform.parent.gameObject);
        }
        instantated_images.Clear();

        // Create new images
        for (int i = 0; i < sprites.Count; i++)
        {
            GameObject new_image = Instantiate(image_prefab);
            new_image.GetComponent<Image>().sprite = sprites[i];
            new_image.transform.SetParent(image_panel.transform, false);
            new_image.transform.localScale = new Vector3(1f, 1f, 1f);
            new_image.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);

            new_image.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
            new_image.transform.GetChild(0).GetComponent<Image>().color = Color.black;

            new_image.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
            new_image.transform.GetChild(0).GetChild(0).localScale = new Vector3(0.9f, 0.9f, 0.9f);
            new_image.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprites[i];

            instantated_images.Add(new_image.transform.GetChild(0).GetComponent<Image>());
        }

        // Update the score
        required_score = score;
        text.text = required_score.ToString();
        text.color = Color.black;
    }

    public IEnumerator CheckOrder(List<Ingredient> used_ingredients, List<Ingredient> preferred_ingredients, float base_score, CustomerVisualizer customerVisualizer)
    {
        float multiplied_score = base_score;

        // Multiply the score
        for (int i = 0; i < preferred_ingredients.Count; i++)
        {
            bool was_used = false;
            foreach (Ingredient ingredient in used_ingredients)
            {
                float current_multipler = 2f;
                if (ingredient == preferred_ingredients[i])
                {
                    was_used = true;
                    instantated_images[i].color = Color.green;

                    // Multiply score
                    multiplied_score *= current_multipler;
                    current_multipler = 1f + ((current_multipler - 1f) / 2f);

                    score_text.transform.position = instantated_images[i].transform.position;
                    score_text.text = multiplied_score.ToString();

                    yield return new WaitForSeconds(0.5f);
                }
            }

            // Change color to red if was not used at all
            if (!was_used)
            {
                instantated_images[i].color = Color.red;
            }
        }

        score_text.transform.position = score_text_end_pos.transform.position;

        yield return new WaitForSeconds(1f);

        // Change color for the score text
        if (multiplied_score >= required_score)
        {
            text.color = Color.green;

            customerVisualizer.CustomerHappy();
        }
        else
        {
            text.color = Color.red;

            customerVisualizer.CustomerDisappointed();
        }
    }
}
