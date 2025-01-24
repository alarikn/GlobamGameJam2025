using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    public RectTransform image_panel;
    public GameObject image_prefab;
    public TextMeshProUGUI text;

    public Sprite test_Sprite; // This is only for testing

    public void Start()
    {
        newOrder(new Sprite[]{ test_Sprite, test_Sprite, test_Sprite ,test_Sprite }, 100); // This can be commented, used to show how it is used
    }

    public void newOrder(Sprite[] sprites, int score)
    {
        // Destroy all previous images
        for (int i = image_panel.childCount - 1; i >= 0; i--)
        {
            Destroy(image_panel.GetChild(i).gameObject);
        }

        // Create new images
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject new_image = Instantiate(image_prefab);
            new_image.GetComponent<Image>().sprite = sprites[i];
            new_image.transform.SetParent(image_panel.transform);
            new_image.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Update the score
        text.text = score.ToString();
    }
}
