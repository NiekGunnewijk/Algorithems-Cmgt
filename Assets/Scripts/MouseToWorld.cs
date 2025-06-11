using UnityEngine;
using UnityEngine.Events;
public class MouseToWorld : MonoBehaviour
{
    Vector3 latestHit;
    public UnityEvent<Vector3> OnClick;

    void Update()
    {
        // Get the mouse click position in world space
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo))
            {
                Vector3 clickWorldPosition = hitInfo.point;
                latestHit = clickWorldPosition;

                OnClick.Invoke(clickWorldPosition);
            }
        }
        // drawing the Line
        if (latestHit != null && Camera.main != null)
        {
            Debug.DrawLine(Camera.main.transform.position, latestHit);
        }

    }
}
