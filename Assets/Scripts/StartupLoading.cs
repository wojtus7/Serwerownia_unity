using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StartupLoading : MonoBehaviour {

    public Vector3 startingOffset = new Vector3 { x = -1.92f, y = 1.388f, z = 0.5f };
    public float verticalSpacing = 0.02f;

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

            if (cabinetObject != null)
            {
                float heightOffset = 0.0f;

                foreach (var item in items)
                {

                    if (item.Attribute("type") != null)
                    {
                            var pName = item.Attribute("type").Value;
                            var pType = Resources.Load(pName, typeof(GameObject));

                            var newIt = Instantiate(pType, cabinetObject.transform) as GameObject;

                            var path = @"file://" + Application.dataPath + item.Value;
                        Debug.Log(path);
                            var tex = new WWW(path);

                        var mat = Resources.Load(pName, typeof(Material));
                        newIt.GetComponent<Renderer>().material = new Material((Material)mat);
                            newIt.GetComponent<Renderer>().material.mainTexture = tex.texture;
                        //newIt.GetComponent<Renderer>().



                        continue;
                    }

                    // check for custom offset
                    if (item.Attribute("spacing") != null)
                    {
                        float spacingOffset = float.Parse(item.Attribute("spacing").Value);
                        heightOffset += spacingOffset;
                    }

                    prefabName = item.Value;
                    var prefabType = Resources.Load(prefabName);

                    var newPosition = startingOffset;

                    if (prefabType != null)
                    {
                        newItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;

                        // offset center of new object by half of its height + previous offset from other objects
                        float height = newItem.GetComponent<Renderer>().bounds.extents.z; //z
                        heightOffset += height;

                        // apply offset
                        newPosition.z += heightOffset;
                        newItem.transform.localPosition = newPosition;

                        // apply custom or default rotation
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

                        // add second half of object's height and global item spacing for offsetting next objects
                        heightOffset += height + verticalSpacing;

                    }

                }
            }
 
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
