using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class test3 : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;
    [Button]
    private void a()
    {
        // StartCoroutine(b());
        c().Forget();
    }
    [Button]
    private void e()
    {
        StartCoroutine(b());
    }
    private async UniTaskVoid c()
    {
        await UniTask.WaitForEndOfFrame(this);
        Texture2D texture2d = ScreenCapture.CaptureScreenshotAsTexture();
        Rect rect = new Rect(0f,0f,  texture2d.width, texture2d.height);
        SpriteRenderer.sprite = Sprite.Create(texture2d, rect,new Vector2(0.5f,0.5f) );
    }
    private IEnumerator b()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture2d = ScreenCapture.CaptureScreenshotAsTexture();
        Rect rect = new Rect(0f,0f,  texture2d.width, texture2d.height);
        SpriteRenderer.sprite = Sprite.Create(texture2d, rect,new Vector2(0.5f,0.5f) );
        
        
    }
}
