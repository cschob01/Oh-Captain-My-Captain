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
    public float spin_strength = 1;
    public float vel_strength = 1;

    void Start()
    {
        actions = new bool[6];

        StartCoroutine(RandomActionRoutine());
    }

    IEnumerator RandomActionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            //Every five seconds chose a new action to be actively doing
            int index = Random.Range(0, actions.Length);
            actions = new bool[actions.Length];
            actions[index] = true;
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

        // Get the camera's rotation in radians. Rotating the ship's movement by
        // this will keep its movement relative to the player's camera
        float camRot = Camera.main.transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 rotatedDir = new Vector2(
            dir.x * Mathf.Cos(camRot) - dir.y * Mathf.Sin(camRot),
            dir.x * Mathf.Sin(camRot) + dir.y * Mathf.Cos(camRot)
        );
        Ship.Instance.SetVel(rotatedDir, dir);
    }
}