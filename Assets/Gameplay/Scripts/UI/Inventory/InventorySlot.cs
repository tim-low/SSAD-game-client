using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    [SerializeField]
    private Text quantityText;

    void Awake()
    {
        if (quantityText == null)
            quantityText = transform.Find("Quantity").GetComponent<Text>();
    }

    public void SetQuantityText(int quantity)
    {
        quantityText.text = quantity.ToString();
    }
}
