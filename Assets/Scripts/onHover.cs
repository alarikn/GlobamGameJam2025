using UnityEngine;

public class onHover : MonoBehaviour
{
    public GameObject infoPannel;
    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
        infoPannel.SetActive(true);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
        infoPannel.SetActive(false);
    }
}
