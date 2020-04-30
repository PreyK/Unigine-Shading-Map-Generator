using System.Collections;
using System.Collections.Generic;
using System.IO;
using SFB;
//uusing UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class ShadingMapBaker : MonoBehaviour
{
    public static ShadingMapBaker instance;
    public Material bakingMat;
    public Material previewMat;
    public Vector2Int Resolution;

    [Header("loaded textures")]
    public Texture R;
    public Texture G;
    public Texture B;
    public Texture A;

    [Header("Display Images")]
    public Image r_prev;
    public Image g_prev;
    public Image b_prev;
    public Image a_prev;
    public RawImage out_prev;

    public Texture tem;

    public Button saveShadingMap;

    private void Start()
    {
        saveShadingMap.onClick.AddListener(delegate
        {
            Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
            RenderTexture.active = shadingMapRT;
            texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);

            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "png");

            //save texture to file
            byte[] png = texture.EncodeToPNG();
            if(File.Exists(path)){
                File.Delete(path);
            }
            File.WriteAllBytes(path, png);
            
            //AssetDatabase.Refresh();
        });
    }

    [ContextMenu("ADDT")]
    public void AddTexTest()
    {
        R = tem;
        bakingMat.SetTexture("_R", R);
        r_prev.sprite = R.createSprite();
        RenderShadingMap();
    }


    public RenderTexture shadingMapRT;

    private void Awake()
    {
        if (!instance) instance = this;
    }
    public void AddTexture(int id, string path)
    {
        var data = System.IO.File.ReadAllBytes(path);
        var t = new Texture2D(1, 1);
        t.LoadImage(data);
        switch (id)
        {
            case 0:
                R = t;
                bakingMat.SetTexture("_R", R);
                r_prev.sprite = R.createSprite();
                break;
            case 1:
                G = t;
                bakingMat.SetTexture("_G", G);
                g_prev.sprite = G.createSprite();
                break;
            case 2:
                B = t;
                bakingMat.SetTexture("_B", B);
                b_prev.sprite = B.createSprite();
                break;
            case 3:
                A = t;
                bakingMat.SetTexture("_A", A);
                a_prev.sprite = A.createSprite();
                break;
        }
        // SetShaderMaps();
        RenderShadingMap();
        saveShadingMap.gameObject.SetActive(true);
    }
    void SetFinalTextureResolution(Texture t)
    {
        if (Resolution.x < t.width)
        {
            Resolution.x = t.width;
        }
        if (Resolution.y < t.height)
        {
            Resolution.y = t.height;
        }
    }
    void RenderShadingMap()
    {
        shadingMapRT = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, shadingMapRT, bakingMat);

        previewMat.SetTexture("_ShadingMap", shadingMapRT);

        //transfer image from rendertexture to texture

        Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
        RenderTexture.active = shadingMapRT;
        texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);
        out_prev.texture = shadingMapRT;

        //out_prev.sprite = texture.createSprite();
    }

    void SetShaderMaps()
    {
        if (R)
        {
            bakingMat.SetTexture("_R", R);
        }
        if (G)
        {
            bakingMat.SetTexture("_G", G);
        }
        if (B)
        {
            bakingMat.SetTexture("_B", B);
        }
        if (A)
        {
            bakingMat.SetTexture("_A", A);
        }
    }




    //editor test

    [ContextMenu("bakemat")]
    public void Bake()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, bakingMat);

        previewMat.SetTexture("_ShadingMap", renderTexture);
        //transfer image from rendertexture to texture
        Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);

        var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "png");

        //save texture to file
        byte[] png = texture.EncodeToPNG();
        File.WriteAllBytes(path, png);
        //AssetDatabase.Refresh();
    }
}
