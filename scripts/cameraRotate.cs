using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotate : MonoBehaviour
{
    public Transform target; // The target object to orbit around
    public float orbitSpeed = 5f; // Speed of orbit

    private float distance; // Distance from the target

    void Start()
    {
        // Calculate initial distance from the target
        distance = Vector3.Distance(transform.position, target.position);
    }

    void Update()
    {
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Orbit around the target based on mouse movement
        transform.RotateAround(target.position, Vector3.up, mouseX * orbitSpeed);
        transform.RotateAround(target.position, transform.right, -mouseY * orbitSpeed);

        // Ensure the distance to the target is maintained
        Vector3 desiredPosition = (transform.position - target.position).normalized * distance + target.position;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 5f);

        // Look at the target
        transform.LookAt(target);
    }
}
