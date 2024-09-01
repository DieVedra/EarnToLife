using UnityEngine;
public class LogicEntryScene
{
    private readonly SceneSwitch _sceneSwitch;
    private readonly ButtonsUpgradeCar _buttonsUpgradeCar;
    private readonly GarageUI _garageUI;
    private readonly PanelsActivator _panelsActivator;
    private readonly ConfirmationUpgrade _confirmationUpgrade;
    private readonly MapPanelHandler _mapPanelHandler;
    private readonly AudioHandlerUI _audioHandlerUI;
    private readonly AudioSettingSwitch _audioSettingSwitch;
    private readonly Spawner _spawner;
    public LogicEntryScene(Garage garage, Map map, SpriteRenderer startMenuBackground, ViewEntryScene viewEntryScene,
        GameData gameData, GarageData garageData, GlobalAudio globalAudio, PlayerDataHandler playerDataHandler)
    {
        _sceneSwitch = new SceneSwitch(playerDataHandler, gameData);
        _audioHandlerUI = new AudioHandlerUI(globalAudio);
        _spawner = new Spawner();
        _confirmationUpgrade = new ConfirmationUpgrade(viewEntryScene.PanelGarage.PanelConfirmationUpgrade, _spawner,
            _audioHandlerUI, garageData.GaragePrefabsProvider.SegmentIconUpgradeButtonIconPrefab,
            viewEntryScene.PanelGarage.ButtonBoosterUpgrade.IndicatorSegmentOn,
            viewEntryScene.PanelGarage.ButtonBoosterUpgrade.IndicatorSegmentOff);
        _buttonsUpgradeCar = new ButtonsUpgradeCar(garageData.UpgradeButtonsGarageContent, viewEntryScene.PanelGarage,
            garage, _confirmationUpgrade, _spawner, _audioHandlerUI,
            garageData.GaragePrefabsProvider.SegmentIconUpgradeButtonIconPrefab, garageData.DescriptionsButtonsUpgrades);
        _garageUI = new GarageUI(viewEntryScene.PanelGarage, garage, _buttonsUpgradeCar, _audioHandlerUI);
        _mapPanelHandler = new MapPanelHandler(viewEntryScene.PanelMap, map);
        _audioSettingSwitch = new AudioSettingSwitch(globalAudio,
            viewEntryScene.PanelStartMenu.PanelSettings.MusicTextStatus,
            viewEntryScene.PanelStartMenu.PanelSettings.SoundTextStatus);
        _panelsActivator = new PanelsActivator(viewEntryScene, _garageUI, _sceneSwitch, viewEntryScene.DarkBackground,
            startMenuBackground, map, garage, gameData, _mapPanelHandler, _audioHandlerUI, _audioSettingSwitch);
    }
}