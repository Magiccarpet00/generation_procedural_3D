using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grid : MonoBehaviour
{
    public GameObject[,,] cells;

    public static int GRID_SIZE_X = 8;
    public static int GRID_SIZE_Y = 1;
    public static int GRID_SIZE_Z = 8;

    public GameObject preCell; 

    // On l'uttilise a la toute fin du programe pour charger la texture d'un celules
    public Dictionary<TypeFloor, GameObject> nameToTexture;

    public Dictionary<string, TypeFloor> maskBorderOfRiver = new Dictionary<string, TypeFloor>();




    // Inutile lol
    public Dictionary<Vector3, Cell> CellsPos = new Dictionary<Vector3, Cell>(); // Une variable qui permet de connaitre la celule si on lui donne des coordonnée

    public static Grid instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Le Gros tableau en 3D
        cells = new GameObject[GRID_SIZE_X, GRID_SIZE_Y, GRID_SIZE_Z];



        // Dictonaire de ShapeTheLand()
        nameToTexture = new Dictionary<TypeFloor, GameObject>();
        nameToTexture.Add(TypeFloor.FULL_PLAIN, Texture.instance.plainRDLU);
        nameToTexture.Add(TypeFloor.FULL_WATER, Texture.instance.waterRDLU);

        nameToTexture.Add(TypeFloor.PLAIN_DownLeftTop_WATER_Right, Texture.instance.plainDLU_waterR);
        nameToTexture.Add(TypeFloor.PLAIN_RightLeftTop_WATER_Down, Texture.instance.plainLUR_waterD);
        nameToTexture.Add(TypeFloor.PLAIN_RightDownTop_WATER_Left, Texture.instance.plainURD_waterL);
        nameToTexture.Add(TypeFloor.PLAIN_RightDownLeft_WATER_Top, Texture.instance.plainRDL_waterU);

        nameToTexture.Add(TypeFloor.PLAIN_DownRight_WATER_LeftTop, Texture.instance.plainRD_waterLU);
        nameToTexture.Add(TypeFloor.PLAIN_DownLeft_WATER_TopRight, Texture.instance.plainDL_waterUR);
        nameToTexture.Add(TypeFloor.PLAIN_TopLeft_WATER_RightDown, Texture.instance.plainLU_waterRD);
        nameToTexture.Add(TypeFloor.PLAIN_TopRight_WATER_LeftDown, Texture.instance.plainUR_waterDL);


        // Dictonnaire de mask pour les plaine avec une bordure en water
        // Avec 1 bord de water
        maskBorderOfRiver.Add("1000", TypeFloor.PLAIN_DownLeftTop_WATER_Right);
        maskBorderOfRiver.Add("0100", TypeFloor.PLAIN_RightLeftTop_WATER_Down);
        maskBorderOfRiver.Add("0010", TypeFloor.PLAIN_RightDownTop_WATER_Left);
        maskBorderOfRiver.Add("0001", TypeFloor.PLAIN_RightDownLeft_WATER_Top);
        // Avec 2 bord de water
        maskBorderOfRiver.Add("1100", TypeFloor.PLAIN_TopLeft_WATER_RightDown);
        maskBorderOfRiver.Add("0110", TypeFloor.PLAIN_TopRight_WATER_LeftDown);
        maskBorderOfRiver.Add("0011", TypeFloor.PLAIN_DownRight_WATER_LeftTop);
        maskBorderOfRiver.Add("1001", TypeFloor.PLAIN_DownLeft_WATER_TopRight);





        BuildGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

    } // Pour reload avec R

    public void BuildGrid() // C'est la grid des pre_cell
    {        
        for (int x = 0; x < GRID_SIZE_X; x++)
        {
            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                for (int z = 0; z < GRID_SIZE_Z; z++)
                {
                    float posX = x * 10;
                    float posY = y * 10;
                    float posZ = z * 10;

                    GameObject cube = Instantiate(preCell, new Vector3(posX, posY, posZ), Quaternion.identity);

                    Cell cell = cube.GetComponent<Cell>();
                    cell.X = x;
                    cell.Y = y;
                    cell.Z = z;
                    cells[x, y, z] = cube;

                    cell.typeFloor = TypeFloor.FULL_PLAIN;


                    Vector3 pos = new Vector3(x, y, z);
                    CellsPos.Add(pos, cell);                    
                }
            }
        }

        CheckAllFloorNeighborCell();

        LetTheRiverFlow(); // Crée la river
                          

        for (int x = 0; x < GRID_SIZE_X; x++)
        {
            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                for (int z = 0; z < GRID_SIZE_Z; z++)
                {
                    /*string mask = cells[x, y, z].GetComponent<Cell>().BorderOfRiver();*/ // Crée les bordure de la river

                    //cells[x, y, z].GetComponent<Cell>().typeFloor = maskBorderOfRiver[mask];
                }
            }
        }


        // Shape the land c'est tout a la fin
        ShapeTheLand(); // Donne de la texture a toute les cells
    }

    public void LetTheRiverFlow() // la River traverse la map Z(2 à 7)
    {
        int directionChangeThreshold = 60; // Le seul à atteindre (en pourcentage)
        int directionChangeOffset = 15; // Le coup de pouce du destin si on change pas de direction avant
        int currentDirectionChangeOffset = 0; // Coup de pouce actuel ;-) wshats a boy

        int y = 0;
        int x = UnityEngine.Random.Range(3,6);
        
        for (int z = 0; z < GRID_SIZE_Z; z++)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random + currentDirectionChangeOffset >= directionChangeThreshold)// Ding ding changement de direction :P
            {
                cells[x, y, z].GetComponent<Cell>().typeFloor = TypeFloor.FULL_WATER;
                if (random % 2 == 0 && x > 2) // a goche
                {
                    x--;
                }
                if (random % 2 == 1 && x < GRID_SIZE_X - 3) // a droite
                {
                    x++;
                }
                currentDirectionChangeOffset = 0;
            }
            else
            {
                currentDirectionChangeOffset += directionChangeOffset;
            }
            cells[x, y, z].GetComponent<Cell>().typeFloor = TypeFloor.FULL_WATER;
        }

        // Replire les trous, on remplit la ou sa touche 3 ou 4 fois la rivier

        for (int x2 = 0; x2 < GRID_SIZE_X; x2++)
        {
            for (int y2 = 0; y2 < GRID_SIZE_Y; y2++)
            {
                for (int z2 = 0; z2 < GRID_SIZE_Z; z2++)
                {
                    bool SetFullWatter = cells[x2, y2, z2].GetComponent<Cell>().More3NeighborIsFullWater();

                    if (SetFullWatter == true)
                    {
                        cells[x2, y2, z2].GetComponent<Cell>().typeFloor = TypeFloor.FULL_WATER;
                    }
                }
            }
        }

    }

    

    public void CheckAllFloorNeighborCell()
    {
        for (int x = 0; x < GRID_SIZE_X; x++)
        {
            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                for (int z = 0; z < GRID_SIZE_Z; z++)
                {
                    cells[x, y, z].GetComponent<Cell>().CheckFloorNeighbor();
                }
            }
        }
    }

    public void ShapeTheLand()
    {
        for (int x = 0; x < GRID_SIZE_X; x++)
        {
            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                for (int z = 0; z < GRID_SIZE_Z; z++)
                {
                    GameObject texture = nameToTexture[cells[x, y, z].GetComponent<Cell>().typeFloor];
                    texture.transform.parent = cells[x, y, z].transform;
                    Instantiate(texture, cells[x, y, z].transform.position, Quaternion.identity);
                }
            }
        }
    }

    

}
