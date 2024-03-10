public class BackCarDestructionHandler
{
    private readonly BackWingDestructionHandler _backWingDestructionHandler;
    private readonly BumperDestructionHandler _backBumperDestructionHandler; // ?
    public BackCarDestructionHandler(BackWingDestructionHandler backWingDestructionHandler, BumperDestructionHandler backBumperDestructionHandler)
    {
        _backWingDestructionHandler = backWingDestructionHandler;
        _backBumperDestructionHandler = backBumperDestructionHandler;
    }

    public void DestructNow()
    {
        
    }
}