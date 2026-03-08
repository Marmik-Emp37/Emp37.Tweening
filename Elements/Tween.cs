using System;

using UnityEngine;
using UObject = UnityEngine.Object;

namespace Emp37.Tweening
{
      public abstract class Tween
      {
            // F I E L D S
            protected Func<float, bool> updateFunc;

            private string tag;
            private UObject linkedTarget;
            private Callbacks callbacks;
            private Loop loop;

            private float initialDelay, remainingDelay;

            private Delta timeMode;
            private Phase phase;
            private sbyte direction;

            private bool isInitializationPending, isLinked, isAutoKill, isRecyclable, isRetreating;

            // P R O P E R T I E S
            public string Tag => tag;
            public Phase Phase => phase;
            public abstract bool IsEmpty { get; }

            protected abstract bool CanMoveForward { get; }
            protected abstract bool CanMoveBack { get; }

            private bool IsLinkDead => isLinked && linkedTarget == null;
            private bool IsDead => phase is Phase.Dead;


            internal void Update()
            {
                  if (IsLinkDead)
                  {
                        Kill();
                        return;
                  }

                  float deltaTime = timeMode is Delta.Scaled ? Time.deltaTime : Time.unscaledDeltaTime;

                  if (remainingDelay > 0F)
                  {
                        if ((remainingDelay -= deltaTime) > 0F) return;

                        deltaTime = Math.Abs(remainingDelay);
                        remainingDelay = 0F;
                  }

                  if (isInitializationPending)
                  {
                        isInitializationPending = false;
                        OnInitialize();

                        callbacks.onStart();
                  }

                  int direction = this.direction * loop.Direction;

                  bool complete = updateFunc(deltaTime * direction);
                  callbacks.onUpdate?.Invoke();
                  if (!complete) return;

                  if (isRetreating)
                  {
                        FinishRetreat();
                        return;
                  }

                  bool forward = direction > 0;

                  if (loop.TryAdvance(forward))
                  {
                        OnLoop(loop.Mode, forward);
                        callbacks.onLoopComplete();
                        return;
                  }

                  phase = Phase.Completed;
                  callbacks.onComplete();

                  if (isAutoKill) Kill();
            }

            // C O N T R O L
            /// <summary>Resumes or switches to forward playback from the current position.</summary>
            public void PlayForward()
            {
                  if (IsDead || !CanMoveForward) return;

                  isRetreating = false;
                  direction = 1;

                  phase = Phase.Active;
            }
            /// <summary>Resumes or switches to backward playback from the current position.</summary>
            public void PlayBackward()
            {
                  if (IsDead || !CanMoveBack) return;

                  isRetreating = false;
                  direction = -1;

                  phase = Phase.Active;
            }
            /// <summary>
            /// Restarts the tween from the beginning and plays it forward.
            /// <br>Source and destination values are re-evaluated if <paramref name="rebuild"/> is true.</br>
            /// </summary>
            public void Replay(bool includeDelay = true, bool rebuild = false)
            {
                  if (IsDead) return;

                  direction = 1;
                  if (rebuild) isInitializationPending = true;
                  Reset(includeDelay);

                  phase = Phase.Active;
            }
            /// <summary>
            /// Resets the tween back to its start position so it is ready to be replayed later.
            /// </summary>
            /// <param name="snap">If true, the reset is instant; otherwise the tween plays backward at normal speed until it reaches the start.</param>
            public void Retreat(bool snap = true)
            {
                  if (isInitializationPending || IsDead || !CanMoveBack) return;
                  if (snap)
                  {
                        FinishRetreat();
                        return;
                  }
                  if (isRetreating) return;
                  isRetreating = true;
                  direction = (sbyte) -loop.Direction;
                  phase = Phase.Active;
            }
            public void Kill()
            {
                  if (IsDead) return;

                  phase = Phase.Dead;
                  callbacks.onKill();
                  Clear();

                  if (isRecyclable) OnRecycle();
            }

            internal void Pause()
            {
                  if (phase != Phase.Active) return;

                  phase = Phase.Paused;
                  OnPause();
            }
            internal void Resume()
            {
                  if (phase != Phase.Paused) return;

                  phase = Phase.Active;
                  OnResume();
            }

            // L I F E C Y C L E
            protected virtual void RestoreToDefault()
            {
                  callbacks = Callbacks.Default;
                  initialDelay = remainingDelay = 0F;
                  loop = Loop.Default;
                  timeMode = Delta.Scaled;
                  phase = Phase.Idle;
                  direction = 1;
                  isInitializationPending = isAutoKill = isRecyclable = true;
                  isLinked = isRetreating = false;
            }
            protected virtual void OnPause()
            {
            }
            protected virtual void OnResume()
            {
            }
            protected virtual void Clear()
            {
                  updateFunc = null;
                  tag = null;
                  linkedTarget = null;
                  callbacks = Callbacks.Default;
            }
            protected virtual void OnRecycle()
            {
            }

            protected abstract void OnInitialize();
            protected abstract void OnLoop(LoopType type, bool forward);
            protected abstract void OnReset(bool snapToStart);

            private void Reset(bool includeDelay = true)
            {
                  remainingDelay = includeDelay ? initialDelay : 0F;
                  loop.Reset();

                  OnReset(!isInitializationPending);
            }
            /// <summary>
            /// Resets state to the start position and pauses. The tween remains alive and can be replayed.
            /// </summary>
            private void FinishRetreat()
            {
                  isRetreating = false;
                  direction = 1;
                  Reset();

                  // force pause
                  phase = Phase.Paused;
                  OnPause();

                  callbacks.onRetreat();
            }

            #region I N T E R N A L   C O N F I G
#pragma warning disable IDE1006
            internal void setDelay(float value) => initialDelay = remainingDelay = Mathf.Max(0F, value);
            internal void setTag(string value) => tag = value;
            internal void setTimeMode(Delta mode) => timeMode = mode;
            internal void setAutoKill(bool value) => isAutoKill = value;
            internal void setRecyclable(bool value) => isRecyclable = value;
            internal void setLink(UObject link) { linkedTarget = link; isLinked = link != null; }
            internal void setLooping(int iterations, LoopType type) => loop.Configure(iterations, type);
            internal void setOnStart(Action callback) => callbacks.onStart = callback ?? Callbacks.none;
            internal void setOnUpdate(Action callback) => callbacks.onUpdate = callback;
            internal void setOnRetreat(Action callback) => callbacks.onRetreat = callback ?? Callbacks.none;
            internal void setOnLoopComplete(Action callback) => callbacks.onLoopComplete = callback ?? Callbacks.none;
            internal void setOnComplete(Action callback) => callbacks.onComplete = callback ?? Callbacks.none;
            internal void setOnKill(Action callback) => callbacks.onKill = callback ?? Callbacks.none;
#pragma warning restore IDE1006
            #endregion        
      }
}