using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DisplayControlGuidance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private float WaitTime;
    [SerializeField] private float FadeTime;
    [SerializeField] private bool ShowBasics;

    [System.Serializable]
    public class BindingReplacement
    {
        public string placeHolder = "@";
        public InputActionReference binding;
    };

    [System.Serializable]
    public class MiniPrompt
    {
        public BindingReplacement[] bindingReplacements;
        public string prompt;
    };

    [System.Serializable]
    public class Prompt
    {
        [HideInInspector] public bool shown = false;
        [HideInInspector] public bool active = false;

        public MiniPrompt[] miniPrompts;
    };

    [Header("Movement/Interact")]
    [SerializeField] private Prompt OffRipText;

    [Header("Gun Controls")]
    [SerializeField] private Prompt FirstGunText;
    [SerializeField] private Prompt SecondGunText;
    [SerializeField] private Prompt ThirdGunText;

    [Header("Gadget Controls")]
    [SerializeField] private Prompt FirstGadgetText;
    [SerializeField] private Prompt SecondGadgetText;


    [Header("Ship Controller")]
    [SerializeField] private Prompt ControlShip;
    [SerializeField] private Gadget ControlGadget;

    private int prev_gun_count;
    private int prev_gadget_count;
    private bool Used = true;

    private void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();
        if (Text == null) Debug.Log("ERROR: TextMeshProUGUI component not found in DisplayControlGuidance");
        Text.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(WaitUntillActive());
    }

    private IEnumerator WaitUntillActive()
    {
        yield return new WaitForSeconds(2f);
        Used = false;
        OffRipText.active = ShowBasics;
    }

    private void Update()
    {
        UpdateDisplay();

        if (CaptainHandler.Instance == null) return;

        if (CaptainHandler.Instance.Guns.Count == 1) FirstGunText.active = true;
        if (CaptainHandler.Instance.Guns.Count == 2) SecondGunText.active = true;
        if (CaptainHandler.Instance.Guns.Count == 3) ThirdGunText.active = true;

        if (CaptainHandler.Instance.Gadgets.Count == 1) FirstGadgetText.active = true;
        if (CaptainHandler.Instance.Gadgets.Count == 2) SecondGadgetText.active = true;

        if (CaptainHandler.Instance.Gadgets.Count != 0)
        {
            if (CaptainHandler.Instance.Gadgets[CaptainHandler.Instance.CurrGadget].name.Replace("(Clone)", "") 
                == ControlGadget.name.Replace("(Clone)", ""))
            {
                ControlShip.active = true;
            }
            else
            {
                ControlShip.active = false;
                ControlShip.shown = false;
            }
        }
        else
        {
            ControlShip.active = false;
            ControlShip.shown = false;
        }

    }

    private void UpdateDisplay()
    {
        if (OffRipText.active && !OffRipText.shown && !Used) ShowPrompt(OffRipText);

        if (FirstGunText.active && !FirstGunText.shown && !Used) ShowPrompt(FirstGunText);
        if (SecondGunText.active && !SecondGunText.shown && !Used) ShowPrompt(SecondGunText);
        if (ThirdGunText.active && !ThirdGunText.shown && !Used) ShowPrompt(ThirdGunText);

        if (FirstGadgetText.active && !FirstGadgetText.shown && !Used) ShowPrompt(FirstGadgetText);
        if (SecondGadgetText.active && !SecondGadgetText.shown && !Used) ShowPrompt(SecondGadgetText);

        if (ControlShip.active && !ControlShip.shown && !Used) ShowPrompt(ControlShip);
    }

    private void ShowPrompt(Prompt prompt)
    {
        Used = true;
        prompt.shown = true;
        string display = ConvertPrompt(prompt);
        if (display != null && display != "") StartCoroutine(Use(display));
        else Used = false;
    }

    private string ConvertPrompt(Prompt prompt)
    {
        string display = "";
        foreach (var miniPrompt in prompt.miniPrompts)
        {
            string miniDisplay = miniPrompt.prompt;

            bool bindingFound = false;
            foreach (var br in miniPrompt.bindingReplacements)
            {
                string binding = InputHandler.Instance.GetBinding(br.binding);
                if (binding != "" && binding != null)
                {
                    bindingFound = true;
                    miniDisplay = miniDisplay.Replace(br.placeHolder, InputHandler.Instance.GetBinding(br.binding.action));
                }
            }
            if (bindingFound) display += miniDisplay + "\n";
        }
        return display;
    }

    private IEnumerator Use(string DisplayText)
    {
        
        Text.text = DisplayText;
        Text.DOFade(1f, FadeTime);
        yield return new WaitForSeconds(FadeTime + WaitTime);
        Text.DOFade(0f, FadeTime);
        yield return new WaitForSeconds(FadeTime + .5f);

        Used = false;
    }

}
