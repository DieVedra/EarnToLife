using UnityEngine;
public class PresenterEntryScene
{
    private ModelEntryScene _modelEntryScene;
    public PresenterEntryScene(ViewEntryScene viewEntryScene, Garage garage, Map map, SpriteRenderer startMenuBackground, GameData gameData, GarageData garageData, GlobalAudio globalAudio, PlayerDataHandler playerDataHandler)
    {
        _modelEntryScene = new ModelEntryScene(garage, map, startMenuBackground, viewEntryScene, gameData, garageData, globalAudio, playerDataHandler);
    }
}
