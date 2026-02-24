using UnityEngine;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [Header("References")]
    /**
     * The component that displays the player's current money.
     */
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Animator animator;

    private bool isOpen = true;

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        animator.SetBool("isMenuOpen", isOpen);
    }

    void OnGUI()
    {
        moneyText.text = BuildManager.Instance.GetCurrentMoney().ToString();
    }

    public void SetSelectedTower(int selectedTower)
    {
        BuildManager.Instance.SetSelectedTower(selectedTower);
    }

}
