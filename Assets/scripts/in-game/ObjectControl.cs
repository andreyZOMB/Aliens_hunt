using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

//Должен быть на каждом объекте взаимодействующес с другими
public class ObjectControl : MonoBehaviour
{
    [NonSerialized]
    public LevelController controller;
    [NonSerialized]
    public int2 centerPos;
    public List<int2> cellArray = new();
    public bool isStatic = false;
    private CollideScript colide;
    private MoveScript move;
    private float moveTime;
    private void Start()
    {
        controller = FindAnyObjectByType<LevelController>();
        colide = GetComponent<CollideScript>();
        move = GetComponent<MoveScript>();
        moveTime = controller.moveTime;
        cellArray = CoordsRotator.Orient(cellArray,transform.rotation.eulerAngles.y);
        float3 p = (gameObject.transform.position - controller.transform.position) / controller.CellSize;
        centerPos = new int2((int)p.x, (int)p.z);
        if ((cellArray.Count < 1) && !isStatic)
        {
            throw new InvalidOperationException("Object must take at least 1 cell or be static");
        }
        for (int i = 0; i < cellArray.Count; i++)
        {
            var coords = centerPos + cellArray[i];
            if (controller.cellArray[coords.x, coords.y] == null)
            {
                controller.cellArray[coords.x, coords.y] = this;
            }
            else
            {
                Debug.LogError("Object " + gameObject.name + " overlaps with " + controller.cellArray[centerPos.x + cellArray[i].x, centerPos.y + cellArray[i].y].name);
            }
        }
    }

    //Проверяет возможность предвижения объекта
    //В случае успеха вызывает MoveScript.StartMove
    //В случае провала вызывает СollideScript.Collide
    public void Move(int2 dir)
    {
        //check
        bool success = true;
        List<ObjectControl> obstacles = new();
        for (int i = 0; i < cellArray.Count; i++)
        {
            if (controller.ValidIndex(new int2(centerPos.x + dir.x + cellArray[i].x, centerPos.y + dir.y + cellArray[i].y)))
            {
                if (controller.cellArray[centerPos.x + dir.x + cellArray[i].x, centerPos.y + dir.y + cellArray[i].y] != null && controller.cellArray[centerPos.x + dir.x + cellArray[i].x, centerPos.y + dir.y + cellArray[i].y] != this)
                {
                    success = false;
                    obstacles.Add(controller.cellArray[centerPos.x + dir.x + cellArray[i].x, centerPos.y + dir.y + cellArray[i].y]);
                }
            }
            else
            {
                success = false;
            }
        }
        //do
        if (success)
        {
            for (int i = 0; i < cellArray.Count; i++)
            {
                controller.cellArray[centerPos.x + cellArray[i].x, centerPos.y + cellArray[i].y] = null;
            }
            centerPos += dir;
            for (int i = 0; i < cellArray.Count; i++)
            {
                controller.cellArray[centerPos.x + cellArray[i].x, centerPos.y + cellArray[i].y] = this;
            }
            float3 newPosition = new(gameObject.transform.position.x + (dir.x * controller.CellSize), gameObject.transform.position.y, gameObject.transform.position.z + (dir.y * controller.CellSize));
            move.StartMove(gameObject.transform.position,newPosition,moveTime);
        }
        else
        {
            colide.Collide(obstacles);
            for (int i = 0;i < obstacles.Count;i++)
            {              
                obstacles[i].colide.Collide(new List<ObjectControl> { this });
            }
        }
    }

    private void OnDestroy()
    {
        if (controller != null)
        {
            for (int i = 0; i < cellArray.Count; i++)
            {
                controller.cellArray[(centerPos + cellArray[i]).x, (centerPos + cellArray[i]).y] = null;
            }
        }
    }
}
