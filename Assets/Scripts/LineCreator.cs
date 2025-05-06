using UnityEngine;

public class LineCreator : MonoBehaviour
{
    [SerializeField]
    public GameObject[] BoardPins;

    public Material material;
    public LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>(); // Get the LineRenderer component attached to this GameObject
                                                     // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.material = material; // Set the material for the line renderer

        int PinCount = BoardPins.Length; // Get the number of pins

        lineRenderer.positionCount = PinCount-1; // Set the number of points in the line

        for (int i = 0; i < BoardPins.Length-2; i++)
        {
            lineRenderer.SetPosition(i, BoardPins[i].transform.position); // Set the position of each point in the line
        }
        for (int i = BoardPins.Length - 2; i < BoardPins.Length; i++)
        {
            lineRenderer.SetPosition(i, BoardPins[i].transform.position); // Set the position of each point in the line
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawLines(GameObject[] currentBoardPins)
    {
        for (int i = 0; i < currentBoardPins.Length; i++)
        {
            lineRenderer.SetPosition(i, currentBoardPins[i].transform.position);
        }
    }
}
