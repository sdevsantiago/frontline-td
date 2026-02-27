using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public RectTransform creditsText;
    public float scrollSpeed = 50f;
    public float endYPosition = 2000f;

    private Vector2 startPosition;
    private bool isRunning = true;

    void Start()
    {
        isRunning = true;
        startPosition = creditsText.anchoredPosition;
    }

    void Update()
    {
        if (!isRunning)
            return;

        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= endYPosition)
            ResetCredits();
    }

    void ResetCredits()
    {
        isRunning = false;
        creditsText.anchoredPosition = startPosition;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void StartCredits()
    {
        creditsText.anchoredPosition = startPosition;
        isRunning = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
