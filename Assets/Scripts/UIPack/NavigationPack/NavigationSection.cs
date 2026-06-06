using System.Collections.Generic;
using UIPack.NavigationPack.Interface;
using UnityEngine;
using Utils;

namespace UIPack.NavigationPack
{
    public sealed class NavigationSection : INavigationSection
    {
        private readonly CircularEnumerable<INavigationElement> _navigationElements;
        private readonly ENavigationOrientation _orientation;
        private readonly ENavigationOrientation? _navNextDirection;
        private readonly INavigationUI _navigationUI;

        private INavigationElement _selectedElement;

        public bool Enabled { get; private set; } = true;

        public NavigationSection(INavigationUI navigationUI, IEnumerable<INavigationElement> navigationElements, ENavigationOrientation orientation, ENavigationOrientation? navNextDirection = null)
        {
            _navNextDirection = navNextDirection;
            _navigationUI = navigationUI;
            _navigationElements = new CircularEnumerable<INavigationElement>(navigationElements);
            _orientation = orientation;
        }

        public void Select(bool skipToLast = false)
        {
            if (skipToLast) (_navigationElements.Count - 1).Repeat(() => _navigationElements.Next());
            _selectedElement = _navigationElements.Next();
            _selectedElement.OnSelect(_navigationUI);
        }

        public void Reset()
        {
            Deselect();
            _navigationElements.Reset();
        }

        public void Deselect()
        {
            _navigationElements.ForEach(e => e.OnDeselect(_navigationUI));
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            _navigationElements.ForEach(e => ((MonoBehaviour)e).gameObject.SetActive(enabled));
        }

        public void Submit()
        {
            _selectedElement.OnSubmit(_navigationUI);
        }

        public bool HandleNavigation(ENavigationDirection direction) =>
            NavigationHandlingFactory.GetDirectionHandler(direction, this).Invoke();

        public bool LeftNavigation()
        {
            if (_orientation == ENavigationOrientation.VERTICAL)
            {
                if (_navNextDirection == ENavigationOrientation.VERTICAL) return false;
                
                if (_navigationUI.NavigationManager.PreviousSection()) return true;
            }

            if (_navNextDirection == ENavigationOrientation.HORIZONTAL &&
                _navigationElements.LastIndex == _navigationElements.Count - 1 &&
                _navigationUI.NavigationManager.PreviousSection()) return true;

            _navigationElements.ForEach(e => e.OnDeselect(_navigationUI));
            _selectedElement = _navigationElements.Previous();
            _selectedElement.OnSelect(_navigationUI);
            return true;
        }

        public bool RightNavigation()
        {
            if (_orientation == ENavigationOrientation.VERTICAL) 
            {
                if (_navNextDirection == ENavigationOrientation.VERTICAL) return false;

                if (_navigationUI.NavigationManager.NextSection()) return true;
            }
            
            if (_navNextDirection == ENavigationOrientation.HORIZONTAL && 
                _navigationElements.NextIndex == 0 && 
                _navigationUI.NavigationManager.NextSection()) return true;

            _navigationElements.ForEach(e => e.OnDeselect(_navigationUI));
            _selectedElement = _navigationElements.Next();
            _selectedElement.OnSelect(_navigationUI);
            return true;
        }

        public bool UpNavigation()
        {
            if (_orientation == ENavigationOrientation.HORIZONTAL) 
            {
                if (_navNextDirection == ENavigationOrientation.HORIZONTAL) return false;

                if (_navigationUI.NavigationManager.PreviousSection()) return true;
            }

            if (_navNextDirection == ENavigationOrientation.VERTICAL &&
                _navigationElements.LastIndex == _navigationElements.Count - 1 &&
                _navigationUI.NavigationManager.PreviousSection()) return true;

            _navigationElements.ForEach(e => e.OnDeselect(_navigationUI));
            _selectedElement = _navigationElements.Previous();
            _selectedElement.OnSelect(_navigationUI);
            return true;
        }

        public bool DownNavigation()
        {
            if (_orientation == ENavigationOrientation.HORIZONTAL)
            {
                if (_navNextDirection == ENavigationOrientation.HORIZONTAL) return false;

                if (_navigationUI.NavigationManager.NextSection()) return true;
            }

            if (_navNextDirection == ENavigationOrientation.VERTICAL && 
                _navigationElements.NextIndex == 0 &&
                _navigationUI.NavigationManager.NextSection()) return true;

            _navigationElements.ForEach(e => e.OnDeselect(_navigationUI));
            _selectedElement = _navigationElements.Next();
            _selectedElement.OnSelect(_navigationUI);
            return true;
        }
        
        public enum ENavigationOrientation
        {
            HORIZONTAL = 0,
            VERTICAL = 1,
        }
    }
}