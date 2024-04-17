namespace CruZ.GameEngine.GameSystem.UI
{
    /// <summary>
    /// Allow embeded UIControl to a component
    /// </summary>
    public class UIComponent : Component
    {
        public UIComponent()
        {
            EntryControl = new UIControl();
        }

        public UIControl EntryControl { get; private set; }
    }
}
