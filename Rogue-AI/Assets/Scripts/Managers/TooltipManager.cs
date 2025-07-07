using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;
    
    [SerializeField] private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowTooltip(string tooltip)
    {
        gameObject.SetActive(true);
        tooltipText.text = tooltip;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltipText.text = "";
    }
}
