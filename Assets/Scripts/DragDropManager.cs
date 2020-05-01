using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using B83.Win32;
public class DragDropManager : MonoBehaviour
{
    DropInfo dropInfo = null;
    public RectTransform R;
    public RectTransform G;
    public RectTransform B;
    public RectTransform A;
    public RectTransform RGB;
    public RectTransform A2;

    class DropInfo
    {
        public string file;
        public Vector2 pos;
    }
    void OnEnable()
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;

    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }
    void OnFiles(List<string> aFiles, POINT aPos)
    {
        var s = aFiles[0];
        var info = new DropInfo
        {
            file = s,
            pos = new Vector2(aPos.x, aPos.y)
        };
        dropInfo = info;

        if (RectTransformUtility.RectangleContainsScreenPoint(R, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO R");
            ShadingMapBaker.instance.AddTexture(0, dropInfo.file);
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(G, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO G");
            ShadingMapBaker.instance.AddTexture(1, dropInfo.file);
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(B, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO B");
            ShadingMapBaker.instance.AddTexture(2, dropInfo.file);
        }
        if (RectTransformUtility.RectangleContainsScreenPoint(A, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO A");
            ShadingMapBaker.instance.AddTexture(3, dropInfo.file);
        }
         if (RectTransformUtility.RectangleContainsScreenPoint(RGB, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO RGB");
            ShadingMapBaker.instance.AddTexture(4, dropInfo.file);
        }
         if (RectTransformUtility.RectangleContainsScreenPoint(A2, Input.mousePosition))
        {
            Debug.Log("DRAGGED TO A2");
            ShadingMapBaker.instance.AddTexture(5, dropInfo.file);
        }
    }
    void LoadImage(int aIndex, DropInfo aInfo)
    {
        /*
        if (aInfo == null)
            return;
        // get the GUI rect of the last Label / box
        var rect = GUILayoutUtility.GetLastRect();
        // check if the drop position is inside that rect
        if (rect.Contains(aInfo.pos))
        {
            var data = System.IO.File.ReadAllBytes(aInfo.file);
            var tex = new Texture2D(1, 1);
            tex.LoadImage(data);
            if (textures[aIndex] != null)
                Destroy(textures[aIndex]);
            textures[aIndex] = tex;
        }
        */
    }
}
