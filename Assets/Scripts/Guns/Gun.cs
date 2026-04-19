using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public abstract void Fire(Vector2 dir);
    public abstract void Reload();

    protected void shoot()
    {

    }
}