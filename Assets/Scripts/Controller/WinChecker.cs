using System;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    public event Action<SymbolData> WinEvent;
    public event Action LoseEvent;

    public void Evaluate(SymbolData[,] grid)
    {
        if (grid == null || grid.Length == 0) return;

        if (CheckRows(grid) || CheckColumns(grid) || CheckDiagonals(grid))
        {
            Debug.Log("WIN!");
            WinEvent?.Invoke(grid[1,1]); 
        }
        else
        {
            Debug.Log("LOSE");
            LoseEvent?.Invoke();
        }
    }

    private bool CheckRows(SymbolData[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            bool allMatch = true;
            for (int c = 1; c < cols; c++)
            {
                if (grid[r, c].name != grid[r, 0].name)
                {
                    allMatch = false;
                    break;
                }
            }
            if (allMatch) return true;
        }
        return false;
    }

    private bool CheckColumns(SymbolData[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int c = 0; c < cols; c++)
        {
            bool allMatch = true;
            for (int r = 1; r < rows; r++)
            {
                if (grid[r, c].name != grid[0, c].name)
                {
                    allMatch = false;
                    break;
                }
            }
            if (allMatch) return true;
        }
        return false;
    }

    private bool CheckDiagonals(SymbolData[,] grid)
    {
        int size = grid.GetLength(0);
        if (size != grid.GetLength(1)) return false;

        bool diag1 = true;
        for (int i = 1; i < size; i++)
        {
            if (grid[i, i].name != grid[0, 0].name)
            {
                diag1 = false;
                break;
            }
        }

        bool diag2 = true;
        for (int i = 1; i < size; i++)
        {
            if (grid[i, size - i - 1].name != grid[0, size - 1].name)
            {
                diag2 = false;
                break;
            }
        }

        return diag1 || diag2;
    }
}
