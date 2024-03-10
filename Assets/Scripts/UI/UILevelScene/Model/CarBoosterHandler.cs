public class CarBoosterHandler
{
    private CustomButton _buttonBoost;
    public Booster Booster { get; private set; }

    public CarBoosterHandler(ViewUILevel viewUILevel, CarInLevel carInLevel)
    {
        Booster = carInLevel.Booster;
        _buttonBoost = viewUILevel.ButtonBoost;
        CheckBoosterButtonInteractable();
    }
    private void CheckBoosterButtonInteractable()
    {
        _buttonBoost.interactable = true;
        Booster.BoosterFuelTank.OnTankEmpty += ButtonOffInteractableBooster;
        Booster.OnBoosterDisable += ButtonOffInteractableBooster;
    }

    private void ButtonOffInteractableBooster()
    {
        Booster.BoosterFuelTank.OnTankEmpty -= ButtonOffInteractableBooster;
        Booster.OnBoosterDisable -= ButtonOffInteractableBooster;
        SetBoosterButtonOffInteractable();
    }
    public void SetBoosterButtonOffInteractable()
    {
        _buttonBoost.interactable = false;
    }
}