using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StartupLoading : MonoBehaviour {

    public Vector3 startingOffset = new Vector3 { x = -4.062f, y = -0.9f, z = 2.512f };
    public float verticalSpacing = 0.05f;

    // Use this for initialization
    void Start () {

        string prefabName = "brak";
        GameObject newItem;

        XDocument xdoc = XDocument.Load("XML/Serwerownia.xml");

        //Run query
        var cabinets = xdoc.Descendants("cabinet");

        //Loop through results
        foreach (var cabinet in cabinets)
        {
            var items = cabinet.Elements();
            var cabinetObject = GameObject.Find(cabinet.Attribute("name").Value);
            float heightOffset = 0.0f;

            foreach (var item in items)
            {
                if(item.Attribute("spacing") != null)
                {
                    float spacingOffset = float.Parse(item.Attribute("spacing").Value);
                    heightOffset += spacingOffset;
                }

                prefabName = item.Value;
                var prefabType = Resources.Load(prefabName);

                var newPosition = startingOffset;

                if (prefabType != null)
                {
                    //newPosition = cabinetObject.transform.position;
                    //newPosition.x -= cabinetObject.transform.lossyScale.x;
                    //newPosition.z -= cabinetObject.transform.lossyScale.z;

                    newItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;

                    float height = newItem.GetComponent<Renderer>().bounds.extents.z;
                    heightOffset += height; //* 2.0f;
                    var debugext = newItem.GetComponent<Renderer>().bounds.extents;
                    Debug.Log(String.Format("{0} > Extens x: {1}; Extens y: {2}; Extens z: {3}"
                        , prefabName, debugext.x, debugext.y, debugext.z));

                    //newPosition.y -= cabinetObject.GetComponent<Renderer>().bounds.center.y;
                    newPosition.y += heightOffset;

                    newItem.transform.position = newPosition;

                    if (item.Element("rotation") != null)
                    {
                        Vector3 r;
                        r.x = float.Parse(item.Element("rotation").Attribute("x").Value);
                        r.y = float.Parse(item.Element("rotation").Attribute("y").Value);
                        r.z = float.Parse(item.Element("rotation").Attribute("z").Value);
                        newItem.transform.Rotate(r);
                    }
                    else
                        newItem.transform.Rotate(90, 0, 0);

                    heightOffset += height + verticalSpacing;

                }

            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
