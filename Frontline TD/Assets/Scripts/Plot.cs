using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    /**
     * The SpriteRenderer component of this plot, used to change the plot's color when hovered.
     */
    [SerializeField] private SpriteRenderer spriteRenderer;
    /**
     * The color to change the plot to when the mouse is hovering over it.
     */
    [SerializeField] private Color hoverColor;

    /**
     * The tower that is currently built on this plot. Null if no tower is built.
     */
    private GameObject currentTower;
    
    /**
     * The original color of the plot, stored to reset the color when the mouse exits.
     */
    private Color startColor;

    void Start()
    {
        // store the original color of the plot
        startColor = spriteRenderer.color; 
    }

    void OnMouseEnter()
    {
        // change the plot's color when the mouse is hovering over this plot
        spriteRenderer.color = hoverColor;
    }

    void OnMouseExit()
    {
        // reset the plot's color when the mouse exits
        spriteRenderer.color = startColor;
    }

    void OnMouseDown()
    {
        // if there is already a tower built on this plot, do nothing
        if (currentTower != null) return ;

        // create a new tower on this plot
        Tower tower = BuildManager.Instance.GetSelectedTower();

        if (BuildManager.Instance.GetCurrentMoney() < tower.cost)
        {
            // do nothing if the player does not have enough money to build this tower
            Debug.Log("Not enough money to build this tower.");
            return ;
        }

        // reduce the player's money by the cost of the tower
        BuildManager.Instance.SpendMoney(tower.cost);

        // instantiate the selected tower on this plot
        currentTower = Instantiate(tower.prefab, transform.position, Quaternion.identity);
    }
}
