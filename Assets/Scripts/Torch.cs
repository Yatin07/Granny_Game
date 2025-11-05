
using UnityEngine;

public class Torch : MonoBehaviour
{
    [Header("References")]
    public Light torchLight;              // The Light component for the torch
    public Transform followTarget;        // Usually the camera or hand

    [Header("Follow Settings")]
    public float positionFollowSpeed = 5f;
    public float rotationFollowSpeed = 5f;

    [Header("Input Settings")]
    public KeyCode toggleKey = KeyCode.F; // Press F to toggle torch

    private bool isOn = true;

    private void Start()
    {
        if (torchLight == null)
        {
            Debug.LogError("Torch Light not assigned!");
            return;
        }

        // Detach torch from parent (so it follows freely)
        torchLight.transform.parent = null;
    }

    private void Update()
    {
        // Toggle torch on/off
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            torchLight.enabled = isOn;
        }
    }

    private void LateUpdate()
    {
        if (torchLight == null || followTarget == null)
            return;

        // Smooth follow for position and rotation
        torchLight.transform.position = Vector3.Lerp(
            torchLight.transform.position,
            followTarget.position,
            positionFollowSpeed * Time.deltaTime
        );

        torchLight.transform.rotation = Quaternion.Lerp(
            torchLight.transform.rotation,
            followTarget.rotation,
            rotationFollowSpeed * Time.deltaTime
        );
    }
}
