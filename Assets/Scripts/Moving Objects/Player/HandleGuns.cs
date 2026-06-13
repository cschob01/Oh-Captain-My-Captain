using Unity.VisualScripting;
using UnityEngine;

public class HandleGuns : MonoBehaviour
{
    [SerializeField] private GameObject StartingGun = null;
    [SerializeField] public float distanceFromPlayer = .1f;

    private GameObject obj = null;
    private Gun gun = null;

    public Vector2 dir;
    private void Start()
    {
        //Set up gun using prefab
        GunChange(StartingGun);
    }

    public void GunChange(GameObject Gun)
    {
        LoseGun();
        if (Gun != null)
        {
            obj = Instantiate(Gun, transform.position, Quaternion.identity, transform);
            gun = obj.GetComponent<Gun>();
        }
        EventHandler.Instance.GunChange(obj);
    }

    private void LoseGun()
    {
        if (obj != null) Destroy(obj);
        gun = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (obj != null && gun != null)
        {
            // Fire/Reload gun if requested
            if (InputHandler.Instance.FireIsPressed()) gun.Fire();
            if (InputHandler.Instance.ReloadWasPressedThisFrame()) gun.Reload();
        }
    }

    private void FixedUpdate()
    {
        if (obj != null && gun != null)
        {
            // Point gun
            ///////////////////////////////////////////////////////////////////
            Vector2 dir = InputHandler.Instance.LookReadValue();
            obj.transform.localPosition = dir * distanceFromPlayer;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
