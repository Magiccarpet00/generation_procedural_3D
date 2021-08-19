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

    // Pour la les plaine qui on des border en river
    public Dictionary<string, GameObject> maskBorderOfRiver = new Dictionary<string, GameObject>();

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

    private void Start()
    {
        maskBorderOfRiver.Add("1000", plainDLU_waterR);
        maskBorderOfRiver.Add("0100", plainLUR_waterD);
        maskBorderOfRiver.Add("0010", plainURD_waterL);
        maskBorderOfRiver.Add("0001", plainRDL_waterU);
    }

}
