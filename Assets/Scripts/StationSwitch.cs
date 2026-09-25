using UnityEngine;
using UnityEngine.UI; 

public class StationSwitch : MonoBehaviour
{
    public Transform mainCamera;
    public float speed = 15f;

    public Transform[] targetPositions = new Transform[5];
    

    [Header("UI Buttons")]
    public Button[] stationButtons = new Button[5];

    private Transform currentTarget;
    private bool shouldmove = false;

void Update()
{
    if (shouldmove && mainCamera != null && currentTarget != null)
    {
        Vector3 safeDestination = new Vector3(currentTarget.position.x, currentTarget.position.y, -10f);

        mainCamera.position = Vector3.MoveTowards(mainCamera.position, safeDestination, speed * Time.deltaTime);

        if (mainCamera.position == safeDestination)
        {
            shouldmove = false;
        }
    }
}


    public void GoToPosition(int positionIndex)
    {
        if (positionIndex >= 0 && positionIndex < targetPositions.Length)
        {
            if (targetPositions[positionIndex] != null)
            {
                currentTarget = targetPositions[positionIndex];
                shouldmove = true; 
            }
        }
    }


    
    public void OnStation1Clicked() => GoToPosition(0);
    public void OnStation2Clicked() => GoToPosition(1);
    public void OnStation3Clicked() => GoToPosition(2);
    public void OnStation4Clicked() => GoToPosition(3);
    public void OnStation5Clicked() => GoToPosition(4);
}
