using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresenterUILevel
{
    private ModelUILevel _modelUILevel;
    public PresenterUILevel(ViewUILevel viewUILevel, CarInLevel carInLevel,
        GamePause gamePause, ResultsLevelProvider resultsLevel,
        SceneSwitch sceneSwitch, GlobalAudio globalAudio, CarControlMethod carControlMethod)
    {
        _modelUILevel = new ModelUILevel(viewUILevel, carInLevel, gamePause, resultsLevel,
            sceneSwitch, globalAudio, carControlMethod);
    }
}
