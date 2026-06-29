using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private bool parent = false;

    public void DestroySelf_()
    {
        if (parent) Destroy(transform.parent.gameObject);
        else Destroy(gameObject);
    }
}
