using UnityEngine;

public class onHover : MonoBehaviour
{
    public GameObject infoPannel;
    void OnMouseEnter()
    {
        Debug.Log("Mouse is over GameObject.");
        infoPannel.SetActive(true);
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse is no longer on GameObject.");
        infoPannel.SetActive(false);
    }
}
