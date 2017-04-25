#region Usings

using UnityEngine;
using System.Collections;

#endregion

public class ObjectSpawnerOnLoadGame : ObjectSpawner
{
    #region Private Properties

    GameObject masterObject;

    #endregion

    #region Constructors

    public ObjectSpawnerOnLoadGame(GameObject master)
    {
        this.masterObject = master;
    }

    #endregion

    #region Methods

    public GameObject SpawnImage(ImageObject img)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().imageObject;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());
        newObject.tag = tagName;
        newObject.name = "ImageObject";
        newObject.GetComponent<ImageScript>().SetImageObject(img);
        newObject.GetComponent<ImageScript>().LoadFromImageObject();
        return newObject;

    }

    public GameObject SpawnShape(ShapeObject shape)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapeObject;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());
        newObject.tag = tagName;
        newObject.name = "ShapeObject";
        newObject.GetComponent<ShapeScript>().SetShapeObject(shape);
        newObject.GetComponent<ShapeScript>().LoadFromShapeObject(); ;

        olObject = masterObject.GetComponent<ToolsObjects>().colorPicker;
        GameObject colorpicker = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());
        colorpicker.GetComponent<SaveTransformScript>().SetParentObject(newObject);
        colorpicker.transform.FindChild("ColorPicker").gameObject.AddComponent<ColorPickerScript>();

        return newObject;

    }

    public GameObject SpawnPhotoSphere(PhotoSphere photoSph)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().sphereObject;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());
        newObject.tag = tagName;
        newObject.name = "PhotoSphereObject";
        newObject.GetComponent<PhotoSphereScript>().SetPhotoSphere(photoSph);
        newObject.GetComponent<PhotoSphereScript>().LoadFromPhotoSphereObject();
        return newObject;
    }

    public GameObject SpawnShape3DObject(Shape3DObject shape)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapes3DObjects[shape.shape3DObjectNumber];
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());

        Object scaleHandlerPrefab = masterObject.GetComponent<WorldObjects>().scaleHandlerFor3DObjects;
        GameObject scalehandler = (GameObject)GameObject.Instantiate(scaleHandlerPrefab, new Vector3(), new Quaternion());
        scalehandler.transform.localScale = newObject.transform.localScale;
        scalehandler.name = "ScaleHandler";
        scalehandler.transform.parent = newObject.transform;
        


        newObject.GetComponent<PhotonView>().viewID = shape.PhotonViewID;
        newObject.GetComponent<PhotonView>().instantiationId = shape.PhotonViewID;
        newObject.tag = tagName;
        newObject.name = "Shape3DObject";
        newObject.GetComponent<Shape3DScript>().SetShape3DObject(shape);
        newObject.GetComponent<Shape3DScript>().LoadFromShape3DObject();

        olObject = masterObject.GetComponent<ToolsObjects>().colorPicker;
        GameObject colorpicker = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());
        colorpicker.GetComponent<SaveTransformScript>().SetParentObject(newObject);
        colorpicker.transform.FindChild("ColorPicker").gameObject.AddComponent<ColorPickerScript>();
        colorpicker.name = "ColorPicker";

        return newObject;
    }


    public GameObject SpawnTextObject(TextObject textObject)
    {
        GameObject newObject = (GameObject)GameObject.Instantiate(Resources.Load(textObject.prefabName), new Vector3(), new Quaternion());
        newObject.tag = tagName;
        newObject.name = "TextObject";
        newObject.GetComponent<TextScript>().SetTextObject(textObject);
        newObject.GetComponent<TextScript>().LoadFromTextObject();

        GameObject olObject = masterObject.GetComponent<ToolsObjects>().colorPicker;
        GameObject colorpicker = (GameObject)GameObject.Instantiate(olObject,new Vector3(), new Quaternion());
        colorpicker.GetComponent<SaveTransformScript>().SetParentObject(newObject);
        colorpicker.transform.FindChild("ColorPicker").gameObject.AddComponent<ColorPickerScript>();
        colorpicker.name = "ColorPicker";
        colorpicker.transform.rotation = newObject.transform.rotation;

        Transform toolsDock = newObject.transform.FindChild("ToolsDock");

        olObject = masterObject.GetComponent<ToolsObjects>().editButton;
        GameObject button = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());

        button.GetComponent<SaveTransformForEditButtonScript>().parentObject = toolsDock.gameObject;
        button.GetComponent<SaveTransformForEditButtonScript>().textObject = newObject.gameObject;
        button.transform.FindChild("button").GetComponent<TextEditButtonScript>().myTextObject = newObject.gameObject;

        return newObject;
    }

    public GameObject SpawnSeparatorObject(SeparatorObject sepObject)
    {
        try
        {
            GameObject newObject = (GameObject)GameObject.Instantiate(Resources.Load(sepObject.prefabName), new Vector3(), new Quaternion());
            newObject.tag = tagName;
            newObject.name = "SeparatorObject";

            if (newObject.GetComponent<SeparatorScript>() != null)
            {
                newObject.GetComponent<SeparatorScript>().SetSeparatorObject(sepObject);
                newObject.GetComponent<SeparatorScript>().LoadFromSeparatorObject();
            }
            return newObject;
        }
        catch (System.Exception)
        {
            SaveManager.DeleteFromDatabase(sepObject.PhotonViewID);
        }
        return null;
        
    }

    public GameObject SpawnLineObject(LineObject lineObject)
    {
        
        GameObject oldObject = GameObject.Find("Player").GetComponent<WorldObjects>().lineObject;
        GameObject newObject = (GameObject)GameObject.Instantiate(oldObject, new Vector3(), new Quaternion());

        newObject.tag = tagName;
        newObject.name = "lineObject";
        try
        {
            newObject.GetComponent<LineScript>().SetLineObject(lineObject);
            newObject.GetComponent<LineScript>().LoadFromLineObject();
            newObject.GetComponent<LineScript>().CreateDeleteButton();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        return newObject;
    }

    #endregion

}
