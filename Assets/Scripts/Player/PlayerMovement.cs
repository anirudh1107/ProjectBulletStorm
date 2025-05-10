using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; } // Singleton instance of PlayerMovement

    [SerializeField]
    private float movementSpeedHorizontal = 5f; // Speed of the player movement
    [SerializeField]
    private float movementSpeedVertical = 5f; // Speed of the player movement

    private Rigidbody playerRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        // Get input from the user
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = MousePos - this.transform.position; // Calculate the direction from the player to the mouse position
        Direction.Normalize(); // Normalize the direction vector
        this.transform.up = Direction; // Rotate the player to face the mouse position

        // Create a new Vector3 for movement
        Vector3 movement = new Vector3(moveHorizontal * movementSpeedHorizontal, moveVertical * movementSpeedVertical,0.0f );

        // Move the player using Rigidbody
        this.transform.Translate(movement * Time.deltaTime, Space.World);
    }
}
