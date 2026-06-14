using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandlePlayerDeath : MonoBehaviour
{
    [Tooltip("Disables this")]
    [SerializeField] private Collider2D Collider;
    [Tooltip("Matches Ship's velocity")]
    [SerializeField] private OnBoard onBoard;
    [Tooltip("Activates Trigger: Died")]
    [SerializeField] private Animator animator;
    [Tooltip("Sets Velocity to Zero")]
    [SerializeField] MovingObject movingObject;

    [SerializeField] private float WaitTime = 3f;
    private float WaitProg;

    private void OnEnable()
    {
        EventHandler.Instance.OnPlayerDied += OnPlayerDeath;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnPlayerDied -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        InputHandler.Instance.EnableControls(false);
        Collider.enabled = false;
        onBoard.momentum = Ship.Instance.vel;
        animator.SetTrigger("Died");
        movingObject.ModifyVel(Vector2.zero);
        StartCoroutine(WaitForReset());
    }

    private IEnumerator WaitForReset()
    {
        while(WaitProg < WaitTime)
        {
            WaitProg += Time.deltaTime;
            yield return null;
            onBoard.momentum = Ship.Instance.vel;
        }
        SceneHandler.Instance.RestartLevel();
    }

}
