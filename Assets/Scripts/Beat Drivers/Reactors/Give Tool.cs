using UnityEngine;

public class GiveTool : MonoBehaviour
{
    [System.Serializable]
    public class Tool
    {
        public string beat;
        public Gun gun;
        public Gadget gadget;
    }

    [SerializeField] private Tool[] Tools;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += GivePlayerTool;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= GivePlayerTool;
    }

    private void GivePlayerTool(string Beat)
    {
        if (CaptainHandler.Instance == null)
        {
            Debug.Log("No Captain to give a tool to.");
            return;
        }

        for (int i = 0; i < Tools.Length; i++)
        {
            if (Tools[i].beat == Beat)
            {
                if (Tools[i].gun != null) CaptainHandler.Instance.AddGun(Tools[i].gun);
                if (Tools[i].gadget != null) CaptainHandler.Instance.AddGadget(Tools[i].gadget);
            }
        }
    }
}
