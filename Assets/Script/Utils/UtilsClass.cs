using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UtilsClass
{
    static UtilsClass instance;

    private UtilsClass()
    {
    }

    public static UtilsClass Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UtilsClass();
            }

            return instance;
        }
    }

    public Vector3 GetMouseWorldPosition3D(Vector3 mousePosition, string layerName)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(layerName));
        return hit.point;
    }

    public Vector2 GetMouseWorldPosition2D(Vector3 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
        return hit.point;
    }

    public GameObject DrawTextOnObjectHead(Vector3 position, Vector3 offset, string text)
    {
        GameObject textObject = new GameObject("TextObject");
        textObject.transform.position = position + offset;
        textObject.AddComponent<MeshRenderer>();
        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        return textObject;
    }


    public bool IsMouseOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}