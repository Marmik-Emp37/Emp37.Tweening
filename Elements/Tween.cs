using System;

using UnityEngine;
using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      internal struct Callbacks
      {
            internal static readonly Action none = static () => { };
            internal Action onStart, onUpdate, onLoopComplete, onComplete, onKill;

            internal static readonly Callbacks Default = new() { onStart = none, onUpdate = null, onLoopComplete = none, onComplete = none, onKill = none };
      }

      public abstract class Tween
      {
            // T Y P E S
            protected enum PlaybackMode : byte { Forward, Rewind, Static }

            // F I E L D S
            protected Func<float, bool> updateFunc;

            private string tag;
            private UObject linkedTarget;
            private Callbacks callbacks;

            private float delay, delayRemaining;
            private int loopCount, loopsPending;

            private LoopType loopType;
            private Delta timeMode;
            private PlaybackMode activeMode;
            private Phase phase;

            private bool isInitializationPending, isLinked, isAutoKill, isRecyclable;

            // P R O P E R T I E S
            public string MODE => activeMode.ToString();

            public string Tag => tag;
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

                  if (activeMode is PlaybackMode.Rewind)
                  {
                        activeMode = PlaybackMode.Static;
                        ResetToForcePause();
                        return;
                  }
                  if (loopType is not LoopType.None && loopsPending != 0)
                  {
                        if (loopsPending > 0) loopsPending--;
                        OnLoopComplete(loopType);
                        callbacks.onLoopComplete();
                        return;
                  }

                  phase = Phase.Completed;
                  callbacks.onComplete();

                  if (isAutoKill) Kill();
            }

            // C O N T R O L
            public void Replay(bool includeDelay = true, bool rebuild = false)
            {
                  if (phase is Phase.Dead) return;

                  activeMode = PlaybackMode.Forward;
                  Reset(includeDelay);

                  if (rebuild) isInitializationPending = true;
                  if (!isInitializationPending) OnRewind(snap: true);

                  phase = Phase.Active;
            }
            public void Rewind(bool snap = true)
            {
                  if (phase is Phase.Dead || isInitializationPending || activeMode is PlaybackMode.Static) return;
                  if (snap)
                  {
                        activeMode = PlaybackMode.Static;
                        ResetToForcePause();
                  }
                  else
                  {
                        if (activeMode is PlaybackMode.Rewind) return;
                        activeMode = PlaybackMode.Rewind;
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

                  if (activeMode is PlaybackMode.Static) activeMode = PlaybackMode.Forward;
                  phase = Phase.Active;
                  OnResume();
            }

            // L I F E C Y C L E
            protected virtual void RestoreToDefault()
            {
                  tag = null;
                  callbacks = Callbacks.Default;

                  delay = delayRemaining = 0F;
                  loopCount = loopsPending = 0;

                  loopType = LoopType.None;
                  timeMode = Delta.Scaled;
                  activeMode = PlaybackMode.Forward;
                  phase = Phase.Idle;

                  isInitializationPending = isAutoKill = isRecyclable = true;
                  isLinked = false;
            }
            private void Reset(bool includeDelay = true)
            {
                  delayRemaining = includeDelay ? delay : 0F;
                  loopsPending = loopCount;
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
            protected abstract void OnReset();
            protected abstract void OnRewind(bool snap);
            protected abstract void OnRecycle();

            #region I N T E R N A L   C O N F I G
#pragma warning disable IDE1006
            internal void setDelay(float value) => delay = Mathf.Max(0F, value);
            internal void setTag(string value) => tag = value;
            internal void setTimeMode(Delta mode) => timeMode = mode;
            internal void setAutoKill(bool value) => isAutoKill = value;
            internal void setRecyclable(bool value) => isRecyclable = value;
            internal void setLink(UObject link) { linkedTarget = link; isLinked = link != null; }
            internal void setLooping(int count, LoopType type) { loopCount = count > 0 ? count : -1; loopType = type; }
            internal void setOnStart(Action callback) => callbacks.onStart = callback ?? Callbacks.none;
            internal void setOnUpdate(Action callback) => callbacks.onUpdate = callback;
            internal void setOnLoopComplete(Action callback) => callbacks.onLoopComplete = callback ?? Callbacks.none;
            internal void setOnComplete(Action callback) => callbacks.onComplete = callback ?? Callbacks.none;
            internal void setOnKill(Action callback) => callbacks.onKill = callback ?? Callbacks.none;
#pragma warning restore IDE1006 // Naming Styles
            #endregion

            protected void ResetToForcePause()
            {
                  Reset();

                  phase = Phase.Paused;
                  OnPause();
            }
      }
}