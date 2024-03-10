using UnityEngine;
public class ModelEntryScene
{
    private SceneSwitch _sceneSwitch;
    private ButtonsUpgradeCar _buttonsUpgradeCar;
    private GarageUI _garageUI;
    private Garage _garage;
    private Map _map;
    private PanelsActivator _panelsActivator;
    private ConfirmationUpgrade _confirmationUpgrade;
    private MapPanelHandler _mapPanelHandler;
    private AudioHandlerUI _audioHandlerUI;
    private AudioSettingSwitch _audioSettingSwitch;
    private Spawner _spawner;
    public ModelEntryScene(Garage garage, Map map, SpriteRenderer startMenuBackground, ViewEntryScene viewEntryScene,
        GameData gameData, GarageData garageData, GlobalAudio globalAudio, PlayerDataHandler playerDataHandler)
    {
        _sceneSwitch = new SceneSwitch(playerDataHandler, gameData);
        _audioHandlerUI = new AudioHandlerUI(globalAudio);
        _spawner = new Spawner();
        _garage = garage;
        _map = map;
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
            startMenuBackground, gameData, _mapPanelHandler, _audioHandlerUI, _audioSettingSwitch);
    }
}