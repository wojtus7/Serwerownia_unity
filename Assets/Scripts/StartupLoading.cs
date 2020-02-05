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
        GameObject newItem;

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
                        var spacingOffset = float.Parse(item.Attribute("spacing").Value);
                        heightOffset += spacingOffset * 0.1345f;
                    }

                    var prefabName = item.Value;
                    var prefabType = Resources.Load(prefabName, typeof(GameObject));

                    var newPosition = startingOffset;


                    // get a prefab item
                    if (prefabType != null)
                    {
                        newItem = Instantiate(prefabType, cabinetObject.transform) as GameObject;
                    }

                    // create a new item with a given size
                    else if (item.Element("size") != null)
                    {
                        var primitiveItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        var hide = new Vector3(100, 100, 100);
                        //hide the primitive item
                        primitiveItem.transform.position = hide;
                        primitiveItem.name = item.Value;
                        newItem = Instantiate(primitiveItem, cabinetObject.transform);
                        // apply sizing
                        var size = item.Element("size");
                        if (size != null)
                        {
                            var h = float.Parse(size.Attribute("h").Value) * 0.1345f;
                            var w = float.Parse(size.Attribute("w").Value) * 0.1345f;
                            var d = float.Parse(size.Attribute("d").Value) * 0.1345f;

                            var sizeVector = new Vector3(d, h, w);
                            newItem.transform.localScale = sizeVector;
                            newItem.transform.Rotate(-180, 0, 0);
                        }
                    }
                    else
                    {
                        newItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    }

                    // offset center of new object by half of its height + previous offset from other objects
                    var height = newItem.GetComponent<Renderer>().bounds.extents.z; //z
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
                    {
                        newItem.transform.Rotate(90, 0, 0);
                    }

                    // apply a texture
                    if (item.Attribute("texture") != null)
                    {
                        var texturePath = item.Attribute("texture").Value;
                        var filePath = $"XML/images/{texturePath}";
                        var texture = new Texture2D(2, 2);

                        if (File.Exists(filePath))
                        {
                            var fileData = File.ReadAllBytes(filePath);
                            texture.LoadImage(fileData);
                        }

                        ApplyTextures(newItem, texture);
                    }

                    if (item.Element("scale") != null)
                    {
                        var newScale = newItem.transform.localScale;
                        newScale.x *= float.Parse(item.Element("scale").Attribute("length").Value);
                        newItem.transform.localScale = newScale;
                    }

                    // add second half of object's height and global item spacing for offsetting next objects
                    heightOffset += height + verticalSpacing;
                }
            }
        }
    }

    private void ApplyTextures(GameObject newItem, Texture2D texture)
    {
        var localScale = newItem.transform.localScale;
        var multiplier = texture.width / localScale.z;

        var atlas = new Texture2D(0, 0);
        Texture2D[] atlasTextures =
        {
            texture,
            new Texture2D((int) (localScale.x * multiplier), texture.width),
            new Texture2D(texture.width, texture.height),
            new Texture2D((int) (localScale.x * multiplier), texture.height),
            new Texture2D((int) (localScale.x * multiplier), texture.height),
            new Texture2D((int) (localScale.x * multiplier), texture.height)
        };

        atlas.PackTextures(atlasTextures, 0, 8192);

        var bytes = atlas.EncodeToPNG();
//        File.WriteAllBytes("test.png", bytes);
        newItem.GetComponent<Renderer>().material.mainTexture = atlas;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}