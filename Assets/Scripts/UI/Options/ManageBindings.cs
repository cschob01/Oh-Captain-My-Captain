using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ManageBindings : MonoBehaviour
{
    [System.Serializable]
    public class Binding
    {
        public Button button;
        public InputActionReference action;
    };

    [SerializeField] private Binding[] Bindings;

    // Update is called once per frame
    void Update()
    {
        foreach (Binding binding in Bindings)
        {
            if (binding.button == null || binding.action == null)
                continue;

            TMP_Text text = binding.button.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.text = binding.action.action.GetBindingDisplayString();
            }
        }
    }

    private string GetGamepadBinding(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            string path = action.bindings[i].effectivePath;

            if (!string.IsNullOrEmpty(path) && path.Contains("<Gamepad>"))
            {
                return action.GetBindingDisplayString(i);
            }
        }

        return "Unbound";
    }
}
