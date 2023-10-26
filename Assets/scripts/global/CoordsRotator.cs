using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public static class CoordsRotator
{
    // Поворачивает координаты на 90 градусов
    // по часовой стрелке отнсительно 0,0
    public static int2 Rotate(int2 input)
    {
        int2 output = new()
        {
            x = input.y,
            y = -input.x
        };
        return output;
    }

    //поворачивает массив координат соответственно вращению объекта
    //работает только с поворотами на 0,90,180,270
    public  static List<int2> Orient(List<int2> input, float dir)
    {
        int k;

        switch (dir)
        {
            case 0:
                {
                    k = 0;
                    break;
                }
            case 90:
                {
                    k = 1;
                    break;
                }
            case 180:
                {
                    k = 2
                        ;
                    break;
                }
            case 270:
                {
                    k = 3;
                    break;
                }
            default:
                {
                    throw new InvalidOperationException("Object have invalid rotation");
                }
        }
        for (int i = 0; i < input.Count; i++)
        {
            for (int j = 0; j < k; j++)
            {
                input[i] = Rotate(input[i]);
            }
        }
        return input;
    }
}
