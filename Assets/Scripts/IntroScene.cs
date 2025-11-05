
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//using static System.Net.Mime.MediaTypeNames;

public class IntroPanelManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject introPanel;         // Panel that covers screen
    public Image panelBackground;         // Panel's background Image (for fade out)
    public TMP_Text instructionText1;     // First instruction text
    public TMP_Text instructionText2;     // Second instruction text

    [Header("Settings")]
    public KeyCode startKey = KeyCode.Return; // Press Enter to start
    public float secondTextDelay = 2f;        // Delay before showing second text
    public float fadeDuration = 1f;           // Fade time for text/panel
    public bool lockCursorAfterStart = true;  // Lock cursor when starting

    private bool introActive = true;

    void Start()
    {
        if (introPanel != null)
            introPanel.SetActive(true);

        // Start fully visible background, hidden texts
        if (panelBackground != null)
            SetAlpha(panelBackground, 1f);
        SetAlpha(instructionText1, 0f);
        SetAlpha(instructionText2, 0f);

        // Pause game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Begin intro sequence
        StartCoroutine(IntroSequence());
    }

    System.Collections.IEnumerator IntroSequence()
    {
        // Fade in first instruction
        yield return StartCoroutine(FadeText(instructionText1, 1f, fadeDuration));

        // Wait a bit before showing second instruction
        yield return new WaitForSecondsRealtime(secondTextDelay);

        // Fade in second instruction
        yield return StartCoroutine(FadeText(instructionText2, 1f, fadeDuration));
    }

    System.Collections.IEnumerator FadeText(TMP_Text text, float targetAlpha, float duration)
    {
        if (text == null) yield break;

        float startAlpha = text.color.a;
        float time = 0f;
        Color color = text.color;

        while (time < duration)
        {
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            text.color = color;
            time += Time.unscaledDeltaTime; // Use unscaled time (game paused)
            yield return null;
        }

        color.a = targetAlpha;
        text.color = color;
    }

    void Update()
    {
        if (introActive && Input.GetKeyDown(startKey))
        {
            StartCoroutine(FadeOutAndStartGame());
        }
    }

    System.Collections.IEnumerator FadeOutAndStartGame()
    {
        introActive = false;

        // Fade out entire panel background
        if (panelBackground != null)
            yield return StartCoroutine(FadeImage(panelBackground, 0f, fadeDuration));

        // Fade out both texts quickly
        yield return StartCoroutine(FadeText(instructionText1, 0f, 0.5f));
        yield return StartCoroutine(FadeText(instructionText2, 0f, 0.5f));

        // Disable panel and resume game
        if (introPanel != null)
            introPanel.SetActive(false);

        Time.timeScale = 1f;

        if (lockCursorAfterStart)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Debug.Log("✅ Intro finished — game started!");
    }

    System.Collections.IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float time = 0f;
        Color color = image.color;

        while (time < duration)
        {
            float t = time / duration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            image.color = color;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = targetAlpha;
        image.color = color;
    }

    void SetAlpha(TMP_Text text, float alpha)
    {
        if (text == null) return;
        Color c = text.color;
        c.a = alpha;
        text.color = c;
    }

    void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
