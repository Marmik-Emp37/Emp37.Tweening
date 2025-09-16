## About
A modern, high-performance, zero-allocation tweening library for Unity that emphasizes clean architecture, developer experience, and runtime efficiency.
Build animations from simple, composable building blocks and combine them into sophisticated flows.

**Version: 2.0.0**

## Features
+ Zero GC during updates - uses allocation-free data structures to avoid runtime garbage collection.
+ Type-safe tweening - supports tweening of any struct type with custom evaluator.
+ Fluent API - chain calls to build complex tweens concisely.
+ Composable animations - allows combining tween elements into sequences or parallel groups.
+ Extensive extensions - includes methods for common Unity components.
+ Tag-based control - start, pause, or stop groups of tweens using tags.
+ Lazy execution - tweens don't consume resources until played.
+ IntelliSense-friendly - includes comprehensive XML documentation.
+ Enter Play Mode Settings compatibility - functions as intended with any option.
  
## Installation
Clone or download this repository and copy the `Emp37.Tweening` folder into your Unity project.

**Requirements:** Unity 2021.3 LTS or newer.

## Quick Start
```csharp
using Emp37.Tweening;

// simple fade-out animation
canvasGroup.TweenFade(0F, 1.5F).Play();

// move with easing and callback
transform.TweenMoveY(5F, 2F).SetEase(Ease.Type.OutBounce).OnComplete(() => Debug.Log("Relocation complete!")).Play();

// scale with a spring animation curve and play the tween with a tag
transform.TweenScale(Vector3.one * 2F, 1F).SetEase(Ease.Curves.Spring).PlayWithTag("UI");
```

## Core Concepts
### Lazy Execution
> [!IMPORTANT]
> All tweens are **lazy** - they're configured but don't start until explicitly played.

```csharp
// configure tween
var tween = transform.TweenScale(Vector3.one * 2F, 1F).SetEase(Ease.Type.OutElastic).SetTimeMode(Delta.Unscaled);

// start it when needed using
tween.Play(); // option 1: play directly on the tween instance
tween.PlayWithTag("TagName"); // option 2: play and associate with a tag for group control
Factory.Play(tween); // option 3: play through the Factory

// or chain .Play() for immediate execution
transform.TweenMove(target, 2F).Play();
```

### Element Types
> [!NOTE]
> Every animation is built on the `IElement` interface, which defines the lifecycle shared by all tween types.

```
IElement
├── Value<T> // concrete tweening
├── Sequence // sequential composition
├── Parallel // concurrent composition
├── Delay    // timing control
└── Invoke   // callback execution
```
---------------
#### Value\<T> → tween any struct between two values over time
```csharp
// built-in primitive types already have optimized evaluators included in the library, no need to provide one
var intensityTween = Tween.Value(link: spotlight, init: () => spotlight.intensity, target: 2F, duration: 0.5F, update: value => spotlight.intensity = value);

// for custom struct types, you must supply an evaluator function that defines how to interpolate between two values of your type.
Tween.Value<CustomStruct>(unityObject, () => current, target, 2F, value => current = value, evaluator: (a, b, t) => CustomStruct.Lerp(a, b, t));
```
`Value<T>` tweens can be repeated using the `Loop` struct. Configure the loop **mode**, **cycle count**, and optional **interval** between each cycle.
```csharp
// restart 3 extra times (total 4 plays)
intensityTween.SetLoop(new Loop(mode: Loop.Type.Restart, count: 3));

// set count to -1 for infinite loop
intensityTween.SetLoop(new Loop(Loop.Type.Yoyo, -1));

// loop with a delay between cycles
intensityTween.SetLoop(new Loop(Loop.Type.Restart, 5, interval: 2F));
```
---------------
#### Sequence → chain multiple elements to execute sequentially</br>
There are 3 approaches to create a sequence:
```csharp
// 1. constructor syntax
// onboarding tooltip: fade in panel, slide text, then play sound
Tween.Sequence(tooltipBackground.TweenFade(1F, 0.3F), tooltipText.TweenMoveY(tooltipText.transform.localPosition.y + 50F, 0.4F).SetEase(Ease.Type.OutCubic), Tween.Invoke(() => audioSource.PlayOneShot(popupClip))).Play();

// 2. fluent building syntax
// level complete flow: fade in overlay, bounce star icons one by one, then show continue button
Tween.Sequence()
    .Append(overlay.TweenFade(1F, 0.4F))
    .Append(stars.ShowUp)
    .Append(continueButton.TweenFade(1F, 0.5F))
    .Play();

// 3. then-chaining syntax
// barrel explosion sequence: wait, flash red, explode
Tween.Delay(0.5F).Then(transform.TweenColor(Color.red, 0.2F)).Then(Tween.Invoke(barrel.Explode)).Play();
```
---------------
#### Parallel → run multiple elements simultaneously
```csharp
// UI reveal: fade in panel, slide title, and play sound all at once
Tween.Parallel(panel.TweenFade(1F, 0.3F), title.TweenScale(Vector3.one, 0.4F).SetEase(Ease.Type.OutCubic), Tween.Invoke(() => audioSource.PlayOneShot(revealClip))).Play();
```
---------------
#### Delay → wait for time or a condition
```csharp
// time-based delay
Tween.Delay(3F);

// condition-based delay
Tween.Delay(() => Input.anyKeyDown);

// combined: wait 1s AND for player jump
Tween.Delay(1F, () => Input.GetKeyDown(KeyCode.Space));

// practical example: tutorial prompt
Tween.Sequence()
    .Append(tutorialPanel.TweenFade(1F, 0.175F))
    .Append(Tween.Delay(2F).Then(Tween.Invoke(() => promptText.text = "Press any key to continue..."))) //  nested sequence - show for 2s and update propmpt
    .Append(Tween.Delay(() => Input.anyKeyDown)) // wait for input
    .Append(tutorialPanel.TweenFade(0F, 0.4F).SetEase(Ease.Curves.Snappy)) // fade-out
    .Play();
```
---------------
#### Invoke → execute an action inside a tween flow
```csharp
Tween.Sequence(explosive.TweenScale(Vector3.zero, 0.3F)).Append(Tween.Invoke(() => { Instantiate(explosionPrefab, explosive.position, Quaternion.identity); Destroy(explosive.gameObject); })).Play();
```
### Easing & Animation
Use from built-in presets, expressive curve types, or provide your own AnimationCurve.
```csharp
// built-in easing Types
Linear, Sine, Cubic, Quint, Circ, Elastic, Quad, Quart, Expo, Back, Bounce

// curve presets
Anticipate, Pop, Punch, Shake, Snappy, Spring

// custom curves
tween.SetEase(customAnimationCurve);
```
Each easing type also supports directional variants:
+ **In** → starts slowly, speeds up.
+ **Out** → starts quickly, slows down.
+ **InOut** → combines both (ease-in then ease-out).

## Lifecycle Control
### Individual Tween
```csharp
var tween = transform.TweenScale(Vector3.one * 2F, 1F);

// control execution
tween.Play();          // start the tween
tween.Pause();         // pause it in place
tween.Resume();        // continue from where it paused
tween.Kill();          // stop immediately and mark as None
tween.TerminateLoop(); // if looping, stop after the current cycle
```

### Global
Use the `Factory` to affect all active tweens across the game.
```csharp
Factory.Pause();   // pause every tween
Factory.Resume();  // resume all paused tweens
Factory.Kill();    // kill all tweens instantly

// group tweens with a tag and manage them collectively
// this is useful for pausing UI animations during gameplay, or killing all enemy tweens on death

// play with a tag
transform.TweenMove(Vector3.one * 5F, 2F).PlayWithTag("UI");

// later, control by tag
Factory.Pause("UI");
Factory.Resume("Player");
Factory.Kill("Enemy");
```

### Configuration
```csharp
// increase pool size for many concurrent tweens
Factory.MaxTweens = 512;  // default: 64

// monitor usage
if (Factory.ActiveTweens > Factory.MaxTweens * 0.8F)
{
    Debug.LogWarning("Tween pool nearly exhausted!");
}
```

## Troubleshooting
### Nothing happens
- Ensure you're calling `.Play()` or `.PlayWithTag()`.
- Check if the linked object is active.
- Verify `Factory.MaxTweens` hasn't been exceeded.

### Unexpected behavior
- Tweens capture initial values when `.Play()` is called, not when created.
- Avoid starting multiple tweens on the same property at the same time.
- Use tags to group and control tweens (pause old ones before starting new ones).

### Logging & Debugging
The tween system includes a lightweight logging utility.
```csharp
Log.Info("Message");
Log.Warning("Warning");
Log.Exception(exception);
```
Logs are enabled by default. To disable all tween logs globally use `Log.Enabled = false;`.

## License
MIT License - see [LICENSE](LICENSE) for details.
