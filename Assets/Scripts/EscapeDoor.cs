using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EscapeDoor : MonoBehaviour
{
    [Header("Detection Settings")]
    public LayerMask doorLayer;              // Layer for door detection
    public float interactRange = 3f;         // How close player must be
    public KeyCode interactKey = KeyCode.I;  // Key to insert keys

    [Header("Key Requirement")]
    public int requiredKeys = 3;             // Number of keys needed

    [Header("UI")]
    public GameObject escapeUIPanel;         // Congratulations panel
    public TMP_Text doorHintText;            // TMP Text: "Press I to insert keys"

    private PlayerKeyPickup playerKeys;
    public Camera playerCamera;

    private GameObject currentDoor;          // Door currently looked at
    private bool unlocked = false;

    private void Start()
    {
        if (escapeUIPanel != null)
            escapeUIPanel.SetActive(false);

        if (doorHintText != null)
            doorHintText.gameObject.SetActive(false);

        // Find player and its camera
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerKeys = player.GetComponent<PlayerKeyPickup>();
            playerCamera = player.GetComponentInChildren<Camera>();
        }
        doorHintText.gameObject.SetActive(true);
        doorHintText.text = "TEST HINT";

    }

    private void Update()
    {
        if (unlocked || playerCamera == null) return;

        DetectDoorAndUnlock();
    }

    private void DetectDoorAndUnlock()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        bool doorInSight = false;

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, doorLayer))
        {
            UnityEngine.Debug.Log("Ray hit: " + hit.collider.name); // 👈 Add this
            if (hit.collider.CompareTag("EscapeDoor"))
            {
                doorInSight = true;
                currentDoor = hit.collider.gameObject;

                if (doorHintText != null)
                {
                    doorHintText.text = "Press 'I' to insert keys";
                    doorHintText.gameObject.SetActive(true);
                    UnityEngine.Debug.Log("Showing hint text!");
                }

                if (Input.GetKeyDown(interactKey))
                {
                    TryUnlockDoor();
                }
            }
        }

        if (doorHintText != null)
            doorHintText.gameObject.SetActive(doorInSight);
    }

    private void TryUnlockDoor()
    {
        if (playerKeys == null) return;

        if (playerKeys.HasEnoughKeys(requiredKeys))
        {
            unlocked = true;
            ShowEscapeScreen();
        }
        else
        {
            UnityEngine.Debug.Log("🔒 You don't have all keys yet!");
        }
    }

    private void ShowEscapeScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (escapeUIPanel != null)
            escapeUIPanel.SetActive(true);

        UnityEngine.Debug.Log("🎉 Door unlocked — You escaped!");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        UnityEngine.Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
