using UnityEngine;
using System.Collections;

public class ThrustShip : MonoBehaviour
{
    [SerializeField] private string Beat;

    [System.Serializable]
    public class Movement
    {
        public float time;
        public float speed;
        public Vector2 dir;

        [HideInInspector]
        public float prog = 0;
    }

    [SerializeField] private Movement Thrust;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += ThrustShipOnBeat;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= ThrustShipOnBeat;
    }

    private void ThrustShipOnBeat(string index)
    {
        if (index == Beat)
        {
            StartCoroutine(execute(Thrust));
        }
    }

    private IEnumerator execute(Movement thrust)
    {
        thrust.dir.Normalize();
        thrust.dir *= thrust.speed;

        while (thrust.prog < thrust.time)
        {
            thrust.prog += Time.fixedDeltaTime;
            Ship.Instance.SetVel(thrust.dir);
            yield return new WaitForFixedUpdate();
        }
        thrust.prog = 0f;
    }

}
