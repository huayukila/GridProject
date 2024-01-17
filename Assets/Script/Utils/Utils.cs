using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static Vector3 GetMouseWorldPosition3D(Vector3 mousePosition, string layerName)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask(layerName));
        return hit.point;
    }

    public static Vector2 GetMouseWorldPosition2D(Vector3 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
        return hit.point;
    }

    public static GameObject DrawTextOnObjectHead(Vector3 position, Vector3 offset, string text)
    {
        GameObject textObject = new GameObject("TextObject");
        textObject.transform.position = position + offset;
        textObject.AddComponent<MeshRenderer>();
        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        return textObject;
    }


    public static bool IsMouseOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public static Vector3 BezierCure(Vector3 start_, Transform end_, float t_)
    {
        Vector3 dir = (end_.position - start_).normalized;
        Vector3 offSet = -dir * 20 + Vector3.up * 6;

        Vector3 a = start_;
        Vector3 b = start_ + offSet;
        Vector3 c = end_.position + offSet;
        Vector3 d = end_.position;

        Vector3 aa = a + (b - a) * t_;
        Vector3 bb = b + (c - b) * t_;
        Vector3 cc = c + (d - c) * t_;

        Vector3 aaa = aa + (bb - aa) * t_;
        Vector3 bbb = bb + (cc - bb) * t_;
        return aaa + (bbb - aaa) * t_;
    }
}