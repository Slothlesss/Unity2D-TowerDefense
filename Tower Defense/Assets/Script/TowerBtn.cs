using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject towerPrefab;

    [SerializeField]
    Sprite sprite;

    [SerializeField]
    int price;

    [SerializeField]
    TextMeshProUGUI priceText;

    public int Price
    {
        get { return price; }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    private void Start()
    {
        priceText.text = price + "$";
        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }
    void PriceCheck()
    {
        if(price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceText.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceText.color = Color.grey;
        }
    }
}
