using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customizer : MonoBehaviour {

    [SerializeField]
    private SkinnedMeshRenderer skinRenderer;
    public Material[] materials;

    private Material[] currentMaterials = null;

    // Use this for initialization
    void Awake() {
        if (skinRenderer == null)
            skinRenderer = GetComponent<SkinnedMeshRenderer>();

        if (currentMaterials == null)
            currentMaterials = skinRenderer.materials;
    }
    public void TestSkin()
    {
        ChangeSkin(0,1);
    }
	public void ChangeSkin(int partIndex, int setIndex)
    {
        if (currentMaterials == null)
            currentMaterials = skinRenderer.materials;

        if (partIndex > skinRenderer.materials.Length || setIndex > materials.Length)
        {
            Debug.Log("error");
            return;
        }
        UserState.Instance().characterWears[partIndex] = (byte)setIndex;
        currentMaterials[partIndex] = materials[setIndex];
        skinRenderer.materials = currentMaterials;
    }
}
