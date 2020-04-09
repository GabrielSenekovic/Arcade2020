using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallVariant
{
    None = 0,
    Bottom_1 = 1,
    BottomLeft_1 = 2,
    BottomRight_1 = 3,
    Left_1 = 4,
    Right_1 = 5,
    Top_1 = 6,
    TopLeft_1 = 7,
    TopRight_1 = 8,
    Bottom_2 = 9,
    BottomLeft_2 = 10,
    BottomRight_2 = 11,
    Left_2 = 12,
    Right_2 = 13,
    Top_2 = 14,
    TopLeft_2 = 15,
    TopRight_2 = 16
}

public class WallPosition : MonoBehaviour
{
    WallVariant m_variant = WallVariant.None;
    public int heightLevel = 0;
    bool m_IsOccupied = false;
    [SerializeField]Vector2 m_Position;

    public void PlaceDown(int height)
    {
        m_IsOccupied = true;
        heightLevel = height;
    }
    public void UnPlace()
    {
        m_IsOccupied = false;
    }
    public void SetPosition(Vector2 position)
    {
        m_Position = position;
    }
    public Vector2 GetPosition()
    {
        return m_Position;
    }
    public bool GetIsOccupied()
    {
        return m_IsOccupied;
    }
    public void SetVariant(WallVariant variant)
    {
        m_variant = variant;
    }
    public WallVariant GetVariant()
    {
        return m_variant;
    }
}