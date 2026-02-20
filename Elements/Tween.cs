using System;

using UnityEngine;
using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      internal struct Callbacks
      {
            internal static readonly Action none = static () => { };
            internal Action onStart, onUpdate, onRewind, onLoopComplete, onComplete, onKill;

            internal static readonly Callbacks Default = new() { onStart = none, onUpdate = null, onRewind = none, onLoopComplete = none, onComplete = none, onKill = none };
      }

      public abstract class Tween
      {
            // T Y P E S
            protected enum PlaybackMode { None, Forward, Backward, Rewind }

            // F I E L D S
            protected Func<float, bool> updateFunc;

            private string tag;
            private UObject linkedTarget;
            private Callbacks callbacks;

            private float initialDelay, delayRemaining;
            private int loopCount, incompleteLoops;

            private LoopType loopType;
            private Delta timeMode;
            private PlaybackMode _mode;
            private Phase phase;

            private bool isInitializationPending, isLinked, isAutoKill, isRecyclable;
            private bool pauseOnComplete;

            // P R O P E R T I E S
            public string Tag => tag;
            protected PlaybackMode Mode
            {
                  get => _mode;
                  set
                  {
                        if (value == _mode) return;

                        _mode = value;
                        OnPlaybackChange(_mode);
                  }
            }
            public Phase Phase => phase;
            protected bool IsLinkDead => isLinked && linkedTarget == null;
            public abstract bool IsEmpty { get; }


            internal void Update()
            {
                  if (IsLinkDead)
                  {
                        Kill();
                        return;
                  }
                  float deltaTime = (timeMode is Delta.Unscaled) ? Time.unscaledDeltaTime : Time.deltaTime;
                  if (delayRemaining > 0F)
                  {
                        delayRemaining -= deltaTime;

                        if (delayRemaining is > 0F) return;
                        deltaTime = -delayRemaining;
                        delayRemaining = 0F;
                  }
                  if (isInitializationPending)
                  {
                        isInitializationPending = false;
                        OnInitialize();

                        callbacks.onStart();
                  }
                  if (!updateFunc(deltaTime))
                  {
                        callbacks.onUpdate?.Invoke();
                        return;
                  }

                  if (pauseOnComplete)
                  {
                        pauseOnComplete = false;
                        Mode = PlaybackMode.None;
                        Reset();
                        phase = Phase.Paused;
                        OnPause();
                        callbacks.onRewind();
                        return;
                  }

                  if (loopType != LoopType.None && incompleteLoops != 0)
                  {
                        if (incompleteLoops > 0) incompleteLoops--;
                        OnLoopComplete(loopType);
                        callbacks.onLoopComplete();
                        return;
                  }
                  phase = Phase.Completed;
                  callbacks.onComplete();

                  if (isAutoKill) Kill();
            }

            // C O N T R O L
            public void PlayForward()
            {
                  if (Mode is PlaybackMode.Forward || phase is Phase.Completed or Phase.Dead) return;

                  pauseOnComplete = false;
                  Mode = PlaybackMode.Forward;
                  phase = Phase.Active;
            }
            public void PlayBackwards()
            {
                  if (Mode is PlaybackMode.None or PlaybackMode.Backward || phase is Phase.Completed or Phase.Dead) return;

                  pauseOnComplete = false;
                  Mode = PlaybackMode.Backward;
                  phase = Phase.Active;
            }
            public void Replay(bool includeDelay = true, bool rebuild = false)
            {
                  if (phase is Phase.Dead) return;

                  Mode = PlaybackMode.Forward;
                  Reset(includeDelay);

                  if (rebuild) isInitializationPending = true;
                  if (!isInitializationPending) OnRewind(snap: true);

                  phase = Phase.Active;
            }
            public void Rewind(bool snap = true)
            {
                  if (phase is Phase.Dead || isInitializationPending || Mode is PlaybackMode.None) return;
                  if (snap)
                  {
                        pauseOnComplete = false;

                        Mode = PlaybackMode.None;
                        Reset();

                        phase = Phase.Paused;
                        OnPause();

                        callbacks.onRewind();
                  }
                  else
                  {
                        if (Mode is PlaybackMode.Rewind) return;

                        pauseOnComplete = true;
                        Mode = PlaybackMode.Rewind;
                        phase = Phase.Active;
                  }
                  OnRewind(snap);
            }
            public void Kill()
            {
                  if (phase is Phase.Dead) return;

                  phase = Phase.Dead;
                  callbacks.onKill();
                  Clear();

                  if (isRecyclable) OnRecycle();
            }

            internal void Pause()
            {
                  if (phase is not Phase.Active) return;

                  phase = Phase.Paused;
                  OnPause();
            }
            internal void Resume()
            {
                  if (phase is not Phase.Paused) return;

                  if (Mode is PlaybackMode.None) Mode = PlaybackMode.Forward;

                  phase = Phase.Active;
                  OnResume();
            }

            // L I F E C Y C L E
            protected virtual void RestoreToDefault()
            {
                  initialDelay = delayRemaining = 0F;
                  loopCount = incompleteLoops = 0;
                  loopType = LoopType.None;
                  timeMode = Delta.Scaled;
                  _mode = PlaybackMode.None;
                  phase = Phase.Idle;
                  isInitializationPending = isAutoKill = isRecyclable = true;
                  isLinked = false;
                  pauseOnComplete = false;
                  Clear();
            }
            private void Reset(bool includeDelay = true)
            {
                  delayRemaining = includeDelay ? initialDelay : 0F;
                  incompleteLoops = loopCount;
                  OnReset();
            }
            protected virtual void OnPause()
            {
            }
            protected virtual void OnResume()
            {
            }
            protected virtual void Clear()
            {
                  tag = null;
                  linkedTarget = null;
                  callbacks = Callbacks.Default;
                  updateFunc = null;
            }

            protected abstract void OnInitialize();
            protected abstract void OnLoopComplete(LoopType loopType);
            protected abstract void OnPlaybackChange(PlaybackMode mode);
            protected abstract void OnRewind(bool snap);
            protected abstract void OnReset();
            protected abstract void OnRecycle();

            #region I N T E R N A L   C O N F I G
#pragma warning disable IDE1006
            internal void setDelay(float value) => initialDelay = Mathf.Max(0F, value);
            internal void setTag(string value) => tag = value;
            internal void setTimeMode(Delta mode) => timeMode = mode;
            internal void setAutoKill(bool value) => isAutoKill = value;
            internal void setRecyclable(bool value) => isRecyclable = value;
            internal void setLink(UObject link) { linkedTarget = link; isLinked = link != null; }
            internal void setLooping(int count, LoopType type) { loopCount = count > 0 ? count : -1; loopType = type; }
            internal void setOnStart(Action callback) => callbacks.onStart = callback ?? Callbacks.none;
            internal void setOnUpdate(Action callback) => callbacks.onUpdate = callback;
            internal void setOnRewind(Action callback) => callbacks.onRewind = callback;
            internal void setOnLoopComplete(Action callback) => callbacks.onLoopComplete = callback ?? Callbacks.none;
            internal void setOnComplete(Action callback) => callbacks.onComplete = callback ?? Callbacks.none;
            internal void setOnKill(Action callback) => callbacks.onKill = callback ?? Callbacks.none;
#pragma warning restore IDE1006 // Naming Styles
            #endregion        
      }
}