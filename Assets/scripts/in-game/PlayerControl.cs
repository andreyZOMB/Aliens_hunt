using Unity.Mathematics;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public const float trashhold = 5f;
    private Ray ray;
    private RaycastHit hit;
    private int2 pos;
    private LevelController controller;
    private ObjectControl obj;
    private bool active = true;
    [SerializeField]
    private HintSystem hintSystem;
    void Start()
    {
        controller = GetComponent<LevelController>();
    }
    void Update()
    {
        if (active)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = Input.GetTouch(0).position;
                ray = Camera.main.ScreenPointToRay(startTouchPosition);
                Physics.Raycast(ray, out hit);
                float3 p = (hit.point - controller.transform.position) / controller.CellSize;
                pos = new int2((int)p.x, (int)p.z);
                pos = controller.ClampIndex(pos);
                if (controller.cellArray[pos.x, pos.y] != null)
                {
                    obj = controller.cellArray[pos.x, pos.y].GetComponent<ObjectControl>();
                }
                else
                {
                    obj = null;
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (obj != null)
                {
                    if (!obj.isStatic)
                    {
                        endTouchPosition = Input.GetTouch(0).position;
                        int2 dir = int2.zero;
                        if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > Mathf.Abs(endTouchPosition.y - startTouchPosition.y))
                        {
                            if (Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > trashhold)
                            {
                                if (endTouchPosition.x - startTouchPosition.x > 0)
                                {
                                    dir = new int2(1, 0);
                                }
                                else
                                {
                                    dir = new int2(-1, 0);
                                }
                            }
                        }
                        else
                        {
                            if (Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > trashhold)
                            {
                                if (endTouchPosition.y - startTouchPosition.y > 0)
                                {
                                    dir = new int2(0, 1);
                                }
                                else
                                {
                                    dir = new int2(0, -1);
                                }
                            }
                        }
                        if (!dir.Equals(int2.zero))
                        {
                            hintSystem.WriteStep(new int4(obj.cellArray[0]+obj.centerPos, dir));
                            obj.Move(dir);
                            controller.Invoke(nameof(controller.Check), controller.moveTime);
                            Invoke(nameof(Enable), controller.moveTime);
                            Disable();
                        }
                    }
                    obj = null;
                }
            }
        }
    }
    public void Enable()
    {
        active = true;
    }
    public void Disable()
    {
        active = false;
    }
}
