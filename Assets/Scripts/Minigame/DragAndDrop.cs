using UnityEngine;

// Simple drag-and-drop for 3D objects using the mouse or touch.
[RequireComponent(typeof(Collider))]
public class DragAndDrop : MonoBehaviour
{
    public Camera dragCamera; // if null, Camera.main is used
    private bool dragging = false;
    private Vector3 offset;
    private Plane dragPlane;

    void Awake()
    {
        if (dragCamera == null) dragCamera = Camera.main;
    }

    void OnMouseDown()
    {
        StartDrag(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        if (dragging)
            DragTo(Input.mousePosition);
    }

    void OnMouseUp()
    {
        if (dragging)
            EndDrag();
    }

    void StartDrag(Vector3 screenPos)
    {
        if (dragCamera == null) return;
        // create a plane facing camera through object
        dragPlane = new Plane(-dragCamera.transform.forward, transform.position);
        Ray ray = dragCamera.ScreenPointToRay(screenPos);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            offset = transform.position - hit;
        }
        dragging = true;
        // optional: disable physics while dragging
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void DragTo(Vector3 screenPos)
    {
        Ray ray = dragCamera.ScreenPointToRay(screenPos);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            transform.position = hit + offset;
        }
    }

    void EndDrag()
    {
        dragging = false;
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        // Notify piece if contains RepairPiece
        var piece = GetComponent<RepairPiece>();
        if (piece != null)
            piece.OnReleased();
    }
}
