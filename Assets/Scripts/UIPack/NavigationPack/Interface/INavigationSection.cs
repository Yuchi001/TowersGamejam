namespace UIPack.NavigationPack.Interface
{
    public interface INavigationSection
    {
        public void Select(bool skipToLast = false);
        public void Reset();
        public void Submit();
        public void Deselect();
        public void SetEnabled(bool enabled);
        public bool Enabled { get; }
        public bool HandleNavigation(ENavigationDirection direction);
    }
}