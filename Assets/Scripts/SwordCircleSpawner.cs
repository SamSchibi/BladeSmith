using UnityEngine;

public class SwordCircleSpawner : MonoBehaviour
{
    [Header("Setup")]
    public SpriteRenderer boundarySprite; // Drag your physical visual rectangle object here
    public GameObject prefabToSpawn;     // Drag your WorldTimingCirclePrefab here

    [Header("Game Settings")]
    public int totalCirclesToWin = 5; 
    private int circlesCleared = 0;   

    void Start()
    {
        SpawnRandomly();
    }

    public void SpawnRandomly()
    {
        if (boundarySprite == null || prefabToSpawn == null)
        {
            Debug.LogError("Please assign both the Boundary Sprite and Prefab on " + gameObject.name);
            return;
        }

        // 1. Get the local bounds of the sprite asset texture itself (ignores position/scale multipliers)
        Bounds localBounds = boundarySprite.sprite.bounds;

        // 2. FIXED PADDING LOGIC: Keep the padding proportional to the raw texture size
        // We take 10% of the raw texture size as a margin. This prevents the random range
        // window from collapsing to 0 regardless of how huge your object's inspector scale is.
        float paddingX = localBounds.size.x * 0.15f;
        float paddingY = localBounds.size.y * 0.15f;

        // 3. Define the safe minimum and maximum limits inside the localized asset box
        float minX = localBounds.min.x + paddingX;
        float maxX = localBounds.max.x - paddingX;
        float minY = localBounds.min.y + paddingY;
        float maxY = localBounds.max.y - paddingY;

        // 4. Select a random coordinate relative to the center of the rectangle sprite
        // Because min and max are wide apart now, this will generate a unique spot every time!
        float localRandomX = Random.Range(minX, maxX);
        float localRandomY = Random.Range(minY, maxY);
        Vector3 localSpawnPos = new Vector3(localRandomX, localRandomY, 0f);

        // 5. Convert that local position point directly into true World Space Coordinates!
        // This automatically accounts for where the sprite is moved, rotated, or scaled in your scene.
        Vector3 globalSpawnPosition = boundarySprite.transform.TransformPoint(localSpawnPos);

        // Make sure it sits slightly in front of the background on the Z axis
        globalSpawnPosition.z = boundarySprite.transform.position.z - 0.1f;

        // 6. Spawn the object cleanly out in the open world space
        // NEW LINE: This explicitly forces the object to spawn at the root level of your hierarchy
        GameObject newObj = Instantiate(prefabToSpawn, globalSpawnPosition, Quaternion.identity, null);
        newObj.transform.localScale = Vector3.one;

        // 7. Connect script reference
        WorldTimingCircle circleScript = newObj.GetComponent<WorldTimingCircle>();
        if (circleScript != null)
        {
            circleScript.SetupCircle(this); 
        }
    }

    public void OnCircleSuccess()
    {
        circlesCleared++; 

        if (circlesCleared >= totalCirclesToWin)
        {
            Debug.Log("🏆 Mini-Game Complete! You Won!");
        }
        else
        {
            SpawnRandomly();
        }
    }
}

