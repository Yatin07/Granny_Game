using UnityEngine;
using TMPro;

public class PlayerKeyPickup : MonoBehaviour
{
    [Header("Key Detection Settings")]
    public LayerMask keyLayer;          // Layer for key objects
    public float pickupRange = 3f;      // How close the player must be to pick up
    private Camera playerCamera;        // Automatically finds your child camera

    [Header("UI Settings")]
    public TMP_Text keyCountText;       // UI text to show key count
    public TMP_Text pickupHintText;     // UI text: "Press E to pick up"

    private int keysCollected = 0;      // How many keys player has picked
    private int totalKeys = 3;          // Total number of keys in the level
    private GameObject currentKey;      // Key currently being looked at

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            UnityEngine.Debug.LogError("❌ No Camera found in children! Assign manually in the inspector.");
        }

        if (pickupHintText != null)
            pickupHintText.gameObject.SetActive(false);

        UpdateKeyCountUI();
    }

    private void Update()
    {
        DetectAndPickupKey();
    }

    private void DetectAndPickupKey()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        bool keyInSight = false;

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, keyLayer))
        {
            if (hit.collider.CompareTag("Key"))
            {
                keyInSight = true;
                currentKey = hit.collider.gameObject;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CollectKey();
                }
            }
        }

        if (pickupHintText != null)
            pickupHintText.gameObject.SetActive(keyInSight);
    }

    private void CollectKey()
    {
        keysCollected++;
        UpdateKeyCountUI();

        if (currentKey != null)
        {
            AudioSource audio = currentKey.GetComponent<AudioSource>();
            if (audio != null)
                audio.Play();

            Destroy(currentKey, 0.2f);
            currentKey = null;
        }

        UnityEngine.Debug.Log($"🔑 Key collected! ({keysCollected}/{totalKeys})");

        if (pickupHintText != null)
            pickupHintText.gameObject.SetActive(false);

        GrannyAI granny = FindObjectOfType<GrannyAI>();
        if (granny != null)
            granny.HearSound(transform.position);
    }

    private void UpdateKeyCountUI()
    {
        if (keyCountText != null)
        {
            keyCountText.text = $"Keys: {keysCollected}/{totalKeys}";
        }
    }

    public bool HasEnoughKeys(int requiredKeys)
    {
        return keysCollected >= requiredKeys;
    }

}
