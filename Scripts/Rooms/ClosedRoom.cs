using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoomTemplates templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

        templates.closedRooms.Add(gameObject);
    }
}
