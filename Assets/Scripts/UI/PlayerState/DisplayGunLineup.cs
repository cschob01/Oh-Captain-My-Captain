using UnityEngine;
using UnityEngine.UI;

public class DisplayGunLineup : MonoBehaviour
{
    [System.Serializable]
    public class TinyDisplay
    {
        public GameObject obj;
        public Image background;
        public Image icon;
    }
    [SerializeField] private TinyDisplay[] Displays;

    private void UpdateDisplay()
    {
        for (int i = 0; i < Displays.Length; i++)
        {
            if (i < CaptainHandler.Instance.Guns.Count)
            {
                Displays[i].icon.sprite = CaptainHandler.Instance.Guns[i].GetComponent<SpriteRenderer>().sprite;
                if (i == CaptainHandler.Instance.CurrGun) Displays[i].background.color = Color.green;
                else Displays[i].background.color = Color.white;


                Displays[i].obj.SetActive(true);
            }
            else Displays[i].obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
}
