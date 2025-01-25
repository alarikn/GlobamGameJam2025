using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private bool grabbed;
    private Vector3 ogPos;

    private void Start()
    {
        ogPos = transform.position;
    }

    private void OnMouseDown()
    {
        Grab();
    }

    private void OnMouseUp()
    {
        Ungrab();
    }
    private void OnMouseEnter()
    {
        //Hover
    }
    private void OnMouseExit()
    {
        //Hover exit
        //Ungrab();
    }

    public void Grab()
    {
        rb.isKinematic = true;
        transform.rotation = Quaternion.identity;
        grabbed = true;

    }
    public void Ungrab()
    {
        rb.isKinematic = false;
        grabbed = false;
    }

    public void LateUpdate()
    {
        if (grabbed)
        {
            var mouseInput = Input.mousePosition;
            mouseInput.z = 3.5f;
            var mousePos = Camera.main.ScreenToWorldPoint(mouseInput);
            transform.position = mousePos;
        }
    }
}
