
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject floor;
    public GameObject[] stairsUpperPrefab;
    private float constantOffSetFloorX = 10f;
    private float constantOffSetFloorZ = 10f;
    private float constantOffSetFloorY = 2.5f;
    public Vector3 posBuilding = new Vector3(0f,0f,0f);

    private Dictionary<Direction, int> stairsUpperDictonary = new Dictionary<Direction, int>();

    private enum Direction
    { 
        UPPER,
        BELOW,

        UPPER_RIGHT,
        RIGHT,
        BELOW_RIGHT,

        UPPER_DOWN,
        DOWN,
        BELOW_DOWN,

        UPPER_LEFT,
        LEFT,
        BELOW_LEFT,

        UPPER_UP,
        UP,
        BELOW_UP
    }
    
    

    //First Level
    private int floorCount = 15;
    private Dictionary<int, Direction> rngDictionatyLevelOne = new Dictionary<int, Direction>();


    void Start()
    {
        // Pour le level 1, c'est les seuls directions ou il peut aller
        rngDictionatyLevelOne[0] = Direction.RIGHT;
        rngDictionatyLevelOne[1] = Direction.UPPER_RIGHT;
        rngDictionatyLevelOne[2] = Direction.UP;
        rngDictionatyLevelOne[3] = Direction.UPPER_UP;
        rngDictionatyLevelOne[4] = Direction.DOWN;
        rngDictionatyLevelOne[5] = Direction.UPPER_DOWN;

        // Pour les escaliers
        stairsUpperDictonary[Direction.UPPER_RIGHT] = 0;
        stairsUpperDictonary[Direction.UPPER_DOWN] = 1;
        stairsUpperDictonary[Direction.UPPER_LEFT] = 2;
        stairsUpperDictonary[Direction.UPPER_UP] = 3;



        Instantiate(floor, transform.position, Quaternion.identity);

        BuildingLevelOne();
    }

    private void BuildingLevelOne() // +X +Y =Z 
    {
        for (int i = 0; i < floorCount; i++)
        {
            int rng = Random.Range(0, rngDictionatyLevelOne.Count);
            
            if(rngDictionatyLevelOne[rng] == Direction.RIGHT)
            {
                posBuilding.x = posBuilding.x + constantOffSetFloorX;
            }
            else if (rngDictionatyLevelOne[rng] == Direction.UPPER_RIGHT)
            {
                MakeStairs(rng);
                posBuilding.x = posBuilding.x + constantOffSetFloorX;
                posBuilding.y = posBuilding.y + constantOffSetFloorY;
            }
            else if(rngDictionatyLevelOne[rng] == Direction.UP)
            {
                posBuilding.z = posBuilding.z + constantOffSetFloorZ;
            }
            else if (rngDictionatyLevelOne[rng] == Direction.UPPER_UP)
            {
                MakeStairs(rng);
                posBuilding.z = posBuilding.z + constantOffSetFloorZ;
                posBuilding.y = posBuilding.y + constantOffSetFloorY;
            }
            else if (rngDictionatyLevelOne[rng] == Direction.DOWN)
            {
                posBuilding.z = posBuilding.z - constantOffSetFloorZ;
            }
            else if (rngDictionatyLevelOne[rng] == Direction.UPPER_DOWN)
            {
                MakeStairs(rng);
                posBuilding.z = posBuilding.z - constantOffSetFloorZ;
                posBuilding.y = posBuilding.y + constantOffSetFloorY;
            }

            Instantiate(floor, posBuilding, Quaternion.identity);

        }
    }

    private void MakeStairs(int rng_)
    {
        Instantiate(stairsUpperPrefab[stairsUpperDictonary[rngDictionatyLevelOne[rng_]]], posBuilding,Quaternion.identity);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
