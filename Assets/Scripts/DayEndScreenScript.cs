using System;
using TMPro;
using UnityEngine;

public class DayEndScreenScript : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float fade_speed = 0.5f;
    [SerializeField] private TextMeshProUGUI day_text;
    private bool new_day = false;
    private bool fade_in = false;
    [SerializeField] private StoreScript storeScript;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (new_day)
        {
            if (fade_in)
            {
                canvasGroup.alpha += fade_speed * Time.deltaTime;

                if (canvasGroup.alpha >= 1f)
                {
                    fade_in = false;
                }
            }
            else
            {
                canvasGroup.alpha -= fade_speed * Time.deltaTime;

                if (canvasGroup.alpha <= 0f)
                {
                    StartNewDay();
                }
            }
        }
    }

    public void EndDay(int day)
    {
        day_text.text = "Day " + day;
        new_day = true;
        fade_in = true;
        gameObject.SetActive(true);
    }

    private void StartNewDay()
    {
        new_day = false;
        storeScript.OpenStore();
        gameObject.SetActive(false);
    }
}
