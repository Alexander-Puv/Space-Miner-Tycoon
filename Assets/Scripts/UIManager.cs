using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    // travel and close buttons
    private const string TRAVEL_BUTTON = "TravelButton";
    private const string CLOSE_BUTTON = "Close";
    // HUD
    private const string MONEY_LABEL = "MoneyLabel";
    private const string RESOURCES_LABEL = "ResourcesLabel";
    // Inventory
    private const string OPEN_INVENTORY_BUTTON = "OpenInventory";
    private const string INVENTORY_VISUAL = "Inventory";
    private const string INVENTORY_MONEY_LABEL = "InventoryMoneyLabel";

    [SerializeField] private UIDocument UIDoc;

    // travel and close buttons
    private Button travelButton;
    private Button closeButton;
    // HUD
    private Label moneyLabel;
    private Label resourcesLabel;
    // Inventory
    private Button openInventoryButton;
    private VisualElement inventoryVisual;
    private Label inventoryMoneyLabel;

    private float animationDuration = .5f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeUIElements();
        RegisterCallbacks();

        moneyLabel.text = inventoryMoneyLabel.text = Inventory.Instance.money.ToString("F0");
        resourcesLabel.text = Inventory.Instance.currentCapacity.ToString();
    }

    private void Start() {
        UpdateMoneyDisplay(Inventory.Instance.money);
        UpdateResourcesDisplay(Inventory.Instance.currentCapacity);
    }

    private void InitializeUIElements() {
        // travel and close buttons
        travelButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_BUTTON);
        closeButton = UIDoc.rootVisualElement.Q<Button>(CLOSE_BUTTON);
        // HUD
        moneyLabel = UIDoc.rootVisualElement.Q<Label>(MONEY_LABEL);
        resourcesLabel = UIDoc.rootVisualElement.Q<Label>(RESOURCES_LABEL);
        // Inventory
        openInventoryButton = UIDoc.rootVisualElement.Q<Button>(OPEN_INVENTORY_BUTTON);
        inventoryVisual = UIDoc.rootVisualElement.Q<VisualElement>(INVENTORY_VISUAL);
        inventoryMoneyLabel = inventoryVisual.Q<Label>(INVENTORY_MONEY_LABEL);

        inventoryVisual.style.display = DisplayStyle.None;
    }

    private void RegisterCallbacks() {
        travelButton.RegisterCallback<MouseEnterEvent>(OnTravelButtonHover);
        travelButton.RegisterCallback<MouseLeaveEvent>(OnTravelButtonUnhover);
        travelButton.RegisterCallback<ClickEvent>(OnTravelButtonClick);

        openInventoryButton.RegisterCallback<MouseEnterEvent>(OnTravelButtonHover);
        openInventoryButton.RegisterCallback<MouseLeaveEvent>(OnTravelButtonUnhover);
        openInventoryButton.RegisterCallback<ClickEvent>(OnInventoryButtonClick);

        closeButton.RegisterCallback<MouseEnterEvent>(OnCloseButtonHover);
        closeButton.RegisterCallback<MouseLeaveEvent>(OnCloseButtonUnhover);
        closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClick);
    }

    public void SetTravelButtonsDisplay(bool showMenu) {
        if (showMenu) {
            travelButton.style.display = DisplayStyle.None;
            closeButton.style.display = DisplayStyle.Flex;
        } else {
            travelButton.style.display = DisplayStyle.Flex;
            closeButton.style.display = DisplayStyle.None;
        }
    }

    public void SetInventoryElementsDisplay(bool showMenu) {
        if (showMenu) {
            openInventoryButton.style.display = DisplayStyle.None;
            closeButton.style.display = DisplayStyle.Flex;

            inventoryVisual.style.display = DisplayStyle.Flex;
        } else {
            openInventoryButton.style.display = DisplayStyle.Flex;
            closeButton.style.display = DisplayStyle.None;

            inventoryVisual.style.display = DisplayStyle.None;
        }
    }


    public void UpdateMoneyDisplay(float newValue) {
        float oldValue = float.Parse(moneyLabel.text);
        StartCoroutine(AnimateValue(moneyLabel, oldValue, newValue));
        StartCoroutine(AnimateValue(inventoryMoneyLabel, oldValue, newValue));
    }

    public void UpdateResourcesDisplay(int newValue) {
        int oldValue = int.Parse(resourcesLabel.text);
        StartCoroutine(AnimateValue(resourcesLabel, oldValue, newValue));
    }

    private IEnumerator AnimateValue(Label label, float startValue, float endValue) {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            label.text = Mathf.RoundToInt(currentValue).ToString();
            yield return null;
        }
        label.text = Mathf.RoundToInt(endValue).ToString();
    }


    private void OnTravelButtonHover(MouseEnterEvent e) {
        travelButton.style.scale = new StyleScale(new Scale(new Vector2(1.1f, 1.1f)));
    }

    private void OnTravelButtonUnhover(MouseLeaveEvent e) {
        travelButton.style.scale = new StyleScale(new Scale(Vector2.one));
    }

    private void OnTravelButtonClick(ClickEvent e) {
        TravelMenu.Instance.ShowMenu();
    }

    private void OnInventoryButtonClick(ClickEvent e) {
        SetInventoryElementsDisplay(true);
    }

    private void OnCloseButtonHover(MouseEnterEvent e) {
        //////////////////////////
    }

    private void OnCloseButtonUnhover(MouseLeaveEvent e) {
        //////////////////////////
    }

    private void OnCloseButtonClick(ClickEvent e) {
        if (inventoryVisual.style.display == DisplayStyle.Flex) {
            SetInventoryElementsDisplay(false);
        } else {
            TravelMenu.Instance.HideMenu();
        }
    }
}
