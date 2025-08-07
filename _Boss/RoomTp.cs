using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTp : MonoBehaviour
{
    [SerializeField] private Transform BossRoom;
    public float tpTimer;
    private float currentTimer;
    public float height { get; private set; }
    public float width { get; private set; }

    public float xShift { get; private set; }
    public float yShift{ get; private set; }
public Vector2 bottomLeft { get; private set; }

    private void Awake()
    {
        Collider2D collider = GetComponent<Collider2D>();

        xShift = collider.bounds.extents.x;
        yShift = collider.bounds.extents.y * 2;

        bottomLeft = BossRoom.Find("bottomLeft").transform.position;
        Vector2 topRight = BossRoom.Find("topRight").transform.position;

        width = topRight.x - bottomLeft.x;
        height = topRight.y - bottomLeft.y;
    }

    public void Teleport(Vector2 pos = default, float xShift=-1, float yShift=-1)
    {
        xShift = (xShift == -1) ? this.xShift : xShift;
        yShift = (yShift == -1) ? this.yShift : yShift;

        Debug.Log(xShift);

        if (pos == default)
        {
            pos = bottomLeft + Vector2.right*xShift;
            pos.x += Random.value * (width-2*xShift);
            pos.y += Random.value * (height-yShift);
        }

        transform.position = pos;
    }
}