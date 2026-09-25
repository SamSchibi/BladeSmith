using UnityEngine;
using UnityEngine.InputSystem; 

public class WorldTimingCircle : MonoBehaviour
{
    [Header("Setup")]
    public Transform fillingCircle; // Drag the "FillingCircle" child sprite here

    [Header("Game Settings")]
    public float targetDuration = 2.0f;    // Time in seconds to reach perfect size (Scale 1.0)
    public float perfectThreshold = 0.15f;  // Margin of error allowed (seconds)
    public float autoResetDelay = 0.3f;    // Grace period past 1.0 scale before auto-resetting

    private float currentTime = 0f;
    private bool isFullyFilled = false;
    private SwordCircleSpawner gameManager; 
    private Collider2D myCollider; 

    // Called by the spawner immediately after instantiation
    public void SetupCircle(SwordCircleSpawner manager)
    {
        gameManager = manager;
        myCollider = GetComponentInChildren<Collider2D>(); 
        ResetCircle();
    }

    void Update()
    {
        // 1. Advance the circle timer
        currentTime += Time.deltaTime;

        // 2. Scale the circle upward relative to target time
        float progress = currentTime / targetDuration;
        fillingCircle.localScale = new Vector3(progress, progress, 1f);

        // 3. Auto-Reset Check if player takes too long
        float maxAllowedTime = targetDuration + autoResetDelay;
        if (currentTime >= maxAllowedTime && !isFullyFilled)
        {
            isFullyFilled = true; 
            OnTimingMissed();
        }

        // 4. Listen for mouse clicks using the New Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ProcessPlayerClick();
        }
    }

    private void ProcessPlayerClick()
    {
        // Get screen position from New Input System
        Vector2 screenMousePos = Mouse.current.position.ReadValue();

        // Account for Camera depth translation offset (Z-depth fix)
        Vector3 mouseWithDepth = new Vector3(
            screenMousePos.x, 
            screenMousePos.y, 
            Mathf.Abs(Camera.main.transform.position.z) 
        );

        // Convert the modified coordinates to 2D World Coordinates
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseWithDepth);

        // If the mouse world coordinate overlays our own collider box, calculate success
        if (myCollider != null && myCollider.OverlapPoint(mouseWorldPosition))
        {
            EvaluateTiming();
        }
    }

    private void EvaluateTiming()
    {
        float timingDifference = Mathf.Abs(currentTime - targetDuration);

        if (timingDifference <= perfectThreshold)
        {
            Debug.Log("🎯 Perfect Hit!");
            gameManager.OnCircleSuccess(); 
            Destroy(gameObject); 
        }
        else
        {
            if (currentTime < targetDuration)
            {
                Debug.Log("❌ Too Early!");
            }
            else
            {
                Debug.Log("❌ Too Late!");
            }
            ResetCircle();
        }
    }

    private void OnTimingMissed()
    {
        Debug.Log("⏰ Timed Out! Automatically resetting.");
        ResetCircle();
    }

    private void ResetCircle()
    {
        currentTime = 0f;
        isFullyFilled = false;
        if (fillingCircle != null)
        {
            fillingCircle.localScale = Vector3.zero; 
        }
    }
}
