using System.Collections.Generic;
using System.Linq;
using UIPack.NavigationPack.Interface;
using UnityEngine;

namespace UIPack.NavigationPack
{
    public class GridNavigationSection : INavigationSection
    {
        private readonly List<INavigationElement> _elements;
        private readonly int _columnCount;
        private readonly INavigationUI _navigationUI;
        private readonly NavigationSection.ENavigationOrientation _navigationExitDir;
        private int RowCount => Mathf.CeilToInt(_elements.Count / (float)_columnCount);

        private int _index = 0;

        public bool Enabled { get; private set; } = true;
        
        public GridNavigationSection(INavigationUI navigationUI, IEnumerable<INavigationElement> elements, int columnCount, NavigationSection.ENavigationOrientation navExitDir)
        {
            _navigationUI = navigationUI;
            _navigationExitDir = navExitDir;
            _columnCount = columnCount;
            _elements = elements.ToList();
        }

        public void Reset()
        {
            Deselect();
            _index = 0;
        }
        
        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
            _elements.ForEach(e => ((MonoBehaviour)e).gameObject.SetActive(enabled));
        }
        
        public void Select(bool skipToLast = false)
        {
            if (skipToLast) _index = _elements.Count - 1;
            _elements[_index].OnSelect(_navigationUI);
        }

        public void Submit()
        {
            _elements[_index].OnSubmit(_navigationUI);
        }

        public void Deselect()
        {
            _elements[_index].OnDeselect(_navigationUI);
        }

        public bool HandleNavigation(ENavigationDirection direction)
        {
            switch (direction)
            {
                case ENavigationDirection.LEFT:
                    if (_index % _columnCount == 0 && _navigationExitDir == NavigationSection.ENavigationOrientation.HORIZONTAL) return false;
                    _index--;
                    
                    if (_index < 0) _index = _elements.Count - 1;
                    break;
                case ENavigationDirection.RIGHT:
                    if (_index % _columnCount == _columnCount - 1 && _navigationExitDir == NavigationSection.ENavigationOrientation.HORIZONTAL) return false;
                    _index++;
                    
                    if (_index >= _elements.Count) _index = 0;
                    break;
                case ENavigationDirection.UP:
                    if (_index - _columnCount < 0 && _navigationExitDir == NavigationSection.ENavigationOrientation.VERTICAL) return false;
                    _index -= _columnCount;
                    
                    if (_index < 0) _index += _elements.Count * RowCount;
                    if (_index >= _elements.Count) _index -= _columnCount;
                    break;
                case ENavigationDirection.DOWN:
                    if (_index + _columnCount >= _elements.Count && _navigationExitDir == NavigationSection.ENavigationOrientation.VERTICAL) return false;
                    _index += _columnCount;
                    
                    if (_index >= _elements.Count) _index -= _elements.Count * RowCount;
                    if (_index < 0) _index += _columnCount;
                    break;
            }
            
            _elements.ForEach(e => e.OnDeselect(_navigationUI));
            Select();
            return true;
        }
    }
}