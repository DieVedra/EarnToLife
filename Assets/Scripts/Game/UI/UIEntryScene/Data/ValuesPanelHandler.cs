using UnityEngine;

public class ValuesPanelHandler
{
    public readonly float BackgroundDurationFade = 1f;
    public readonly float BlackBackground = 1f;
    public readonly float ClearBackground = 0f;
    
    public readonly float FrameBackgroundDurationFade = 0.5f;
    public readonly Color DarkenedFrameBackgroundColor = new Color(0.5f,0.5f,0.5f,1f);
    public readonly Color ClearFrameBackgroundColor = new Color(1f,1f,1f,1f);
    
    public readonly float MovePanelDuration = 0.5f;
    public readonly Vector2 StartPositionPanel = new Vector2(0f, 1000f);
    public readonly Vector2 EndPositionPanel = new Vector2(0f, 0f);
}