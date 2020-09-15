using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour {
    [SerializeField]
    private GameObject[] gridLayouts;
    [SerializeField]
    private GameObject partItemPrefab;
    [SerializeField]
    private Customizer customizer;
    [SerializeField]
    private Sprite[] headUI;
    [SerializeField]
    private Sprite[] shirtsUI;
    [SerializeField]
    private Sprite[] pantsUI;
    [SerializeField]
    private Sprite[] shoesUI;
    [SerializeField]
    private PlaySFX playSFX;
    private void InstantiateItems(int partIndex , BitArray partList , Sprite[] sprites)
    {
        GameObject partItem;
        PartItem partitemScript;
        for (int setIndex = 0; setIndex < partList.Count; setIndex++)
        {
            if(partList[setIndex])
            {
                partItem = Instantiate(partItemPrefab);
                partitemScript = partItem.GetComponent<PartItem>();
                partitemScript.SetPartItem(partIndex, setIndex, customizer, sprites[setIndex]);
                partitemScript.setPlaySFX(playSFX);
                partItem.transform.SetParent(gridLayouts[partIndex].transform, false);
            }
        }
        //create default partitem
        partItem = Instantiate(partItemPrefab);
        partitemScript = partItem.GetComponent<PartItem>();
        partitemScript.SetPartItem(partIndex, 8, customizer, sprites[8]);
        partitemScript.setPlaySFX(playSFX);
        partItem.transform.SetParent(gridLayouts[partIndex].transform, false);

    }
	public void loadData(BitArray headFlags , BitArray shirtFlags , BitArray pantsFlags , BitArray shoesFlags)
    {
        InstantiateItems(0, headFlags, headUI);
        InstantiateItems(1, shirtFlags, shirtsUI);
        InstantiateItems(2, pantsFlags, pantsUI);
        InstantiateItems(3, shoesFlags, shoesUI);
    }
	
}
