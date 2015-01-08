using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareKobo
{
    public class Looper
    {
        private readonly Func<object, IEnumerable<object>> _getChildren;
        private readonly Func<object, bool> _hasChildren;
        private readonly object _loopObject;

        public Looper(object loopObject, Func<object, IEnumerable<object>> getChildren, Func<object, bool> hasChildren)
        {
            if (loopObject == null)
            {
                throw new ArgumentNullException(nameof(loopObject));
            }
            if (getChildren == null)
            {
                throw new ArgumentNullException(nameof(getChildren));
            }
            if (hasChildren == null)
            {
                throw new ArgumentNullException(nameof(hasChildren));
            }
            _loopObject = loopObject;
            _getChildren = getChildren;
            _hasChildren = hasChildren;
        }

        public void Loop(Action<LoopStateManager, object> beforeLoopChildren, Action<LoopStateManager, object> afterLoopChildren, Action betweenChildren)
        {
            var loopStateManager = new LoopStateManager();

            InnerLoop(_loopObject, loopStateManager, _getChildren, _hasChildren, beforeLoopChildren, afterLoopChildren, betweenChildren);
        }

        public void Loop(Action<LoopStateManager, object> beforeLoopChildren, Action<LoopStateManager, object> afterLoopChildren)
        {
            Loop(beforeLoopChildren, afterLoopChildren, null);
        }

        private void InnerLoop(object loopObject, LoopStateManager loopStateManager, Func<object, IEnumerable<object>> getChildren, Func<object, bool> hasChildren, Action<LoopStateManager, object> beforeLoopChildren, Action<LoopStateManager, object> afterLoopChildren, Action betweenChildren)
        {
            if (loopStateManager.LoopState == LoopState.Return)
            {
                return;
            }

            if (hasChildren(loopObject))
            {
                var children = getChildren(loopObject).ToList();
                for (int i = 0, count = children.Count; i < count; i++)
                {
                    var child = children[i];

                    if (beforeLoopChildren != null)
                    {
                        beforeLoopChildren(loopStateManager, child);
                    }

                    if (loopStateManager.LoopState == LoopState.Break)
                    {
                        loopStateManager.Reset();
                        break;
                    }
                    else if (loopStateManager.LoopState == LoopState.Continue)
                    {
                        loopStateManager.Reset();
                        if (betweenChildren != null && i != count - 1)
                        {
                            betweenChildren();
                        }
                        continue;
                    }
                    else if (loopStateManager.LoopState == LoopState.Return)
                    {
                        return;
                    }

                    loopStateManager.Deep++;
                    InnerLoop(child, loopStateManager, getChildren, hasChildren, beforeLoopChildren, afterLoopChildren, betweenChildren);
                    loopStateManager.Deep--;

                    if (loopStateManager.LoopState == LoopState.Return)
                    {
                        return;
                    }

                    if (afterLoopChildren != null)
                    {
                        afterLoopChildren(loopStateManager, child);
                    }

                    if (loopStateManager.LoopState == LoopState.Break)
                    {
                        loopStateManager.Reset();
                        break;
                    }
                    else if (loopStateManager.LoopState == LoopState.Continue)
                    {
                        if (betweenChildren != null && i != count - 1)
                        {
                            betweenChildren();
                        }
                        continue;
                    }
                    else if (loopStateManager.LoopState == LoopState.Return)
                    {
                        return;
                    }

                    if (betweenChildren != null && i != count - 1)
                    {
                        betweenChildren();
                    }
                }
            }
        }
    }
}