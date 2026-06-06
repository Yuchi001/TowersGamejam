namespace UIPack.NavigationPack.Interface
{
    public interface INavigationElement
    {
        public void OnSelect(INavigationUI parentUI);
        public void OnDeselect(INavigationUI parentUI);
        public void OnSubmit(INavigationUI parentUI);
    }
}