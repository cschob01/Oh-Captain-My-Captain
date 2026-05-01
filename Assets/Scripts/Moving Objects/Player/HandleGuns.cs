using UnityEngine;

public class HandleGuns : MonoBehaviour
{
    public GameObject gunPrefab;

    public GameObject obj = null;
    public Gun gun = null;
    public InputHandler Handler;

    public float distanceFromPlayer = .1f;

    public Vector2 dir;
    private void Awake()
    {
        obj = Instantiate(gunPrefab, transform.position, Quaternion.identity, transform);
        gun = obj.GetComponent<Gun>();
    }

    private void Update()
    {
        if (Handler.controls.Gun.Attack.IsPressed()) gun.Fire(dir);
        if (Handler.controls.Gun.Reload.WasPressedThisFrame()) gun.Reload();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Point gun
        Vector2 mouseScreen = Handler.controls.Player.Look.ReadValue<Vector2>();
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0f;

        dir = (mouseWorld - transform.position).normalized;
        obj.transform.localPosition = dir * distanceFromPlayer;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
