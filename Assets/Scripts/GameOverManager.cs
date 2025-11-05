
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI References")]
    public GameObject gameOverPanel;

    [Header("Audio Settings")]
    public AudioSource audioSource;       // For scream sound
    public AudioClip screamClip;          // Assign your scream clip in Inspector

    [Header("Cursor & Delay Settings")]
    public bool unlockCursorOnDeath = true;
    public float showDelay = 0.5f;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (unlockCursorOnDeath)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Disable all player movement scripts
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            foreach (var m in player.GetComponentsInChildren<MonoBehaviour>())
            {
                if (m == null) continue;
                string t = m.GetType().Name.ToLower();
                if (!t.Contains("ui") && !t.Contains("manager"))
                {
                    try { m.enabled = false; } catch { }
                }
            }
        }

        Invoke(nameof(ShowPanel), showDelay);
    }

    void ShowPanel()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // 🔊 Play the scream sound once when game over happens
        if (audioSource != null && screamClip != null)
        {
            audioSource.PlayOneShot(screamClip);
        }
        else
        {
            Debug.LogWarning("Missing AudioSource or ScreamClip on GameOverManager!");
        }
    }

    public void RestartGame() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
