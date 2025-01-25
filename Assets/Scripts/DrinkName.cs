using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkName : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    public void setWord(string word, Color land_color)
    {
        text.text = word;
        background.color = land_color;
        LayoutRebuilder.ForceRebuildLayoutImmediate(background.GetComponent<RectTransform>());
    }
}
