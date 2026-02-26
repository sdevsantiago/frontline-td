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

    private int segments = 100;
    private Color color = new Color(0.15f, 0.15f, 0.15f, 0.45f);

    private LineRenderer line;

    private static bool isSelectedPlot;

    void Start()
    {
        // store the original color of the plot
        startColor = spriteRenderer.color;

        line = gameObject.AddComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.loop = true;
        line.useWorldSpace = false;

        line.widthMultiplier = 0.05f;

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;
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
        // if there is already a tower built on this plot or no tower is selected, do nothing
        if (currentTower != null)
        {
            isSelectedPlot = !isSelectedPlot;
            if (isSelectedPlot)
                line.enabled = true;
            else
                line.enabled = false;
            return ;
        }
        isSelectedPlot = false;
        if (BuildManager.Instance.GetSelectedTower() == null)
            return;
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
        SelectedTower();
        line.enabled = false;
        BuildManager.Instance.SetSelectedTower(-1);
    }

    void SelectedTower()
    {
        float angle = 0f;
        float radius = currentTower.GetComponent<Unit>().GetTargetingRange();

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));
            angle += 360f / segments;
        }
    }
}
