using UnityEngine;
using System.Collections;

public class SpinShip : MonoBehaviour
{
    [SerializeField] private int Beat;

    [System.Serializable]
    public class Movement
    {
        public float time;
        public float speed;

        [Tooltip("True = right")]
        public bool dir;

        [HideInInspector]
        public float prog = 0;
    }

    [SerializeField] private Movement Spin;

    private void OnEnable()
    {
        EventHandler.Instance.OnBeatChange += SpinShipOnBeat;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnBeatChange -= SpinShipOnBeat;
    }

    private void SpinShipOnBeat(int index)
    {
        if (index == Beat)
        {
            StartCoroutine(execute(Spin));
        }
    }

    private IEnumerator execute(Movement spin)
    {

        while (spin.prog < spin.time)
        {
            spin.prog += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

            Ship.Instance.SetSpin(spin.dir? 1 : -1 * spin.speed);
        }
        spin.prog = 0f;
    }

}
