using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    private const string TRAVEL_BUTTON = "TravelButton";
    private const string TRAVEL_CLOSE_MENU_BUTTON = "CloseMenu";

    [SerializeField] private UIDocument UIDoc;

    private Button travelButton;
    private Button travelCloseMenuButton;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeUIElements();
        RegisterCallbacks();
    }

    private void InitializeUIElements() {
        travelButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_BUTTON);
        travelCloseMenuButton = UIDoc.rootVisualElement.Q<Button>(TRAVEL_CLOSE_MENU_BUTTON);
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
