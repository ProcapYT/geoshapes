using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom;
    public List<GameObject> rooms;

    public List<GameObject> closedRooms;

    public float waitTime = 2f;

    public bool setedSpecialRooms = false;

    public float sacrificeRoomSpawn = 33f;

    public float miniBossProbab;
    public float clearRoomProbab;

    private void Update()
    {
        if (waitTime <= 0 && setedSpecialRooms == false)
        {
            int itemRoomI = Random.Range(0, rooms.Count - 1);
            int shopI = GetRandomNumber(new int[] {itemRoomI}, 0, rooms.Count - 1);
            int sacrificeRoomI = -1;

            float sacrificeRoomProbab = Random.Range(0, 1f);

            if (sacrificeRoomProbab > sacrificeRoomSpawn / 100)
            {
                sacrificeRoomI = GetRandomNumber(new int[] { shopI, itemRoomI }, 0, rooms.Count - 1);
                rooms[sacrificeRoomI].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isSacrificeRoom = true;
            }

            rooms[rooms.Count - 1].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isBossRoom = true;
            rooms[itemRoomI].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isItemRoom = true;
            rooms[shopI].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isShop = true;

            for (int i = 0; i < rooms.Count; i++) {
                float isMiniBossRand = Random.value;

                if (i != itemRoomI && i != shopI && i != sacrificeRoomI && isMiniBossRand <= miniBossProbab / 100) {
                    rooms[i].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isMiniBoss = true;
                }
            }

            for (int i = 0; i < rooms.Count; i++) {
                float isClearedRand = Random.value;

                if (i != itemRoomI && i != shopI && i != sacrificeRoomI && isClearedRand <= clearRoomProbab / 100) {
                    if (!rooms[i].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isMiniBoss) {
                        rooms[i].transform.Find("Room enter").gameObject.GetComponent<RoomStart>().isCleared = true;
                    }
                }
            }

            setedSpecialRooms = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    int GetRandomNumber(int[] excludeList, int min, int max)
    {
        int randomNumber;
        do
        {
            randomNumber = Random.Range(min, max);
        } while (excludeList.Contains(randomNumber));

        return randomNumber;
    }
}
