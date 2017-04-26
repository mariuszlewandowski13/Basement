using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics  {
    public static List<string> StaticticsFields = new List<string>() { "TOOLBAR_IMAGES_CLICK",
        "TOOLBAR_SHAPES_CLICK", "TOOLBAR_SPHERES_CLICK","TOOLBAR_BRUSHES_CLICK",
        "TOOLBAR_SHAPES3D_CLICK","TOOLBAR_TEXTS_CLICK","TOOLBAR_GIPHYSEARCH_CLICK", "TOOLBAR_GOOGLESEARCH_CLICK",
         "TOOLBAR_BROWSER_CLICK" };

    public static void AddStatisticsToolbarEvent(int number)
    {
        if (number < StaticticsFields.Count)
        {
            GameObject.Find("Player").GetComponent<DatabaseHandler>().UpdateStats(StaticticsFields[number], ApplicationStaticData.userID);
        }
        
    }


}
