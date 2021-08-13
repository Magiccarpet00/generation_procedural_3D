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



    public void CheckFloorNeighbor()
    {
        if(X != Grid.GRID_SIZE_X - 1)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.RIGHT] = Grid.instance.CellsPos[new Vector3(X + 1, Y, Z)];
        }

        if(X != 0)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.LEFT] = Grid.instance.CellsPos[new Vector3(X - 1, Y, Z)];
        }

        if (Z != Grid.GRID_SIZE_Z - 1)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.UP] = Grid.instance.CellsPos[new Vector3(X, Y, Z + 1)];
        }

        if (Z != 0)
        {
            FloorNeighborCell[DirectionFloorNeighborCell.DOWN] = Grid.instance.CellsPos[new Vector3(X, Y, Z - 1)];
        }            
        
    }

    public bool More3NeighborIsFullWater() //[CODE PANIQUE] !!!!
    {
        int nbNeigborFullWatter = 0;

        foreach (Cell item in FloorNeighborCell.Values)
        {
            if (item.typeFloor == TypeFloor.FULL_WATER)
            {
                nbNeigborFullWatter++;
            }
        }

        if( nbNeigborFullWatter >= 3)
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
