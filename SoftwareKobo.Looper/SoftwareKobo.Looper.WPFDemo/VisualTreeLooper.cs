using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SoftwareKobo.WPFDemo
{
    public class VisualTreeLooper
    {
        private Looper _looper;

        public VisualTreeLooper(DependencyObject obj)
        {
            _looper = new Looper(obj, GetChildren, HasChildren);
        }

        public void Loop(Action<LoopStateManager, DependencyObject> beforeLoopChildren, Action<LoopStateManager, DependencyObject> afterLoopChildren)
        {
            _looper.Loop((arg1, arg2) =>
            {
                if (beforeLoopChildren != null)
                {
                    beforeLoopChildren(arg1, (DependencyObject)arg2);
                }
            }, (arg1, arg2) =>
            {
                if (afterLoopChildren != null)
                {
                    afterLoopChildren(arg1, (DependencyObject)arg2);
                }
            });
        }

        private IEnumerable<object> GetChildren(object arg)
        {
            var obj = (DependencyObject)arg;
            var childCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childCount; i++)
            {
                yield return VisualTreeHelper.GetChild(obj, i);
            }
        }

        private bool HasChildren(object arg)
        {
            var obj = (DependencyObject)arg;
            return VisualTreeHelper.GetChildrenCount(obj) > 0;
        }
    }
}