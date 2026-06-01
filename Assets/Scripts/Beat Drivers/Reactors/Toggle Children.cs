using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenAfterBeat : MonoBehaviour
{
    [SerializeField] private int Beat;

    [Tooltip("Activate/Deavtivate")]
    [SerializeField] private bool active;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += ToggleChildren;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= ToggleChildren;
    }

    private void ToggleChildren(int index)
    {
        if (index  == Beat)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    } 

}
