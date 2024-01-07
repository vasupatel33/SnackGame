using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float steerSpeed = 150.0f;
    public Joystick joystick; // Reference to your virtual joystick component

    void Update()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Calculate the movement based on joystick input
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Apply movement
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the player based on the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, steerSpeed * Time.deltaTime);
        }
    }
}
