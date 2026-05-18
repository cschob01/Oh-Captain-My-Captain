using TMPro;
using UnityEngine;

public class DisplayRound : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    private void OnEnable()
    {
        EventHandler.Instance.OnRoundChange += ShowRound;
    }

    private void OnDisable()
    {
        EventHandler.Instance.OnRoundChange -= ShowRound;
    }

    private void ShowRound(int round)
    {
        Text.text = "Round: " + round.ToString();
    }

}
