using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Uextensions{
    public static Sprite createSprite(this Texture t){
        return Sprite.Create((Texture2D)t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
