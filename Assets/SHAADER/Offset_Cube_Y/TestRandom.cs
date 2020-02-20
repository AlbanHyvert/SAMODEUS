using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandom : MonoBehaviour
{
    public Renderer meshRenderer;
    private Material m_instancedMaterial;
    private Material InstancedMaterial
    {
        get { return m_instancedMaterial; }
        set { m_instancedMaterial = value; }
    }
    public float Offset;
    public float Range;


    private void Start()
    {
        meshRenderer = gameObject.GetComponent<Renderer>();
        InstancedMaterial = meshRenderer.material;
        //InstancedMaterial = Renderer.sharedMaterial;
    }

    private void Update()
    {
        InstancedMaterial.SetFloat("_RandomOffset", Offset);
        InstancedMaterial.SetFloat("_Range", Range);
    }
}
