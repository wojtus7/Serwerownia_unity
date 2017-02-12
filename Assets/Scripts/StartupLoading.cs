using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class StartupLoading : MonoBehaviour {

    string prefabName = "brak";


    struct Item
    {
        string Name;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), prefabName);
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
            float newOffset = 0;

            foreach (var item in items)
            {
                prefabName = item.Value;
                var newPrefab = Resources.Load(prefabName);

                if (newPrefab != null)
                {
                    var newPosition = cabinetObject.transform.position;
                    newPosition.y += newOffset;
                    Instantiate(newPrefab, newPosition, new Quaternion(-90,0,0,0));
                    newOffset += (float) ((newPrefab as GameObject).transform.lossyScale.y * 2.5);
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
