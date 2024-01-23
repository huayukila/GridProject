using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static Vector3 GetMouseWorldPosition3D(Vector3 mousePosition, string layerName)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out var hit, 1000, LayerMask.GetMask(layerName));
        return hit.point;
    }

    public static Vector2 GetMouseWorldPosition2D(Vector3 mousePosition)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        var hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
        return hit.point;
    }

    public static GameObject DrawTextOnObjectHead(Vector3 position, Vector3 offset, string text)
    {
        var textObject = new GameObject("TextObject")
        {
            transform =
            {
                position = position + offset
            }
        };
        textObject.AddComponent<MeshRenderer>();
        var textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        return textObject;
    }


    public static bool IsMouseOverUI()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    public static Vector3 BezierCure(Vector3 start_, Transform end_, float t_)
    {
        var dir = (end_.position - start_).normalized;
        var offSet = -dir * 20 + Vector3.up * 6;

        var a = start_;
        var b = start_ + offSet;
        var c = end_.position + offSet;
        var d = end_.position;

        var aa = a + (b - a) * t_;
        var bb = b + (c - b) * t_;
        var cc = c + (d - c) * t_;

        var aaa = aa + (bb - aa) * t_;
        var bbb = bb + (cc - bb) * t_;
        return aaa + (bbb - aaa) * t_;
    }
}