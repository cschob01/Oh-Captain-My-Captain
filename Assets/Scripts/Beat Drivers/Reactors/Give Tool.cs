using UnityEngine;

public class GiveGun : MonoBehaviour
{
    [SerializeField] string Beat;
    [SerializeField] Gun Gun;
    [SerializeField] Gadget Gadget;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += GivePlayerGun;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= GivePlayerGun;
    }

    private void GivePlayerGun(string Beat)
    {
        if (this.Beat == Beat)
        {
            if (Gun != null) CaptainHandler.Instance.AddGun(Gun);
            if (Gadget != null) CaptainHandler.Instance.AddGadget(Gadget);
        }
    }
}
