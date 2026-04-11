using UnityEngine;

public class HandleGuns : MonoBehaviour
{
    public GameObject gunPrefab;

    public GameObject obj = null;
    public Gun gun = null;
    public InputHandler Handler;

    public float distanceFromPlayer = .1f;

    private void Awake()
    {
        obj = Instantiate(gunPrefab, transform.position, Quaternion.identity, transform);
        gun = obj.GetComponent<Gun>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Handler.controls.Gun.Attack.IsPressed()) gun.Fire();
        if (Handler.controls.Gun.Reload.IsPressed()) gun.Reload();

        // Point gun
        Vector2 MousePos = Handler.controls.Player.Look.ReadValue<Vector2>();
        Vector2 ObjectPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 dir = (MousePos - ObjectPos).normalized;
        obj.transform.localPosition = dir * distanceFromPlayer;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
