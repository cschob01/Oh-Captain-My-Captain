using Unity.VisualScripting;
using UnityEngine;

public class HandleGuns : MonoBehaviour
{
    public GameObject StartingGun;
    public InputHandler Handler;
    public float distanceFromPlayer = .1f;

    private GameObject obj = null;
    private Gun gun = null;

    public Vector2 dir;
    private void Awake()
    {
        //Set up gun using prefab
        EventHandler.Instance.GunChange(StartingGun);
    }

    private void OnEnable()
    {
        EventHandler.Instance.OnGunChange += GunChange;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnGunChange -= GunChange;
    }

    private void GunChange(GameObject Gun)
    {
        LoseGun();
        obj = Instantiate(Gun, transform.position, Quaternion.identity, transform);
        gun = obj.GetComponent<Gun>();
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
            if (Handler.controls.Gun.Attack.IsPressed()) gun.Fire();
            if (Handler.controls.Gun.Reload.WasPressedThisFrame()) gun.Reload();
        }
    }

    private void FixedUpdate()
    {
        if (obj != null && gun != null)
        {
            // Point gun
            ///////////////////////////////////////////////////////////////////
            Vector2 mouseScreen = Handler.controls.Player.Look.ReadValue<Vector2>();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = 0f;

            dir = (mouseWorld - transform.position).normalized;
            obj.transform.localPosition = dir * distanceFromPlayer;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            obj.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
