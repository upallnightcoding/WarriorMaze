using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Framework 
{
    private GameObject model = null;

    public Framework Blueprint(GameObject framework) 
    {
        model = Object.Instantiate<GameObject>(framework);

        return(this);
    }

    public Framework Assemble(GameObject[] additionList, string anchorName)
    {
        int selection = Random.Range(0, additionList.Length);

        return(Assemble(additionList[selection], anchorName));
    }

    public Framework Assemble(GameObject addition, string anchorName, bool create = true)
    {
        if ((addition != null) && (create))
        {
            Transform anchors = model.transform.Find("Anchors");
            Transform anchor = anchors.Find(anchorName);
            
            GameObject newPart = null;

            if (IsAPreFab(addition))
            {
                newPart = Object.Instantiate(addition, anchor);
            } else {
                addition.transform.position = anchor.transform.position;
                addition.transform.parent = anchor;
            }
        }

        return(this);
    }

    public Framework Parent(Transform transform)
    {
        model.transform.parent = transform;

        return(this);
    }

    public Framework Position(Vector3 position)
    {
        model.transform.position = position;

        return(this);
    }

    public GameObject Build()
    {
        return(model);
    }

    public GameObject Build(Vector3 rotate)
    {
        model.transform.rotation = Quaternion.Euler(rotate);

        return(model);
    }

    private bool IsAPreFab(GameObject thing) 
    {
        return(
            PrefabUtility.GetPrefabInstanceStatus(thing) != PrefabInstanceStatus.NotAPrefab 
            || PrefabUtility.GetPrefabAssetType(thing) != PrefabAssetType.NotAPrefab
        );
    }
}
