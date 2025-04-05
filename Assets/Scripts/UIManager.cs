using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    private const string TRAVEL_BUTTON = "TravelButton";
    private const string TRAVEL_CLOSE_MENU_BUTTON = "CloseMenu";
    private const string MONEY_LABEL = "MoneyLabel";
    private const string RESOURCES_LABEL = "ResourcesLabel";

    [SerializeField] private UIDocument UIDoc;

    private Button travelButton;
    private Button travelCloseMenuButton;
    private Label moneyLabel;
    private Label resourcesLabel;

    private float animationDuration = .5f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeUIElements();
        RegisterCallbacks();
    }

    private void Start() {
        UpdateMoneyDisplay(Inventory.Instance.money);
        UpdateResourcesDisplay(Inventory.Instance.currentCapacity);
    }

    private void InitializeUIElements() {
        travelButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_BUTTON);
        travelCloseMenuButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_CLOSE_MENU_BUTTON);
        moneyLabel = UIDoc.rootVisualElement.Q<Label>(MONEY_LABEL);
        resourcesLabel = UIDoc.rootVisualElement.Q<Label>(RESOURCES_LABEL);
    }

    private void RegisterCallbacks() {
        travelButton.RegisterCallback<MouseEnterEvent>(OnTravelButtonHover);
        travelButton.RegisterCallback<MouseLeaveEvent>(OnTravelButtonUnhover);
        travelButton.RegisterCallback<ClickEvent>(OnTravelButtonClick);

        travelCloseMenuButton.RegisterCallback<MouseEnterEvent>(OnTravelCloseMenuButtonHover);
        travelCloseMenuButton.RegisterCallback<MouseLeaveEvent>(OnTravelCloseMenuButtonUnhover);
        travelCloseMenuButton.RegisterCallback<ClickEvent>(OnTravelCloseMenuButtonClick);
    }

    public void SetTravelButtonsDisplay(bool showMenu) {
        if (showMenu) {
            travelButton.style.display = DisplayStyle.None;
            travelCloseMenuButton.style.display = DisplayStyle.Flex;
        } else {
            travelButton.style.display = DisplayStyle.Flex;
            travelCloseMenuButton.style.display = DisplayStyle.None;
        }
    }

    public void UpdateMoneyDisplay(float newValue) {
        float oldValue;
        if (!float.TryParse(resourcesLabel.text, out oldValue)) {
            oldValue = 0;
        }
        StartCoroutine(AnimateValue(moneyLabel, oldValue, newValue));
    }

    public void UpdateResourcesDisplay(int newValue) {
        int oldValue;
        if (!int.TryParse(resourcesLabel.text, out oldValue)) {
            oldValue = 0;
        }
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

    private void OnTravelCloseMenuButtonHover(MouseEnterEvent e) {
        //////////////////////////
    }

    private void OnTravelCloseMenuButtonUnhover(MouseLeaveEvent e) {
        //////////////////////////
    }

    private void OnTravelCloseMenuButtonClick(ClickEvent e) {
        TravelMenu.Instance.HideMenu();
    }
}
