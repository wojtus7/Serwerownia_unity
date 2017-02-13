using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StartupLoading : MonoBehaviour {

    string prefabName = "brak";
    GameObject lastItem;
    public Vector3 startingOffset = new Vector3 { x = -4.062f, y = -2.6f, z = 2.512f };

    struct Item
    {
        string Name;
    }

    void OnGUI()
    {
        //GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), prefabName);
    }

    // Use this for initialization
    void Start () {

        XDocument xdoc = XDocument.Load("XML/Serwerownia.xml");

        //Run query
        var cabinets = xdoc.Descendants("cabinet");

        //Loop through results
        foreach (var cabinet in cabinets)
        {
            var items = cabinet.Elements();
            var cabinetObject = GameObject.Find(cabinet.Attribute("name").Value);
            float newOffset = 0.0f;

            foreach (var item in items)
            {
                prefabName = item.Value;
                var newPrefab = Resources.Load(prefabName);

                var newPosition = startingOffset;

                if (newPrefab != null)
                {
                    //newPosition = cabinetObject.transform.position;
                    //newPosition.x -= cabinetObject.transform.lossyScale.x;
                    //newPosition.z -= cabinetObject.transform.lossyScale.z;

                    lastItem = Instantiate(newPrefab, cabinetObject.transform) as GameObject;

                    float height = lastItem.GetComponent<Renderer>().bounds.size.y;
                    newOffset += height; //* 2.0f;

                    //newPosition.y -= cabinetObject.GetComponent<Renderer>().bounds.center.y;
                    newPosition.y += newOffset;

                    lastItem.transform.position = newPosition;
                    lastItem.transform.Rotate(90, 0, 0);

                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
