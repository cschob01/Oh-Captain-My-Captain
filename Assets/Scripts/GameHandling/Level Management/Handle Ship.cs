using UnityEngine;
using System.Collections;

// Handle Ship
// Randomly does one of six actions every five seconds:
//  Rotates ship clock-wise
//  Rotates ship counter-clockwise
//  Moves ship up
//  Moves ship down
//  Moves ship left
//  Moves ship right
public class HandleShip : MonoBehaviour
{
    public bool[] actions;
    public float spin_strength = .1f;
    public float vel_strength = .5f;
    [Tooltip("6 signify ship movements")]
    public int empty_actions = 10;
    [SerializeField] float TimePerAction = 5f;

    void Start()
    {
        actions = new bool[6 + empty_actions];

        StartCoroutine(RandomActionRoutine());
    }

    IEnumerator RandomActionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimePerAction);

            //Every five seconds chose a new action to be actively doing
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i] = false;
            }
            actions[Random.Range(0, actions.Length)] = true;
        }
    }

    private void FixedUpdate()
    {
        if (actions[0])
        {
            Ship.Instance.SetSpin(-.2f * spin_strength); // Spin Left
        }
        if (actions[1])
        {
            Ship.Instance.SetSpin(.2f * spin_strength); // Spin Right
        }

        // Move ship depending on the active action
        Vector2 dir = Vector2.zero;
        dir.y = (actions[2] ? 1 : 0) + (actions[4] ? -1 : 0);
        dir.x = (actions[3] ? 1 : 0) + (actions[5] ? -1 : 0);
        dir = dir.normalized * vel_strength;

        Ship.Instance.SetVel(dir);
    }
}