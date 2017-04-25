#region Usings

using UnityEngine;
using System.Collections.Generic;

#endregion

public class ObjectSpawnerFromGame : ObjectSpawner
{
    #region Private Properties

    GameObject masterObject;

    #endregion

    public ObjectSpawnerFromGame(GameObject master)
    {
        this.masterObject = master;
    }

    #region Methods


    public  void SpawnImage(ImageObject img)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().imageObject;

        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(0.0f, 50.0f, 0.0f), masterObject.transform.rotation);
        newObject.tag = tagName;
        newObject.name = "ImageObject";
        newObject.GetComponent<ImageScript>().SetImageObject(img);
        newObject.transform.Rotate(new Vector3(90.0f, 180.0f, 0.0f));
    }

    public GameObject SpawnImage(ImageObject img, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().imageObject;
        GameObject newObject = InstantiateGameObject(olObject, position, rotation, scale);
 
        newObject.transform.localScale = scale;
        newObject.tag = tagName;
        newObject.name = "ImageObject";
        newObject.transform.gameObject.GetComponent<ImageScript>().SetImageObject(img);
        newObject.transform.gameObject.GetComponent<ImageScript>().SetImageObjectByNetwork(img);
        newObject.transform.rotation = rotation;
        return newObject;   
    }


    public GameObject SpawnImageDuplicate(GameObject pattern)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().imageObject;
        //GameObject newObject = (GameObject)GameObject.Instantiate(pattern, pattern.transform.position, new Quaternion());
        GameObject newObject = InstantiateGameObject(olObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.localScale);
        newObject.transform.localScale = pattern.transform.localScale;
        newObject.tag = tagName;
        newObject.name = "ImageObject";
        newObject.GetComponent<ImageScript>().SetImageObject(pattern.GetComponent<ImageScript>().imageObject);

        //zerowanie zapisu
        newObject.GetComponent<ImageScript>().imageObject.saved = false;

        newObject.transform.gameObject.GetComponent<ImageScript>().SetImageObjectByNetwork(pattern.GetComponent<ImageScript>().imageObject);
        //newObject.transform.rotation = pattern.transform.rotation;

        return newObject;
    }

    public GameObject SpawnShape3DObject(Shape3DObject shape, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapes3DObjects[shape.shape3DObjectNumber];
        GameObject newObject = InstantiateGameObject(olObject, position, rotation, scale, true);
        newObject.transform.localScale = scale;
        newObject.tag = tagName;
        newObject.name = "Shape3DObject";
        newObject.transform.gameObject.GetComponent<Shape3DScript>().SetShape3DObject(shape);
        newObject.transform.gameObject.GetComponent<Shape3DScript>().SetShape3DObjectByNetwork();
        newObject.transform.rotation = rotation;

        AddSetObjectAdditionalFunctions(newObject);

        return newObject;

        // PhotonNetwork.

    }
    public GameObject SpawnShape3DObjectDuplicate(GameObject pattern)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapes3DObjects[pattern.GetComponent<Shape3DScript>().shapeObject.shape3DObjectNumber];
        ////GameObject newObject = (GameObject)GameObject.Instantiate(pattern, pattern.transform.position, new Quaternion());
        //GameObject newObject = PhotonNetwork.InstantiateSceneObject(olObject.name, pattern.transform.position, pattern.transform.rotation, 0, null);

        GameObject newObject = InstantiateGameObject(olObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.localScale);

        newObject.transform.localScale = pattern.transform.localScale;
        newObject.tag = tagName;
        newObject.name = "Shape3DObject";
        newObject.GetComponent<Shape3DScript>().SetShape3DObject(pattern.GetComponent<Shape3DScript>().shapeObject);
        newObject.transform.gameObject.GetComponent<Shape3DScript>().SetShape3DObjectByNetwork();
        //zerowanie zapisu
        newObject.GetComponent<Shape3DScript>().shapeObject.saved = false;


        AddSetObjectAdditionalFunctions(newObject);

        //newObject.transform.rotation = pattern.transform.rotation;
        return newObject;
    }

    

    public GameObject SpawnShape(ShapeObject shape, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapeObject;
        GameObject newObject = InstantiateGameObject(olObject, position, rotation, scale);
        newObject.transform.localScale = scale;
        newObject.tag = tagName;
        newObject.name = "ShapeObject";
        newObject.transform.gameObject.GetComponent<ShapeScript>().SetShapeObject(shape);
        newObject.transform.gameObject.GetComponent<ShapeScript>().SetShapeObjectByNetwork(shape);
        newObject.transform.rotation = rotation;

        AddSetObjectAdditionalFunctions(newObject);
        //colorpicker.name = "ColorPicker";
        //colorpicker.transform.rotation = rotation;

        return newObject;
    }

    public GameObject SpawnShapeDuplicate(GameObject pattern)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().shapeObject;
        GameObject newObject = InstantiateGameObject(olObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.localScale);
        newObject.transform.localScale = pattern.transform.localScale;
        newObject.tag = tagName;
        newObject.name = "ShapeObject";
        newObject.transform.gameObject.GetComponent<ShapeScript>().SetShapeObject(pattern.GetComponent<ShapeScript>().shapeObject);

        //zerowanie zapisu
        newObject.GetComponent<ShapeScript>().shapeObject.saved = false;

        newObject.transform.gameObject.GetComponent<ShapeScript>().SetShapeObjectByNetwork(pattern.GetComponent<ShapeScript>().shapeObject);
        newObject.transform.rotation = pattern.transform.rotation;

        AddSetObjectAdditionalFunctions(newObject);
       // colorpicker.name = "ColorPicker";
        //colorpicker.transform.rotation = pattern.transform.rotation;

        return newObject;
    }

    //public void SpawnPhotoSphere(PhotoSphere photoSph)
    //{
    //    GameObject sphereObject = GameObject.Find("PhotoSphere");
    //    sphereObject.GetComponent<PhotoSphereScript>().SetPhotoSphere(photoSph);
    //}

    public GameObject SpawnPhotoSphere(PhotoSphere photoSph, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().sphereObject;
        GameObject newObject = InstantiateGameObject(olObject, position, rotation, scale);
        newObject.transform.localScale = scale;
        newObject.tag = tagName;
        newObject.name = "PhotoSphereObject";

        newObject.transform.gameObject.GetComponent<PhotoSphereScript>().SetPhotoSphere(photoSph);
        newObject.transform.gameObject.GetComponent<PhotoSphereScript>().SetPhotoSphereObjectByNetwork(photoSph);

        return newObject;
    }

    public GameObject SpawnPhotoSphereDuplicate(GameObject pattern)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().sphereObject;
        GameObject newObject = InstantiateGameObject(olObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.localScale);
        newObject.transform.localScale = pattern.transform.localScale;
        newObject.tag = tagName;
        newObject.name = "PhotoSphereObject";
        newObject.transform.gameObject.GetComponent<PhotoSphereScript>().SetPhotoSphere(pattern.GetComponent<PhotoSphereScript>().photoSphere);
        //zerowanie zapisu
        newObject.GetComponent<PhotoSphereScript>().photoSphere.saved = false;

        newObject.transform.gameObject.GetComponent<PhotoSphereScript>().SetPhotoSphereObjectByNetwork(pattern.GetComponent<PhotoSphereScript>().photoSphere);
        newObject.transform.rotation = pattern.transform.rotation;

        return newObject;
    }


    public GameObject SpawnTextObjectDuplicate(GameObject pattern)
    {
        GameObject oldObject = Resources.Load(pattern.GetComponent<TextScript>().textObject.prefabName) as GameObject;
        GameObject newObject = InstantiateGameObject(oldObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.lossyScale);
        //GameObject newObject = (GameObject)GameObject.Instantiate(pattern, pattern.transform.position, new Quaternion());
        newObject.tag = tagName;
        newObject.name = pattern.name;
        newObject.GetComponent<TextScript>().SetTextObject(pattern.GetComponent<TextScript>().textObject);
        //zerowanie zapisu
        newObject.GetComponent<TextScript>().textObject.saved = false;

        newObject.GetComponent<TextScript>().SetTextByNetwork();

        newObject.transform.rotation = pattern.transform.rotation;
        newObject.transform.localScale = pattern.transform.lossyScale;

        AddSetObjectAdditionalFunctions(newObject);

        return newObject;

    }

    public GameObject SpawnLineDrawingObject(LineDrawingObject tool, GameObject controller)
    {
        Object olObject = masterObject.GetComponent<ToolsObjects>().drawingObject;

        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, controller.transform.position, new Quaternion());
        newObject.transform.parent = controller.transform.parent;
        newObject.GetComponent<LineDrawingScript>().SetMarker(tool, controller);

        return newObject;
    }


    public GameObject SpawnLineObjectDuplicate(GameObject pattern)
    {
        GameObject newObject = (GameObject)GameObject.Instantiate(pattern, pattern.transform.position, new Quaternion());
        newObject.tag = tagName;
        newObject.name = pattern.name;
        newObject.transform.rotation = pattern.transform.rotation;

        return newObject;
    }

    public GameObject SpawnLine(string name, int number)
    {
        Object olObject = masterObject.GetComponent<WorldObjects>().lineObject;

        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());

        newObject.tag = tagName;
        newObject.name = name;

        if (number == 0)
        {
            SetRandomObjectId();
            number = objectIndex;
        }
        newObject.GetComponent<PhotonView>().instantiationId = number;
        newObject.GetComponent<PhotonView>().viewID = number;

        newObject.GetComponent<LineScript>().lineObject = new LineObject();

        // newObject.AddComponent<MeshCollider>();
        //newObject.GetComponent<MeshCollider>().convex = true;
        return newObject;

    }

    public GameObject SpawnObjectDuplicate(GameObject pattern)
    {
        GameObject oldObject = Resources.Load(pattern.GetComponent<SeparatorScript>().separatorObject.prefabName) as GameObject;
        GameObject newObject = InstantiateGameObject(oldObject, pattern.transform.position, pattern.transform.rotation, pattern.transform.lossyScale);
        newObject.tag = tagName;
        newObject.name = pattern.name;
        newObject.transform.rotation = pattern.transform.rotation;

        newObject.transform.localScale = pattern.transform.lossyScale;

        if (newObject.GetComponent<SeparatorScript>() != null)
            { 
            newObject.GetComponent<SeparatorScript>().SetSeparatorObject(new SeparatorObject(pattern.GetComponent<SeparatorScript>().separatorObject.prefabName, pattern.GetComponent<Renderer>().material.color));
            newObject.GetComponent<SeparatorScript>().separatorObject.saved = false;
            newObject.GetComponent<SeparatorScript>().SetSeparatorByNetwork();
        }
        return newObject;
    }

    public GameObject SpawnTextGroupObject(TextGroupObject textObj, Vector3 position, Quaternion rotation, Vector3 scale)
    {

        GameObject oldObject = GameObject.Find("Player").GetComponent<WorldObjects>().textGroupObjects[textObj.textGroupObjectNumber];
        GameObject txt = InstantiateGameObject(oldObject, position, rotation, scale);
        //txt.tag = tagName;
        txt.name = "TextGroup";

        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in txt.transform)
        {
            children.Add(child.gameObject);
            child.GetComponent<PhotonView>().instantiationId = objectIndex + child.GetSiblingIndex() + 1;
            child.GetComponent<PhotonView>().viewID = objectIndex + child.GetSiblingIndex() + 1;
            if (child.GetComponent<TextMesh>() != null)
            {

                child.GetComponent<TextScript>().SetTextObject(new TextObject(child.GetComponent<TextMesh>().text, child.GetComponent<TextMesh>().color, child.name, objectIndex + child.GetSiblingIndex() + 1));
                child.GetComponent<TextScript>().SetTextByNetwork();
                child.GetComponent<TextScript>().SetColor(textObj.color);
                
                child.tag = tagName;

                AddSetObjectAdditionalFunctions(child.gameObject);

            }
            else if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.SetColor("_Color", textObj.color);
                if (child.GetComponent<SeparatorScript>() != null)
                {
                  //  Debug.Log(child.name);
                    child.GetComponent<SeparatorScript>().separatorObject = new SeparatorObject(child.name, textObj.color);
                    child.GetComponent<SeparatorScript>().UpdateColorInSeparatorObject();
                    child.GetComponent<SeparatorScript>().SetSeparatorByNetwork();
                }
            }
            
        }

        return txt;
    }

    private GameObject InstantiateGameObject(Object prefab, Vector3 pos, Quaternion rot, Vector3 scale, bool withScaleHandler = false)
    {
        GameObject newObject = (GameObject)GameObject.Instantiate(prefab, pos, rot);

        if (withScaleHandler)
        {
            Object scaleHandlerPrefab = masterObject.GetComponent<WorldObjects>().scaleHandlerFor3DObjects;
            GameObject scalehandler = (GameObject)GameObject.Instantiate(scaleHandlerPrefab, pos, rot);
            scalehandler.transform.localScale = newObject.transform.localScale;
            scalehandler.name = "ScaleHandler";
            scalehandler.transform.parent = newObject.transform;
        }

        if (!PhotonNetwork.inRoom)
        {
            GameObject.DestroyImmediate(newObject.GetComponent<CubeLerp>());
            GameObject.DestroyImmediate(newObject.GetComponent<PhotonView>());
        }
        else {
            SetRandomObjectId();
            if (PhotonView.Find(objectIndex) != null)
            {
                SetRandomObjectId();
            }
            newObject.GetComponent<PhotonView>().instantiationId = objectIndex;
            newObject.GetComponent<PhotonView>().viewID = objectIndex;
            newObject.GetComponent<PhotonView>().ownershipTransfer = OwnershipOption.Takeover;
           // newObject.GetComponent<PhotonView>().RequestOwnership();

            masterObject.GetComponent<PhotonView>().RPC("InstantiateOnRemote", PhotonTargets.Others, prefab.name, objectIndex, pos, rot, scale, withScaleHandler);
        }
        return newObject;
    }


    public void AddSetObjectAdditionalFunctions(GameObject gameObj)
    {
        if (gameObj.GetComponent<ShapeScript>() != null || gameObj.GetComponent<Shape3DScript>() != null || gameObj.GetComponent<TextScript>() != null)
        {
            GameObject olObject = masterObject.GetComponent<ToolsObjects>().colorPicker;
            GameObject colorpicker = (GameObject)GameObject.Instantiate(olObject, gameObj.transform.position, new Quaternion());
            colorpicker.GetComponent<SaveTransformScript>().SetParentObject(gameObj);
            colorpicker.transform.FindChild("ColorPicker").gameObject.AddComponent<ColorPickerScript>();
            //colorpicker.transform.FindChild("Pointer").GetComponent<ColorPickerPointerScript>().SetColorPicker(colorpicker.transform.FindChild("ColorPicker").gameObject);
            //colorpicker.transform.FindChild("Pointer2").GetComponent<BrightnessPickerScript>().SetColorPicker(colorpicker.transform.FindChild("ColorPicker").gameObject, colorpicker.transform.FindChild("BrightnessPicker").gameObject);
            colorpicker.name = "ColorPicker";
        }

        if (gameObj.GetComponent<TextScript>() != null)
        {
            GameObject olObject = masterObject.GetComponent<ToolsObjects>().editButton;
            GameObject button = (GameObject)GameObject.Instantiate(olObject, new Vector3(), new Quaternion());

            Transform toolsDock = gameObj.transform.FindChild("ToolsDock");
            button.GetComponent<SaveTransformForEditButtonScript>().parentObject = toolsDock.gameObject;
            button.GetComponent<SaveTransformForEditButtonScript>().textObject = gameObj;
            button.transform.FindChild("button").GetComponent<TextEditButtonScript>().myTextObject = gameObj;
        }

    }


    #endregion

}
