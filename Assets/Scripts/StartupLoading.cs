using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class StartupLoading : MonoBehaviour
{
    public Vector3 scaleChange = new Vector3 {x = -1.92f, y = 1.388f, z = 0.5f};

    public Vector3 startingOffset = new Vector3 {x = -1.92f, y = 1.388f, z = 0.5f};

    public float verticalSpacing;

    // Use this for initialization
    private void Start()
    {
        GameObject newFrontItem;
        GameObject newBackItem;
        GameObject newCenterItem;

        var xdoc = XDocument.Load("XML/Serwerownia.xml");

        //Run query
        var cabinets = xdoc.Descendants("cabinet");

        //Loop through results
        foreach (var cabinet in cabinets)
        {
            var items = cabinet.Elements();
            var cabinetObject = GameObject.Find(cabinet.Attribute("name").Value);

            if (cabinetObject != null)
            {
                var heightOffset = 0.0f;

                foreach (var item in items)
                {
                    // check for custom offset
                    if (item.Attribute("spacing") != null)
                    {
                        var spacingOffset = float.Parse(item.Attribute("spacing").Value, CultureInfo.InvariantCulture);
                        heightOffset += spacingOffset * 0.1345f;
                    }

                    var prefabName = item.Value;
                    var prefabType = Resources.Load(prefabName, typeof(GameObject));

                    var newPosition = startingOffset;


                    // get a prefab item
                    if (prefabType != null)
                    {
                        newFrontItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;
                        newBackItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;
                        newCenterItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;
                    }

                    // create a new item with a given size
                    else if (item.Element("size") != null)
                    {
                        var primitiveItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        var hide = new Vector3(100, 100, 100);
                        //hide the primitive item
                        primitiveItem.transform.position = hide;
                        primitiveItem.transform.localScale = new Vector3(0,0,0);
                        primitiveItem.name = item.Value;
                        newFrontItem = Instantiate(primitiveItem, cabinetObject.transform);
                        newBackItem = Instantiate(primitiveItem, cabinetObject.transform);
                        newCenterItem = Instantiate(primitiveItem, cabinetObject.transform);
                        // apply sizing
                        var size = item.Element("size");
                        if (size != null)
                        {
                            var h = float.Parse(size.Attribute("height").Value, CultureInfo.InvariantCulture) * 0.1345f;
                            var w = 2.3f;
                            var d = float.Parse(size.Attribute("depth").Value, CultureInfo.InvariantCulture) * 0.1345f * 0.22497f;

                            var sizeVector = new Vector3(0.001f, h, w);
                            newFrontItem.transform.localScale = sizeVector;
                            newFrontItem.transform.Rotate(-180, 0, 0);
                            newBackItem.transform.localScale = sizeVector;
                            newBackItem.transform.Rotate(-180, 0, 0);
                            newCenterItem.transform.localScale = new Vector3(d, h, w);
                            newCenterItem.transform.Rotate(-180, 0, 0);
                        }
                    }
                    else
                    {
                        newFrontItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        newBackItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        newCenterItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        var hide = new Vector3(100, 100, 100);
                        newFrontItem.transform.position = hide;
                        newFrontItem.transform.localScale = new Vector3(0,0,0);
                        newBackItem.transform.position = hide;
                        newBackItem.transform.localScale = new Vector3(0,0,0);
                        newCenterItem.transform.position = hide;
                        newCenterItem.transform.localScale = new Vector3(0,0,0);
                    }

                    // offset center of new object by half of its height + previous offset from other objects
                    var height = newFrontItem.GetComponent<Renderer>().bounds.extents.z; //z
                    heightOffset += height;

                    // apply offset
                    newPosition.z += heightOffset;
                    var size2 = item.Element("size");
                    newCenterItem.transform.localPosition = newPosition;
                    if (size2 != null)
                        {
                            newPosition.x -= (float.Parse(size2.Attribute("depth").Value, CultureInfo.InvariantCulture) / 2) * 0.1345f * 0.22497f;
                        }
                    newFrontItem.transform.localPosition = newPosition;
                    if (size2 != null)
                        {
                            newPosition.x += float.Parse(size2.Attribute("depth").Value, CultureInfo.InvariantCulture) * 0.1345f * 0.22497f;
                        }
                    newBackItem.transform.localPosition = newPosition;

                    // apply custom or default rotation
                    if (item.Element("rotation") != null)
                    {
                        Vector3 r;
                        r.x = float.Parse(item.Element("rotation").Attribute("x").Value, CultureInfo.InvariantCulture);
                        r.y = float.Parse(item.Element("rotation").Attribute("y").Value, CultureInfo.InvariantCulture);
                        r.z = float.Parse(item.Element("rotation").Attribute("z").Value, CultureInfo.InvariantCulture);
                        newFrontItem.transform.Rotate(r);
                        newBackItem.transform.Rotate(r);
                        newCenterItem.transform.Rotate(r);
                    }
                    else
                    {
                        newFrontItem.transform.Rotate(90, 0, 0);
                        newBackItem.transform.Rotate(90, 0, 0);
                        newCenterItem.transform.Rotate(90, 0, 0);
                    }

                    // apply a texture
                    if (item.Attribute("texture") != null)
                    {
                        var texturePathFront = item.Attribute("texture").Value;
                        var texturePathBack = item.Attribute("textureBack").Value;
                        var filePathFront = $"XML/images/{texturePathFront}";
                        var filePathBack = $"XML/images/{texturePathBack}";
                        var textureFront = new Texture2D(2, 2);
                        var textureBack = new Texture2D(2, 2);

                        if (File.Exists(filePathFront))
                        {
                            var fileDataFront = File.ReadAllBytes(filePathFront);
                            var fileDataBack = File.ReadAllBytes(File.Exists(filePathBack) ? filePathBack : filePathFront);
                            textureFront.LoadImage(fileDataFront);
                            textureBack.LoadImage(fileDataBack);
                        }

                        newFrontItem.GetComponent<Renderer>().material.mainTexture = textureFront;
                        newBackItem.GetComponent<Renderer>().material.mainTexture = textureBack;
                    }

                    if (item.Element("scale") != null)
                    {
                        var newScale = newFrontItem.transform.localScale;
                        newScale.x *= float.Parse(item.Element("scale").Attribute("length").Value, CultureInfo.InvariantCulture);
                        newFrontItem.transform.localScale = newScale;
                        newBackItem.transform.localScale = newScale;
                        newCenterItem.transform.localScale = newScale;
                    }

                    // add second half of object's height and global item spacing for offsetting next objects
                    heightOffset += height + verticalSpacing;
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}