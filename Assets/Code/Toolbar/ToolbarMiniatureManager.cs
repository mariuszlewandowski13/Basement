#region Usings

using UnityEngine;
using System.Collections;

#endregion

public static class ToolbarMiniatureManager {

    #region Methods

    public static GameObject CreateMiniature(GameObject owner, SceneObject miniaturedObject)
    {
        GameObject result = null;
        if (owner.transform.childCount > 0)
        {
            GameObject.Destroy(owner.transform.Find("TileMiniature").gameObject);
        }


        if (miniaturedObject is ImageObject)
        {
            result = CreateImageMiniature(owner, (ImageObject)miniaturedObject);
        }
        else if (miniaturedObject is ShapeObject)
        {
            result =  CreateShapeMiniature(owner, (ShapeObject)miniaturedObject);
        }
        else if (miniaturedObject is VideoObject)
        {
            result = CreateVideoMiniature(owner, (VideoObject)miniaturedObject);
        }
        else if (miniaturedObject is PhotoSphere)
        {
            result = CreatePhotoSphereMiniature(owner, (PhotoSphere)miniaturedObject);
        }
        else if (miniaturedObject is AudioObject)
        {
            result = CreateAudioMiniature(owner, (AudioObject)miniaturedObject);
        }
        else if (miniaturedObject is TextGroupObject)
        {
            result = CreateTextGroupMiniature(owner, (TextGroupObject)miniaturedObject);
        }
        else if (miniaturedObject is LineDrawingObject)
        {
            result = CreateMarkerMiniature(owner, (LineDrawingObject)miniaturedObject);
        }
        else if (miniaturedObject is ToolsButtons)
        {
            result = CreateToolsButtons(owner, (ToolsButtons)miniaturedObject);
        }
        else if (miniaturedObject is Shape3DObject)
        {
            result = Create3DShapeObjectMiniature(owner, (Shape3DObject)miniaturedObject);
        }

        if (!(miniaturedObject is VideoObject))
        {
            SetNewMiniatureRotation(result);
        }
        

        return result;
    }

    private static GameObject CreateShapeMiniature(GameObject owner, ShapeObject shape)
    {
        Object oldObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().shapeMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(oldObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.GetComponent<ShapeMiniatureScript>().SetMiniatureShapeObject(shape);
        newObject.transform.localRotation = new Quaternion();
        //newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    private static GameObject CreateImageMiniature(GameObject owner,ImageObject img)
    {
        Object oldObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().imageMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(oldObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.GetComponent<ImageMiniatureScript>().SetMiniatureImageObject(img);
        newObject.transform.localRotation = new Quaternion();
       // newObject.transform.position -= new Vector3(0.0f, 0.02f,0.0f);
        return newObject;
    }

    private static GameObject CreateVideoMiniature(GameObject owner, VideoObject video)
    {
        Object olObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().videoMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, owner.transform.rotation);

        newObject.transform.parent = owner.transform;
        newObject.transform.Rotate(90.0f,180.0f, 60.0f);

        newObject.name = "TileMiniature";
        newObject.GetComponent<VideoMiniatureScript>().SetMiniatureVideoObject(video);
       // newObject.transform.localRotation = new Quaternion();
        

       

        // newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    private static GameObject CreatePhotoSphereMiniature(GameObject owner, PhotoSphere photoSphere)
    {
        Object olObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().photoSphereMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";

        newObject.GetComponent<PhotoSphereMiniatureScript>().SetPhotoSphere(photoSphere);
        newObject.transform.localRotation = new Quaternion();
        return newObject;
    }

    private static GameObject CreateAudioMiniature(GameObject owner, AudioObject audio)
    {
        Object olObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().audioMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.transform.Find("Audio Source").GetComponent<AudioScript>().SetAudioSource(audio);
        newObject.transform.localRotation = new Quaternion();
        //newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    //private static GameObject CreateTextMiniature(GameObject owner, TextObject text)
    //{
    //    Object olObject = new Object();//= GameObject.Find("Player").GetComponent<MiniaturesObjects>().textMiniature;
    //    GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, new Quaternion());
    //    newObject.transform.parent = owner.transform;
    //    newObject.name = "TileMiniature";
    //    newObject.GetComponent<TextMiniatureScript>().SetTextObject(text);
    //    newObject.transform.localRotation = new Quaternion();
    //   // newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
    //    return newObject;
    //}

    private static GameObject CreateTextGroupMiniature(GameObject owner, TextGroupObject text)
    {
        GameObject olObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().txtMiniatures[text.textGroupObjectNumber];
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, olObject.transform.rotation);
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.transform.localRotation = new Quaternion();
        newObject.GetComponent<TextGroupMiniatureScript>().SetTextGroup(text);
        // newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    private static GameObject CreateMarkerMiniature(GameObject owner, LineDrawingObject line)
    {
        Object olObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().markerMiniature;
        GameObject newObject = (GameObject)GameObject.Instantiate(olObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.GetComponent<DrawingLinesMiniatureScript>().SetMiniatureDrawerObject(line);
        newObject.transform.localRotation = new Quaternion();
       // newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    private static GameObject CreateToolsButtons(GameObject owner, ToolsButtons tool)
    {
        GameObject newObject = (GameObject)GameObject.Instantiate(tool.toolIcon, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        if (newObject.GetComponent<ToolScript>() != null)
        {
            newObject.GetComponent<ToolScript>().SetToolsObjects(tool, tool.needColorsbar);
        }
       
        newObject.transform.localRotation = owner.transform.localRotation;
       // newObject.transform.position -= new Vector3(0.0f, 0.02f, 0.0f);
        return newObject;
    }

    private static void SetNewMiniatureRotation(GameObject miniature)
    {
        miniature.transform.Rotate(0.0f, -60.0f, 0.0f);
    }

    private static GameObject Create3DShapeObjectMiniature(GameObject owner, Shape3DObject shape)
    {
        Object oldObject = GameObject.Find("Player").GetComponent<MiniaturesObjects>().shapes3DMiniatures[shape.shape3DObjectNumber];
        GameObject newObject = (GameObject)GameObject.Instantiate(oldObject, owner.transform.position, new Quaternion());
        newObject.transform.parent = owner.transform;
        newObject.name = "TileMiniature";
        newObject.GetComponent<Shapes3DMiniatureScript>().Set3DShape(shape);

        //if (TutorialScript.tutorialActive && shape.shape3DObjectNumber == 5)
        //{
        //    newObject.GetComponent<HighlightingSystem.Highlighter>().FlashingOn(Color.green, Color.white, 2.0f);
        //}

        return newObject;
    }

    #endregion

}
