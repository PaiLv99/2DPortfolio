using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public static class ShadowCaster
{
    private struct DirVector
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public DirVector(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
    }

    private struct ColumnPortion
    {
        public int X { get; private set; }
        public DirVector TopVector { get; private set; }
        public DirVector BottomVector { get; private set; }

        public ColumnPortion(int x,  DirVector bottom, DirVector top) : this()
        {
            X = x;
            BottomVector = bottom;
            TopVector = top;
        }
    }

    private static bool IsInRadius(int x, int y, int length)
    {
        return (2 * x - 1) * (2 * x - 1) + (2 * y - 1) * (2 * y - 1) <= 4 * length * length;
    }

    private static Func<int, int, T> TranslateOrigin<T>(Func<int, int, T> f, int x, int y)
    {
        return (a, b) => f(a + x, b + y);
    }

    private static Action<int, int> TranslateOrigin(Action<int, int> f, int x, int y)
    {
        return (a, b) => f(a + x, b + y);
    }

    private static Func<int, int, T> TranslateOctant<T>(Func<int, int, T> f, int octant)
    {
        switch (octant)
        {
            default: return f;
            case 1: return (x, y) => f(y, x);
            case 2: return (x, y) => f(-y, x);
            case 3: return (x, y) => f(-x, y);
            case 4: return (x, y) => f(-x, -y);
            case 5: return (x, y) => f(-y, -x);
            case 6: return (x, y) => f(y, -x);
            case 7: return (x, y) => f(x, -y);
        }
    }

    private static Action<int, int> TranslateOctant(Action<int, int> f, int octant)
    {
        switch (octant)
        {
            default: return f;
            case 1: return (x, y) => f(y, x);
            case 2: return (x, y) => f(-y, x);
            case 3: return (x, y) => f(-x, y);
            case 4: return (x, y) => f(-x, -y);
            case 5: return (x, y) => f(-y, -x);
            case 6: return (x, y) => f(y, -x);
            case 7: return (x, y) => f(x, -y);
        }
    }

    // x, y = 플레이어의 좌표, radius = 시야 범위 isOpaque = 불투명 전달, setFOV = 설정된 FOV
    public static void ComputeFOVWithShadowCast(int x, int y, int radius, Func<int, int, bool> isOpaque, Action<int, int> setFOV)
    {
        Func<int, int, bool> opaque = TranslateOrigin(isOpaque, x, y);
        Action<int, int> fov = TranslateOrigin(setFOV, x, y);

        for (int octant = 0; octant < 8; ++octant)
        {
            ComputeFOVOctantZero(TranslateOctant(opaque, octant), TranslateOctant(fov, octant), radius);
        }
    }

    private static void ComputeFOVOctantZero(Func<int, int, bool> isOpaque, Action<int, int> setFOV, int radius)
    {
        var queue = new Queue<ColumnPortion>();
        queue.Enqueue(new ColumnPortion(0, new DirVector(1, 0), new DirVector(1, 1)));

        while (queue.Count != 0)
        {
            var currant = queue.Dequeue();

            if (currant.X >= radius)
                continue;

            ComputeFOVForColumnPortion(currant.X, currant.TopVector, currant.BottomVector, isOpaque, setFOV, radius, queue);
        }
    }

    private static void ComputeFOVForColumnPortion(int x, DirVector top, DirVector bottom, Func<int, int, bool> isOpaque, Action<int,int> setFOV, int radius, Queue<ColumnPortion> queue)
    {
        int topY;
        if (x == 0)
            topY = 0;
        else
        {
            int quotient = (2 * x + 1) * top.Y / (2 * top.X);
            int remainder = (2 * x + 1) * top.Y % (2 * top.X);

            if (remainder > top.X)
                topY = quotient + 1;
            else
                topY = quotient;
        }

        int bottomY;
        if (x == 0)
            bottomY = 0;
        else
        {
            int quotient = (2 * x - 1) * bottom.Y / (2 * bottom.X);
            int remainder = (2 * x - 1) * bottom.Y % (2 * bottom.X);

            if (remainder >= bottom.X)
                bottomY = quotient + 1;
            else
                bottomY = quotient;
        }

        bool? wasLastCellOpaque = null;
        for (int y = topY; y >= bottomY; --y)
        {
            bool inRadius = IsInRadius(x, y, radius);
            if (inRadius)
            {
                setFOV(x, y);
            }

            bool currentIsOpaque = !inRadius || isOpaque(x, y);
            if (wasLastCellOpaque != null)
            {
                if (currentIsOpaque)
                {
                    if (!wasLastCellOpaque.Value)
                    {
                        queue.Enqueue(new ColumnPortion(x + 1, new DirVector(x * 2 - 1, y * 2 + 1), top));
                    }
                }
                else if (wasLastCellOpaque.Value)
                {
                    top = new DirVector(x * 2 + 1, y * 2 + 1);
                }
            }
            wasLastCellOpaque = currentIsOpaque;
        }

        if (wasLastCellOpaque != null && !wasLastCellOpaque.Value)
            queue.Enqueue(new ColumnPortion(x + 1, bottom, top));
    }

}
