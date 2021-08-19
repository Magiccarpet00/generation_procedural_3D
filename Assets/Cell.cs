using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int X;
    public int Y;
    public int Z;

    public TypeFloor typeFloor;
    public Dictionary<DirectionFloorNeighborCell, Cell> FloorNeighborCell = new Dictionary<DirectionFloorNeighborCell, Cell>();
    

    // Ya toujours 4 voisin mais ceux du bord sont null
    public void CheckFloorNeighbor()
    {
        if(X != Grid.GRID_SIZE_X - 1)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.RIGHT] = Grid.instance.CellsPos[new Vector3(X + 1, Y, Z)];
        }
        else
        {
            FloorNeighborCell[DirectionFloorNeighborCell.RIGHT] = null;
        }

        if(X != 0)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.LEFT] = Grid.instance.CellsPos[new Vector3(X - 1, Y, Z)];
        }
        else
        {
            FloorNeighborCell[DirectionFloorNeighborCell.LEFT] = null;
        }

        if (Z != Grid.GRID_SIZE_Z - 1)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.UP] = Grid.instance.CellsPos[new Vector3(X, Y, Z + 1)];
        }
        else
        {
            FloorNeighborCell[DirectionFloorNeighborCell.UP] = null;
        }

        if (Z != 0)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.DOWN] = Grid.instance.CellsPos[new Vector3(X, Y, Z - 1)];
        }
        else
        {
            FloorNeighborCell[DirectionFloorNeighborCell.DOWN] = null;
        }

    } 

    public bool More3NeighborIsFullWater()
    {
        int nbNeigborFullWatter = 0;

        // [CODE PANIQUE 2] Avec ma modification ça marchais plus du coup j'ai refait en mode nul ;)

        //foreach (Cell item in FloorNeighborCell.Values)
        //{
        //    if (item.typeFloor == TypeFloor.FULL_WATER)
        //    {
        //        nbNeigborFullWatter++;
        //    }
        //}



        if (FloorNeighborCell[DirectionFloorNeighborCell.DOWN] != null)
        {
            if (FloorNeighborCell[DirectionFloorNeighborCell.DOWN].typeFloor == TypeFloor.FULL_WATER)
            {
                nbNeigborFullWatter++;
            }
        }
        

        if (FloorNeighborCell[DirectionFloorNeighborCell.LEFT] == null)
        {

        }
        else if (FloorNeighborCell[DirectionFloorNeighborCell.LEFT].typeFloor == TypeFloor.FULL_WATER)
        {
            nbNeigborFullWatter++;
        }

        if (FloorNeighborCell[DirectionFloorNeighborCell.UP] == null)
        {

        }
        else if (FloorNeighborCell[DirectionFloorNeighborCell.UP].typeFloor == TypeFloor.FULL_WATER)
        {
            nbNeigborFullWatter++;
        }

        if (FloorNeighborCell[DirectionFloorNeighborCell.RIGHT] == null)
        {

        }
        else if (FloorNeighborCell[DirectionFloorNeighborCell.RIGHT].typeFloor == TypeFloor.FULL_WATER)
        {
            nbNeigborFullWatter++;
        }
        

        if ( nbNeigborFullWatter >= 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
public enum TypeFloor
{
    FULL_PLAIN,
    FULL_WATER,

    PLAIN_DownLeftTop_WATER_Right,
    PLAIN_RightLeftTop_WATER_Down,
    PLAIN_RightDownTop_WATER_Left,
    PLAIN_RightDownLeft_WATER_Top,

    PLAIN_DownLeft_WATER_TopRight,
    PLAIN_TopLeft_WATER_RightDown,
    PLAIN_TopRight_WATER_LeftDown,
    PLAIN_DownRight_WATER_LeftTop,

    PLAIN_Top_WATER_RightDownLeft,
    PLAIN_Right_WATER_DownLeftTop,
    PLAIN_Down_WATER_LeftTopRight,
    PLAIN_Left_WATER_TopRightDown,
}

public enum DirectionFloorNeighborCell
{
    RIGHT,
    DOWN,
    LEFT,
    UP
}
