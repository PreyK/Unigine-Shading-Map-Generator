using System;
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
    public Material BPR_bakingMat;
    public Material albedoBakingMat;
    public Material previewMat;
    public Vector2Int Resolution;

    Texture R;
    Texture G;
    Texture B;
    Texture A;
    Texture RGB;
    Texture A2;

    [Header("Display Images")]
    public Image r_prev;
    public Image g_prev;
    public Image b_prev;
    public Image a_prev;
    public Image albedo_prev;
    public Image opacity_prev;
    public RawImage PBR_outPrev;
    public RawImage albedoOutPrev;
    public Texture tem;
    public Button saveShadingMap;
    public Button saveAlbedoMap;

    int r;

    private void Start()
    {
        r = UnityEngine.Random.Range(1, 99);
        
        saveShadingMap.onClick.AddListener(delegate
        {
            Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
            RenderTexture.active = shadingMapRT;
            texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);
            

            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "Shade", "png");

            //save texture to file
            byte[] png = texture.EncodeToPNG();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllBytes(path, png);
            
            //AssetDatabase.Refresh();
        });

         saveAlbedoMap.onClick.AddListener(delegate
        {
            Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
            RenderTexture.active = opacityMapRT;
            texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);

            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "Albedo", "png");

            //save texture to file
            byte[] png = texture.EncodeToPNG();
            if (File.Exists(path))
            {
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
        BPR_bakingMat.SetTexture("_R", R);
        r_prev.sprite = R.createSprite();
        RenderShadingMap();
    }


    public RenderTexture shadingMapRT;
    public RenderTexture opacityMapRT;

    private void Awake()
    {
        if (!instance) instance = this;
    }
    public void AddTexture(int id, string path)
    {
        var data = System.IO.File.ReadAllBytes(path);
        var t = new Texture2D(1, 1);
        t.LoadImage(data);

        SetFinalTextureResolution(t);
        switch (id)
        {
            case 0:
                R = t;
                BPR_bakingMat.SetTexture("_R", R);
                r_prev.sprite = R.createSprite();
                break;
            case 1:
                G = t;
                BPR_bakingMat.SetTexture("_G", G);
                g_prev.sprite = G.createSprite();
                break;
            case 2:
                B = t;
                BPR_bakingMat.SetTexture("_B", B);
                b_prev.sprite = B.createSprite();
                break;
            case 3:
                A = t;
                BPR_bakingMat.SetTexture("_A", A);
                a_prev.sprite = A.createSprite();
                break;
            case 4:
                RGB = t;
                albedoBakingMat.SetTexture("_RGB", RGB);
                albedo_prev.sprite = RGB.createSprite();
                break;
            case 5:
                A2 = t;
                albedoBakingMat.SetTexture("_A", A2);
                opacity_prev.sprite = A2.createSprite();
                break;
        }

        if (id<=3)
        {
            RenderShadingMap();
            saveShadingMap.gameObject.SetActive(true);

        }
        else
        {
            RenderOpacityMap();
            saveAlbedoMap.gameObject.SetActive(true);
        }
        // SetShaderMaps();
        // RenderShadingMap();
    }

    private void RenderOpacityMap()
    {
         opacityMapRT = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, opacityMapRT, albedoBakingMat);

        previewMat.SetTexture("_Albedo", opacityMapRT);

        //transfer image from rendertexture to texture

       // Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
       // RenderTexture.active = opacityMapRT;
       // texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);
        albedoOutPrev.texture = opacityMapRT;
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
        Graphics.Blit(null, shadingMapRT, BPR_bakingMat);

        previewMat.SetTexture("_ShadingMap", shadingMapRT);

        //transfer image from rendertexture to texture

      //  Texture2D texture = new Texture2D(Resolution.x, Resolution.y);
     //   RenderTexture.active = shadingMapRT;
      //  texture.ReadPixels(new Rect(Vector2.zero, Resolution), 0, 0);
        PBR_outPrev.texture = shadingMapRT;

        //out_prev.sprite = texture.createSprite();
    }

    void SetShaderMaps()
    {
        if (R)
        {
            BPR_bakingMat.SetTexture("_R", R);
        }
        if (G)
        {
            BPR_bakingMat.SetTexture("_G", G);
        }
        if (B)
        {
            BPR_bakingMat.SetTexture("_B", B);
        }
        if (A)
        {
            BPR_bakingMat.SetTexture("_A", A);
        }
    }




    //editor test

    [ContextMenu("bakemat")]
    public void Bake()
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(Resolution.x, Resolution.y);
        Graphics.Blit(null, renderTexture, BPR_bakingMat);

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
