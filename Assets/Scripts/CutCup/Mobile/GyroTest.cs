using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroTest : MonoBehaviour
{
    private Vector3 rot;
    public float maxRotationAngle = 50;
    public float rotationSpeed = 5f; // Adjust this value to control the rotation speed.

    private Quaternion initialRotation;
    private Quaternion previousGyroRotation;
    public float offSetRotation;

    // Start is called before the first frame update
    void Start()
    {
        rot = Vector3.zero;
        Input.gyro.enabled = true;

        initialRotation = transform.rotation;
        previousGyroRotation = Input.gyro.attitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.gyro.enabled)
        {
            Quaternion gyroRotation = Input.gyro.attitude;
            gyroRotation.x *= -1; // Adjust the gyroscope input to match the Unity coordinate system.
            gyroRotation.z *= -1; // Adjust the gyroscope input to match the Unity coordinate system.

            // Calculate the change in rotation since the last frame.
            Quaternion deltaRotation = Quaternion.Inverse(previousGyroRotation) * gyroRotation;

            // Calculate the desired rotation based on the change in rotation and the offset.
            Vector3 desiredEulerAngles = new Vector3(
                Mathf.Clamp(Mathf.DeltaAngle(deltaRotation.eulerAngles.y, 0 - offSetRotation), -maxRotationAngle, maxRotationAngle),
                Mathf.Clamp(Mathf.DeltaAngle(deltaRotation.eulerAngles.x, 0), -maxRotationAngle, maxRotationAngle),
                0
            );

            Quaternion desiredRotation = Quaternion.Euler(desiredEulerAngles);

            // Use Quaternion.Lerp to smoothly rotate towards the desired rotation.
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation * desiredRotation, Time.deltaTime * rotationSpeed);

            // Update the previous gyro rotation with the current gyro rotation for the next frame.
            previousGyroRotation = gyroRotation;

        }
    }
}
