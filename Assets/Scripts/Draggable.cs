using UnityEngine;
using UnityEngine.Events;

public class Draggable : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private UnityEvent OnHover;
    [SerializeField] private UnityEvent OnHoverExit;
    [SerializeField] private float rightBoundary = 5;
    [SerializeField] private float leftBoundary = -5;
    [SerializeField] private float bottomBoundary = 0;


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
        OnHover?.Invoke();
    }

    private void OnMouseExit()
    {
        OnHoverExit?.Invoke();
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

        if (transform.position.y <= bottomBoundary)
        {
            ResetPos();
        }
        if (!(leftBoundary <= transform.position.x && transform.position.x <= rightBoundary))
        {
            ResetPos();
        }
    }

    private void ResetPos()
    {
        transform.position = ogPos;
        if (!rb.isKinematic)
            rb.linearVelocity = Vector3.zero;
    }
}
