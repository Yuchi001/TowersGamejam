using System;
using System.Collections.Generic;
using UIPack.NavigationPack.Interface;

namespace UIPack.NavigationPack
{
    public static class NavigationHandlingFactory
    {
        private static readonly Dictionary<ENavigationDirection, Func<NavigationSection, Func<bool>>> _handlingDict = new()
        {
            { ENavigationDirection.LEFT, nav => nav.LeftNavigation },
            { ENavigationDirection.RIGHT, nav => nav.RightNavigation },
            { ENavigationDirection.UP, nav => nav.UpNavigation },
            { ENavigationDirection.DOWN, nav => nav.DownNavigation },
        };

        public static Func<bool> GetDirectionHandler(ENavigationDirection direction, NavigationSection section) => _handlingDict[direction].Invoke(section);
    }
}