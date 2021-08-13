using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture : MonoBehaviour
{
    public static Texture instance;
    private void Awake()
    {
        instance = this;
    }
    
    // RIGHT -- DOWN -- LEFT -- UP
    // On tourne dans le sens trigonometrique, et on commence par Right

    [Header("Floor 4 Coté identique")]
    public GameObject plainRDLU;
    public GameObject waterRDLU;
    
    [Header("Floor Water 2 côte ")]
    public GameObject plainRD_waterLU;
    public GameObject plainDL_waterUR;
    public GameObject plainLU_waterRD;
    public GameObject plainUR_waterDL;

    [Header("Floor Water 1 côte ")]
    public GameObject plainRDL_waterU;
    public GameObject plainDLU_waterR;
    public GameObject plainLUR_waterD;
    public GameObject plainURD_waterL;




}
