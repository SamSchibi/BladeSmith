using UnityEngine;
using UnityEngine.InputSystem;

public class GrindstoneMiG : MonoBehaviour
{
    private bool isDragging = false;
    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Mouse.current == null || myCollider == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z)));

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (myCollider.OverlapPoint((Vector2)mouseWorldPos))
            {
                isDragging = true;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            transform.position = new Vector3(
                mouseWorldPos.x,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
