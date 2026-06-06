using System.Collections.Generic;
using System.Linq;
using UIPack.NavigationPack.Interface;
using Utils;

namespace UIPack.NavigationPack
{
    public class NavigationManager
    {
        private readonly List<INavigationSection> _sections;
        private INavigationSection _focusedSection;

        public NavigationManager(INavigationSection mainSection, params INavigationSection[] sections)
        {
            _sections = new List<INavigationSection>(sections);
            _sections.Insert(0, mainSection);
            _focusedSection = _sections[0];
            _focusedSection.Select();
        }

        public void HandleSubmit()
        {
            _focusedSection.Submit();
        }

        public void ResetSelection()
        {
            _focusedSection.Reset();
        }

        public void SelectCurrentSection()
        {
            _focusedSection.Select();
        }

        public void HandleCycleSection()
        {
            var current = _sections.IndexOf(_focusedSection);
            current++;
            if (current >= _sections.Count) current = 0;

            _focusedSection.Deselect();
            _focusedSection = _sections[current];
            _focusedSection.Select();
        }

        public bool NextSection()
        {
            var currentIndex = _sections.IndexOf(_focusedSection);
            currentIndex++;
            if (currentIndex >= _sections.Count) currentIndex = 0;
            
            var nextSection = _sections[currentIndex];
            if (!nextSection.Enabled) return false;
            
            _focusedSection.Reset();
            _focusedSection = nextSection;
            _focusedSection.Select();

            return true;
        }

        public bool PreviousSection()
        {
            var currentIndex = _sections.IndexOf(_focusedSection);
            currentIndex--;
            if (currentIndex < 0) currentIndex = _sections.Count - 1;
            
            var previousSection = _sections[currentIndex];
            if (!previousSection.Enabled) return false;
            
            _focusedSection.Reset();
            _focusedSection = previousSection;
            _focusedSection.Select(true);

            return true;
        }
        
        public void HandleNavigation(ENavigationDirection direction)
        {
            var success = _focusedSection.HandleNavigation(direction);
            if (success) return;

            if (direction == ENavigationDirection.DOWN || direction == ENavigationDirection.RIGHT) NextSection();
            else PreviousSection();
        }
    }
}