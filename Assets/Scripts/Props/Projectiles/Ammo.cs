using UnityEngine;

[CreateAssetMenu]
public class Ammo : ScriptableObject
{
    [Range(0, 1000)]
    [Tooltip("Hitpoints")]
    public int damage = 10;

    [Range(0, 400)]
    public float weight = 4f;
}
