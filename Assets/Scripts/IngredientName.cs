using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngredientName : MonoBehaviour
{
    public Transform[]child;
    public Transform parent;
    public string selectedChild;
    public int moveValue;

 
    public void ChildOneMove()
    {
        child[0].SetSiblingIndex(moveValue); 
    }

    public void ChildTwoMove()
    {
        child[1].SetSiblingIndex(moveValue);
    }

    public void ChildThreeMove()
    {
        child[2].SetSiblingIndex(moveValue);
    }
    
    public void ChildFourMove()
    {
        child[3].SetSiblingIndex(moveValue);
    }

    

}

