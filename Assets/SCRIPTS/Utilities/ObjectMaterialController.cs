using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectMaterialController : MonoBehaviour
{
    private Dictionary<string, Material> curMaterials;
    private Material[] baseMaterials;
    private SkinnedMeshRenderer m_meshRender;
    private Outline outline;
    private bool hasOutline = false;

    private void Start()
    {
        outline = GetComponent<Outline>();
        if(outline != null)
        {
            hasOutline = true;
            outline.enabled = false;
        }
        

        m_meshRender = GetComponent<SkinnedMeshRenderer>();
        baseMaterials = m_meshRender.materials;
        curMaterials = new Dictionary<string, Material>();
        ClearDictionary();
    }

    private void ClearDictionary()
    {
        
       if(hasOutline) outline.enabled = false;
        curMaterials.Clear();
        
        foreach (Material mat in m_meshRender.materials)
        {
            curMaterials.Add(mat.name, mat);
        }
        if (hasOutline) outline.enabled = true;
    }

    public void ResetToBase()
    {
        ClearDictionary();
    }

    public void AddMaterial(Material newMaterial)
    {
        try
        {
            if (hasOutline) outline.enabled = false;

            curMaterials.Add(newMaterial.name, newMaterial);
            m_meshRender.materials = curMaterials.Values.ToArray();

            if (hasOutline) outline.enabled = true;
        }
        catch(System.Exception ex) 
        {
            if (!(ex is System.ArgumentException)) Debug.Log("Algo deu errado ao adicionar material");
        }
    }

    public void RemoveMaterial(string nameOfMaterial)
    {
        if (curMaterials.Remove(nameOfMaterial))
        {
            if (hasOutline) outline.enabled = false;
            m_meshRender.materials = curMaterials.Values.ToArray();
            if (hasOutline) outline.enabled = true;
        }
    }
}
