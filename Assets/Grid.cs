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

    public GameObject fullPlain;
    public GameObject fullWater;    

    public Dictionary<TypeFloor, GameObject> nameToTexture;

    public Dictionary<Vector3, Cell> CellsPos = new Dictionary<Vector3, Cell>(); // Une variable qui permet de connaitre la celule si on lui donne des coordonnée



    public static Grid instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cells = new GameObject[GRID_SIZE_X, GRID_SIZE_Y, GRID_SIZE_Z];
        nameToTexture = new Dictionary<TypeFloor, GameObject>();
        nameToTexture.Add(TypeFloor.FULL_PLAIN, fullPlain);
        nameToTexture.Add(TypeFloor.FULL_WATER, fullWater);
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

                    if(SetFullWatter == true)
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
