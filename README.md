# About
A fast, composable tweening framework for Unity with clean architecture and zero-allocation at runtime.

> **Version: 2.1.0**


# Features
+ Allocation free during updates - pooled tweens minimize GC while running.  
+ Type safe tweening - supports any type (struct or class) with custom interpolation.
+ Fluent API - chain calls to build complex tweens concisely.
+ Composable animations - allows combining tween elements into sequences or parallel groups.
+ Extensions - includes helpers for common Unity components.
+ Tag based control - pause, resume, or kill tweens by tag via `Factory`.
+ Lazy execution - tweens initialize only when played.
+ Safe callbacks - all actions wrapped with exception handling.
+ Compatible with Enter Play Mode options.


# Installation
Clone or download this repository and copy the **`Emp37.Tweening`** folder into your project.

**Requirements:** Unity 2021.3 LTS or newer.


# Quick Start
```csharp
using Emp37.Tweening;

// simple fade-out animation
canvasGroup.TweenFade(0F, 1.5F).Play();

// move tween with easing and callback
transform.TweenMoveY(5F, 2F)
      .setEase(Ease.Type.OutBounce)
      .onComplete(() => Debug.Log("Relocation complete!"))
      .Play();

// scale tween with a spring animation and tag
transform.TweenScale(Vector3.one * 2F, 1F)
      .setEase(Ease.Curves.Spring)
      .Play()
      .WithTag("UI");
```


# Core Concepts
## Lazy Execution
> [!IMPORTANT]
> Tweens are **lazy** - they’re configured first and only start when played.

```csharp
// configure tween
var tween = transform.TweenScale(Vector3.one * 2F, 1F).setEase(Ease.Type.OutElastic).setTimeMode(Delta.Unscaled);

// start manually
tween.Play();        // play directly
Factory.Play(tween); // play through the global Factory

// assign an optional tag
tween.WithTag("UI-Intro");

// or chain .Play() for immediate execution
transform.TweenMove(target, 2F).setEase(Ease.Type.OutBack).Play();
```

## Element Types
Every tween is based on the `ITween` interface, which defines the lifecycle shared by all tween types.
```
IElement
├── Value<T> // concrete tweening
├── Sequence // sequential composition
├── Parallel // concurrent composition
├── Delay    // timing control
└── Callback // callback execution
```
---
### Value\<T> → Interpolates any struct value between a and b over time
```csharp
// built-in types (float, Vector3, Color, etc.)
Tween.Value(light, () => light.intensity, 2F, 0.5F, v => light.intensity = v)
     .setEase(Ease.Curves.Snappy)
     .onComplete(() => Debug.Log("Glow complete"))
     .Play();

// custom types with evaluator
Tween.Value<CustomStruct>(obj, () => start, target, 2F, v => Apply(v), (a, b, t) => CustomStruct.Lerp(a, b, t))
     .setRecyclable(true)
     .Play();

// example using extension
transform.TweenRotateX(20F, 1F).setLoop(2, LoopType.Restart).Snap(5F).Play();
```
| Method                                                      | Description                                                     |
| ----------------------------------------------------------- | --------------------------------------------------------------- |
| **addModifier(Modifier modifier)**                          | Modifies the processed value before it is applied               |
| **setEase(Type type)**                                      | Sets an easing function using a built-in `Ease.Type`            |
| **setEase(AnimationCurve curve)**                           | Uses a custom `AnimationCurve` for easing                       |
| **setEase(Function func)**                                  | Assigns a custom easing delegate (`float → float`)              |
| **setDelay(float seconds)**                                 | Adds a startup delay before tween begins                        |
| **setLoop(int cycles, LoopType type, float interval = 0F)** | Loops tween a number of times (use -1 for infinite)             |
| **disableLoop()**                                           | Stops looping after the current iteration                       |
| **setProgress(float normalizedValue)**                      | Sets progress manually (0–1)                                    |
| **setRecyclable(bool enabled)**                             | Controls reuse via internal object pooling (enabled by default) |
| **setTarget(T value)**                                      | Overrides the target value after creation                       |
| **setTimeMode(Delta mode)**                                 | Chooses between scaled or unscaled time                         |
| **onStart(Action)**                                         | Invoked once when tween starts                                  |
| **onUpdate(Action<float>)**                                 | Called each frame with eased progress (0–1)                     |
| **onComplete(Action)**                                      | Invoked when tween reaches its end                              |
| **onKill(Action)**                                          | Invoked when tween is manually killed                           |
| **onConclude(Action)**                                      | Called when tween concludes (completed or killed)               |
| **Pause() / Resume() / Kill()**                             | Lifecycle controls                                              |
---
### Sequence → Executes tweens sequentially
```csharp
// constructor syntax
// onboarding tooltip: fade in panel, slide text, then play sound
Tween.Sequence(
      tooltipBackground.TweenFade(1F, 0.3F),
      tooltipText.TweenMoveY(tooltipText.transform.localPosition.y + 50F, 0.4F).setEase(Ease.Type.OutCubic),
      Tween.Callback(() => audioSource.PlayOneShot(popupClip))
).Play();

// fluent builder syntax
// level complete flow: fade in overlay, bounce star icons one by one, then show continue button
Tween.Sequence()
    .Append(overlay.TweenFade(1F, 0.4F))
    .Append(stars.ShowUp)
    .Append(continueButton.TweenFade(1F, 0.5F))
    .Play();

// then-chaining syntax
// barrel explosion sequence: wait, flash red, explode
Tween.Delay(0.5F).Then(transform.TweenColor(Color.red, 0.2F)).Then(Tween.Callback(barrel.Explode)).Play();
```
---
### Parallel → Runs tweens simultaneously
```csharp
// UI reveal: fade in panel, slide title, and play sound all at once
Tween.Parallel(
    image.TweenFade(1F, 0.3F),
    text.TweenScale(Vector3.one, 0.4F, relative: true).setEase(Ease.Type.OutCubic),
    Tween.Callback(() => audio.PlayOneShot(pop))
).Play();
```
---
### Delay → Waits for time or a condition
```csharp
// time-based
Tween.Delay(2F);

// predicate-based
Tween.Delay(() => Input.anyKeyDown);

// combined
Tween.Delay(1F, () => Input.GetKeyDown(KeyCode.Space));
```
---
### Callback → Executes an action inline within a tween chain
```csharp
Tween.Sequence(
      explosive.TweenScale(Vector3.zero, 0.3F)).Append(
      Tween.Callback(() =>
      {
          Instantiate(explosionPrefab, explosive.position, Quaternion.identity);
          Destroy(explosive.gameObject);
      })
).Play();
```

## Easing & Animation
Use from built-in presets, expressive curve types, or provide your own AnimationCurve.
```csharp
// built-in easing Types
Linear, Sine, Cubic, Quint, Circ, Elastic, Quad, Quart, Expo, Back, Bounce

// curve presets
Anticipate, Pop, Punch, Shake, Snappy, Spring
```
Each easing type also supports directional variants:
+ **In** → starts slowly, speeds up
+ **Out** → starts quickly, slows down
+ **InOut** → combines both (ease-in then ease-out)

## Lifecycle Control
- Tweens automatically remove themselves when complete or killed.
- The Factory dynamically expands internal capacity if you exceed the default limit.
- Destroyed Unity objects auto-kill their tweens. No null checks needed. 

### Individual Tween
```csharp
var tween = transform.TweenScale(Vector3.one * 2F, 1F);

// control execution
tween.Play();   // start the tween
tween.Pause();  // pause it in place
tween.Resume(); // continue from where it paused
tween.Kill();   // stop immediately and mark as None
```
### Global
Use the `Factory` to affect all active tweens across the game.
```csharp
Factory.Pause();  // pause every tween
Factory.Resume(); // resume all paused tweens
Factory.Kill();   // kill all tweens instantly

// group tweens with a tag and manage them collectively
// this is useful for pausing UI animations during gameplay, or killing all enemy tweens on death

// play with a tag
transform.TweenMove(Vector3.one * 5F, 2F).Play().WithTag("UI");

// later, control by tag
Factory.Pause("UI");
Factory.Resume("Player");
Factory.Kill("Enemy");
```


# Advanced Features
## Snapping
Snap tweens to fixed increments for deterministic motion or stylized effects. 
Useful for grid-based movement, pixel-perfect UI, or stepped animations.
```csharp
// pixel-perfect movement
element.TweenMove(target, 0.5F).Snap(1F).Play();

// integer counter (no decimals)
scoreText.TweenNumber(100, 2F, "0", relative: true).Snap(1F).setEase(Ease.Type.OutExpo).Play();

// per-axis snapping as x = 1, y = 0.5, z = 0.25
transform.TweenMove(target, 2F).Snap(new Vector3(1F, 0.5F, 0.25F)).setEase(Ease.Type.OutQuad).Play();
```

## Relative Tweens
Most extension methods support relative tweening, allowing values to be applied as offsets from the current state rather than absolute targets.
```csharp
transform.TweenMoveY(2F, 1F, relative: true).Play();

// grow by 50% from current scale
transform.TweenScale(transform.localScale * 0.5F, 1F, relative: true).setEase(Ease.Curves.Pop).Play();
```

## Value Modifiers
Value tweens can be extended with modifiers, which transform the interpolated value before it is applied.<br>
Modifiers are composable and executed in the order they are added.
```csharp
tween
    .addModifier(value => Mathf.Abs(value)) // always keep positive
    .addModifier(value => Mathf.Clamp(value, minVal, maxVal)); // clamp to bounds
```


# Debugging
## Tween Debugger Window
Open **Tools > Emp37 > Tweening.Debugger** to monitor all active tweens in real-time.

**Features:**
- Live statistics by type and phase.
- Per-tween details (duration, easing, callbacks, etc).
- Realtime progress bar with color coding:
  - 🟢 Green - Active
  - 🟠 Orange - Paused
  - 🔘 Gray - Inactive
- Tag-based search filtering.
- Pause/Resume/Kill controls per tween or globally.
- Completion history for analysis.

**Usage:**
```csharp
// tag tweens for easier debugging
transform.TweenMove(target, 2F).Play().WithTag("Player");
```
Then search "Player" in debugger to filter tags.

## Logging
```csharp
// selective logging
Log.Info("Tween system initialized");
Log.Warning("Tween capacity reached");
Log.Error("Failed to create tween");

Log.Enabled = false; // disable all logs
```


# Tips
- Ensure you're calling `.Play()`.
- Tweens capture initial values on **Play**, not when created.
- Avoid multiple active tweens modifying the same property.
- Group related tweens with `.WithTag()` to pause/resume/kill them together.
- If you store a tween reference and `setRecyclable(true)` (default), you **must** nullify the reference in `.onConclude()` to avoid using a recycled instance:
```csharp
// ❌ WRONG - tween reference may be reused by pool
ITween myTween = transform.TweenMoveX(5F, 1F).Play();

// later: myTween.Pause(); may affect a different tween
  
// ✅ CORRECT - nullify on conclude
myTween.onConclude(() => myTween = null);

// OR disable recycling if you need persistent reference
myTween.setRecyclable(false);
```
- Tweens automatically clean up when their linked Unity object is destroyed. No manual `.Kill()` needed.
- The Factory starts with capacity for 128 tweens and auto-expands with a warning. Monitor debugger to detect leaks.
- Set `Log.Enabled = false` in production to eliminate debug overhead.
- Delay, Callback, Parallel, and Sequence can nest arbitrarily for complex choreography.
