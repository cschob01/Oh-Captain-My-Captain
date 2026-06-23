using System.Collections.Generic;
using UnityEngine;

public class ToggleChildrenAfterBeat : MonoBehaviour
{
    [System.Serializable]
    public class Activation
    {
        public string beat;
        public bool active;
    }

    [SerializeField] private Activation[] Activations;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += ToggleChildren;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= ToggleChildren;
    }

    private void ToggleChildren(string index)
    {
        for(int i = 0; i < Activations.Length; i++)
        {
            if (index != Activations[i].beat) continue;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(Activations[i].active);
            }
        }
    } 

}
