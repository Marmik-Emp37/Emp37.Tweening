using System;

using UnityEngine;
using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public abstract class Tween
      {
            internal string tag;
            internal Delta timeMode;
            internal Options options;
            internal Action onStart, onUpdate, onComplete, onKill;
            internal UObject linkedTarget;

            internal Loop loop;
            internal int loopsCompleted;

            public Phase Phase { get; protected set; }
            public bool IsNone => Phase is Phase.None;
            protected bool IsLinkDestroyed => (options & Options.Link) != 0 && linkedTarget == null;
            public abstract bool IsEmpty { get; }

            internal void Update()
            {
                  if (IsLinkDestroyed)
                  {
                        Kill();
                        return;
                  }
                  float delta = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (!OnUpdate(delta)) return;

                  if (loop.Mode != 0 && loopsCompleted < loop.Count)
                  {
                        OnLoop(loop.Mode);
                        loopsCompleted++;
                        return;
                  }

                  Phase = Phase.Completed;
                  TryInvoke(onComplete);

                  if ((options & Options.AutoKill) != 0)
                  {
                        Kill();
                  }
            }
            protected abstract bool OnUpdate(float deltaTime);
            protected abstract void OnLoop(Loop.Type loopType);


            internal void Pause()
            {
                  if (Phase is Phase.Active) OnPause();
            }
            protected virtual void OnPause() => Phase = Phase.Paused;

            internal void Resume()
            {
                  if (Phase is Phase.Paused) OnResume();
            }
            protected virtual void OnResume() => Phase = Phase.Active;

            public void Kill()
            {
                  if (IsNone) return;

                  Phase = Phase.None;
                  TryInvoke(onKill, false);
                  Clear();

                  if ((options & Options.Recycle) != 0) OnRecycle();
            }
            protected virtual void Clear()
            {
                  linkedTarget = null;
                  onStart = onUpdate = onComplete = onKill = null;
            }
            protected abstract void OnRecycle();

            public void Replay()
            {
                  OnReplay();
                  Phase = Phase.Active;
            }
            protected abstract void OnReplay();

            public abstract void Rewind(bool snap = true);

            #region H E L P E R S
            protected bool TryInvoke(Action handler, bool killOnException = false)
            {
                  try
                  {
                        handler?.Invoke();
                        return false;
                  }
                  catch (Exception ex)
                  {
                        Log.Exception(ex, linkedTarget);
                        if (killOnException) Kill();
                        return true;
                  }
            }
            protected bool TryInvoke<T>(Action<T> handler, T argument, bool killOnException = false)
            {
                  try
                  {
                        handler?.Invoke(argument);
                        return false;
                  }
                  catch (Exception ex)
                  {
                        Log.Exception(ex, linkedTarget);
                        if (killOnException) Kill();
                        return true;
                  }
            }
            #endregion
      }
}