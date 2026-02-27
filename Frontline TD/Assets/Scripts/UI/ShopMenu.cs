using UnityEngine;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [Header("References")]
    /**
     * The component that displays the player's current money.
     */
    [SerializeField] private Animator animator;

    private float[] speeds = new float[] { 0.5f, 1f, 2f, 4f, 20f };
    private int speedSelector = 1;
    [SerializeField] private TextMeshProUGUI speedUpText;

    private bool isOpen = true;

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        animator.SetBool("isMenuOpen", isOpen);
    }

    public void SetSelectedTower(int selectedTower)
    {
        BuildManager.Instance.SetSelectedTower(selectedTower);
    }

    public void SpeedUp()
    {
        if (speedSelector < speeds.Length - 1)
            speedSelector++;
        else
            speedSelector = 0;
        speedUpText.text = "Speed x" + speeds[speedSelector];
        Time.timeScale = speeds[speedSelector];
    }

}
