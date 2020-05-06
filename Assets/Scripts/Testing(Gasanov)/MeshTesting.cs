using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gasanov.SpeedUtils;
public class MeshTesting : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("MeshTest");

        var meshObject = MeshUtils.CreateQuadObject(1, 1, defaultMaterial);

    }
    
    void Update()
    {
        
    }
}
