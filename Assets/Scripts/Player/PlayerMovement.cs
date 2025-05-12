using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement Instance { get; private set; } // Singleton instance of PlayerMovement
    public LayerMask gateLayer;

    [SerializeField]
    private float movementSpeedHorizontal = 5f; // Speed of the player movement
    [SerializeField]
    private float movementSpeedVertical = 5f; // Speed of the player movement

    private Rigidbody playerRb;
    private float xMaxLimit = 19f;
    private float xMinLimit = -2.5f;
    private float yMaxLimit = 4f;
    private float yMinLimit = -6.5f;

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

        if (Input.GetKey(KeyCode.Space))
        {
            bool isOnGate = Physics2D.OverlapCircle(transform.position, 2f, gateLayer);
            if (isOnGate)
            {
                // Call the method to open the gate
                LevelManager.LoadNextLevel();
            }
        }

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Direction = MousePos - this.transform.position; // Calculate the direction from the player to the mouse position
        Direction.Normalize(); // Normalize the direction vector
        this.transform.up = Direction; // Rotate the player to face the mouse position

        // Create a new Vector3 for movement
        Vector3 movement = new Vector3(moveHorizontal * movementSpeedHorizontal, moveVertical * movementSpeedVertical,0.0f );
        // Move the player using Rigidbody
        this.transform.Translate(movement * Time.deltaTime, Space.World);
        // Clamp the player's position to the defined limits
        if (this.transform.position.x > xMaxLimit)
        {
            this.transform.position = new Vector3(xMaxLimit, this.transform.position.y, this.transform.position.z);
        }
        else if (this.transform.position.x < xMinLimit)
        {
            this.transform.position = new Vector3(xMinLimit, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.y > yMaxLimit)
        {
            this.transform.position = new Vector3(this.transform.position.x, yMaxLimit, this.transform.position.z);
        }
        else if (this.transform.position.y < yMinLimit)
        {
            this.transform.position = new Vector3(this.transform.position.x, yMinLimit, this.transform.position.z);
        }
    }
}
