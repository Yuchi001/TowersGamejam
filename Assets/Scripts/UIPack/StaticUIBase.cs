namespace UIPack
{
    public abstract class StaticUIBase : UIBase
    {
        public sealed override bool OnEscape()
        {
            return false; // never close
        }
    }
}