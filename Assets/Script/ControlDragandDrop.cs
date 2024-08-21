using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDragandDrop : MonoBehaviour
{
    public GameObject[] item;
    public GameObject[] itemDrop;

    public int jarak;

    Vector2[] itemPos = new Vector2[6];
    void Start()
    {
        for (int i = 0; i < itemPos.Length; i++)
        {
            itemPos[i] = item[i].transform.localPosition;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemDrag(int number)
    {
        item[number].transform.position = Input.mousePosition;
    }

    public void ItemEndDrag(int number)
    {
        float distance = Vector3.Distance(item[number].transform.localPosition, itemDrop[number].transform.localPosition);

        if (distance < jarak)
        {
            item[number].transform.localPosition = itemDrop[number].transform.localPosition;

            GameObject.Find("PointsHandler").GetComponent<WinScript>().AddPoints();
        }
        else
        {
            item[number].transform.localPosition = itemPos[number];
        }
    }
}
