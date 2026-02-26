using UnityEngine;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    [Header("References")]
    /**
     * The component that displays the player's current money.
     */
    [SerializeField] private Animator animator;

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

}
