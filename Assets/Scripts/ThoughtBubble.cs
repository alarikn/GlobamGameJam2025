using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    public RectTransform image_panel;
    public GameObject image_prefab;
    public TextMeshProUGUI text;

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

    public void checkOrder(List<int> correct_image_indexes, float gotten_score)
    {
        // Change color for all the image outlines
        for (int i = 0; i < image_panel.childCount; i++)
        {
            if (correct_image_indexes.Contains(i))
            {
                instantated_images[i].color = Color.green;
            }
            else
            {
                instantated_images[i].color = Color.red;
            }
        }

        // Change color for the score text
        if (gotten_score >= required_score)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
    }
}
