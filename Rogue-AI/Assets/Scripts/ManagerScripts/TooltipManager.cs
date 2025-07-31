using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [Header("Ability Tooltip Variablse")] 
    [SerializeField] private GameObject abilityTooltip;
    [SerializeField] private TextMeshProUGUI abilityTooltipText;
    
    public static TooltipManager Instance;

    private bool _isFacingRight;
    

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
        ChangeTooltipDirection(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        
        transform.position = mousePos;
        
        ChangeTooltipDirection(mousePos.x < Screen.width * 14f / 25f);
    }

    private readonly Vector2 _leftTooltip = new Vector2(1,0);
    private readonly Vector2 _rightTooltip = new Vector2(0,0);
    private void ChangeTooltipDirection(bool pointingRight)
    { 
        if (pointingRight == _isFacingRight) return;
        Vector2 direction = pointingRight ? _rightTooltip : _leftTooltip;

        _isFacingRight = pointingRight;
        
        rectTransform.pivot = direction;
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
    
    public void SetAndShowAbilityTooltip(string tooltip)
    {
        abilityTooltip.SetActive(true);
        abilityTooltipText.text = tooltip;
    }

    public void HideAbilityTooltip()
    {
        abilityTooltip.SetActive(false);
        abilityTooltipText.text = "";
    }
}
