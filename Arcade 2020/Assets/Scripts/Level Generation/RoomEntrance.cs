﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntranceType
{
    NormalDoor = 0,
    PuzzleDoor = 1,
    BombableWall = 2,
    LockedDoor = 3,
    MultiLockedDoor = 4, //Uses more than one key
    AmbushDoor = 5 //Locks behind you, defeat all enemies to make them open
}

public class RoomEntrance : MonoBehaviour
{
    public bool Open;
    public bool Spawned;
    public Vector2 DirectionModifier;
    SpriteRenderer m_renderer;
    EntranceType m_type = EntranceType.NormalDoor;

    public void Awake()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        Open = false;
        Spawned = false;
    }
    public void SetDirectionModifier(Vector2 modifier)
    {
        DirectionModifier = modifier;
    }
    public EntranceType GetEntranceType()
    {
        return m_type;
    }
    public void SetEntranceType(EntranceType type, EntranceLibrary lib)
    {
        m_type = type;
       // m_renderer.sprite = lib.GetSprite(type);
    }
}