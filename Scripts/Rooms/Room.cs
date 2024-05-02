using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private RoomTemplates templates;

    // Start is called before the first frame update
    void Start()
    {
        templates = GameObject.Find("Room Templates").GetComponent<RoomTemplates>();
        templates.rooms.Add(gameObject);
    }
}
