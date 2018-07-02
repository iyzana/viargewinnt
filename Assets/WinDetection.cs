using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class WinDetection
{
    private static int width = 7;
    private static int height = 6;

    public static bool isWon(string[,] field)
    {
        // diagonals
        for (int x = 0; x < width - 3; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (checkFour(x, y, (cx, i) => cx + i, (cy, i) => cy + i, field))
                {
                    return true;
                }
                if (checkFour(x, y, (cx, i) => cx + i, (cy, i) => cy + 3 - i, field))
                {
                    return true;
                }
            }
        }

        // horizontal
        for (int x = 0; x < width - 3; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (checkFour(x, y, (cx, i) => cx + i, (cy, i) => cy, field))
                {
                    return true;
                }
            }
        }

        // vertical
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 3; y++)
            {
                if (checkFour(x, y, (cx, i) => cx, (cy, i) => cy + i, field))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool checkFour(int x, int y, Func<int, int, int> cx, Func<int, int, int> cy, string[,] field)
    {
        string player = field[cx(x, 0), cy(y, 0)];

        if (player.Equals(""))
        {
            return false;
        }

        for (int i = 1; i < 4; i++)
        {
            if (!player.Equals(field[cx(x, i), cy(y, i)]))
            {
                return false;
            }
        }

        return true;
    }
}
