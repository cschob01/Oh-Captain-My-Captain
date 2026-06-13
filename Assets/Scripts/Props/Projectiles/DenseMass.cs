using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DenseMass : MonoBehaviour
{
    [SerializeField] public float acceleration;
    [SerializeField] public float radius;

    private void OnEnable()
    {
        Ship.Instance.AddDenseMass(this);
    }

    private void OnDisable()
    {
        Ship.Instance.RemoveDenseMass(this);
    }
}

