using UnityEngine;

public interface IResizable {

    //GameObject GetResizablePrefab();

    string GetResizableObjectPrefabName();

    void SetModyficableObject(ModyficableObject appearanceObject);
    void SetModyficableObjectByNetwork();

    ModyficableObject GetModyficableObject();
    void SetMaterial(Material mat);

    void UpdateActualRatio(float width, float height);
}
