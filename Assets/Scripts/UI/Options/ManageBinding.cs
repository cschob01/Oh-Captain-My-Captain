using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ManageBinding : MonoBehaviour
{
    private Button button;
    [SerializeField] private InputActionReference action;
    private TMP_Text buttonText;
    [SerializeField] private bool allowRebinding = true;

    private void Awake()
    {
        button = transform.Find("Button1").GetComponent<Button>();
        buttonText = button.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        InputHandler.Instance.PlayerInput.onControlsChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        InputHandler.Instance.PlayerInput.onControlsChanged -= Refresh;
    }

    public void Refresh(PlayerInput obj = null)
    {
        if (buttonText == null || action == null)
            return;

        buttonText.text = action.action.GetBindingDisplayString();
        if (buttonText.text == "") buttonText.text = "Unbound";
    }

    public void StartRebind()
    {
        if (!allowRebinding) return;

        buttonText.text = "Press a key...";

        action.action.Disable();
        action.action
            .PerformInteractiveRebinding()
            .OnComplete(op =>
            {
                op.Dispose();
                action.action.Enable();
                Refresh();
            })
            .Start();
    }
}