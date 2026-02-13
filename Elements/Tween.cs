using System;

using UnityEngine;
using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public abstract class Tween
      {
            protected enum PlaybackMode : byte
            {
                  Normal, Rewind
            }

            protected PlaybackMode playbackMode;

            internal string tag;
            internal Delta timeMode;
            internal Options options;

            internal UObject linkedTarget;
            internal Action onStart, onUpdate, onStepComplete, onComplete, onKill;

            internal Loop loop;
            internal int loopsCompleted;

            public Phase Phase { get; protected set; }
            public bool IsNone => Phase is Phase.None;
            protected bool IsLinkDestroyed => (options & Options.Link) != 0 && linkedTarget == null;
            public abstract bool IsEmpty { get; }

            protected virtual void Reset()
            {
                  playbackMode = PlaybackMode.Normal;

                  tag = null;
                  timeMode = Delta.Scaled;
                  options = Options.AutoKill | Options.Recycle;

                  loop = Loop.Default;
                  loopsCompleted = 0;

                  Phase = Phase.None;
            }

            internal void Update()
            {
                  if (IsLinkDestroyed)
                  {
                        Kill();
                        return;
                  }
                  float delta = (timeMode == Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (!OnUpdate(delta)) return;
                  OnStepComplete(playbackMode);
                  TryInvoke(onStepComplete);
                  switch (playbackMode)
                  {
                        case PlaybackMode.Normal:
                              {
                                    if (loop.Mode != 0 && loopsCompleted < loop.Count)
                                    {
                                          OnLoop(loop.Mode);
                                          loopsCompleted++;
                                          return;
                                    }
                                    Phase = Phase.Completed;
                                    TryInvoke(onComplete);
                              }
                              break;

                        case PlaybackMode.Rewind:
                              {
                                    playbackMode = PlaybackMode.Normal;
                                    Phase = Phase.Paused;
                              }
                              break;
                  }
                  if ((options & Options.AutoKill) != 0)
                  {
                        Kill();
                  }
            }
            protected abstract bool OnUpdate(float deltaTime);
            protected abstract void OnLoop(Loop.Type loopType);
            protected virtual void OnStepComplete(PlaybackMode mode) { }

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
                  onStart = onUpdate = onStepComplete = onComplete = onKill = null;
                  linkedTarget = null;
            }
            protected abstract void OnRecycle();

            public void Replay()
            {
                  playbackMode = PlaybackMode.Normal;
                  OnReplay();
                  Phase = Phase.Active;
            }
            protected abstract void OnReplay();

            public virtual void Rewind(bool snap = true)
            {

            }

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