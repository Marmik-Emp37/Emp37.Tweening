Tweening API for Unity.

## Features
- **Extensions** - animate common Unity types (`Transform`, `CanvasGroup`, `Image`, etc.).
- **Composable flow** - build complex timelines with `Sequence` and `Parallel`.
- **Lazy execution** - tweens don’t run until you call `.Play()`.
- **Delays** - fixed timers or conditional waits (e.g. `Input.anyKeyDown`).
- **Looping** - repeat or yoyo with optional intervals.
- **Control** - pause, resume, or kill tweens at any time.
- **Lightweight** - zero dependencies, works out-of-the-box.

## Installation
Clone or download this repository into your Unity project. No external dependencies, works out-of-the-box with Unity 2021+.

## Usage
> [!IMPORTANT]
> All tweens are **lazy**, they won’t start until you call `.Play()` or `Factory.Play(IElement)`.
```csharp
// run a tween without chaining Play()
IElement tween = rectTransform.TweenMoveY(200F, 1F);
Factory.Play(tween);
```
### Extensions
```csharp
// fade out a CanvasGroup to 0 alpha over 1.5 seconds
canvasGroup.TweenFade(target: 0F, duration: 1.5F).Play();

// move transform to Vector3.zero over 3s, apply InOutExpo easing, automatically return to the start value, trigger a callback when finished, then finally start the tween
transform.TweenMove(Vector3.zero, 3F).SetEase(Ease.Type.InOutExpo).SetReturnOnce().OnComplete(() => print("Finished")).Play();
```
### Value
```csharp
// tweening a built-in property
// init() provides the starting value, target defines the end value and update() applies the interpolated value each frame
Tween.Value(init: () => Time.timeScale, target: 2F, duration: 5F, update: value => Time.timeScale = value).Play();

// tweening a custom struct
public struct Foo
{
    public float A, B;
    public Foo(float a, float b) { A = a; B = b; }
}
// you must provide an evaluator (a lerp function) since Unity can't interpolate Foo directly
Tween.Value<Foo>(() => current, new Foo(a: 10F, b: 5F), 2F, value => current = value, evaluator: (a, b, t) => new Foo(Mathf.Lerp(a.A, b.A, t), Mathf.Lerp(a.B, b.B, t))).Play();
```
### Sequence
Sequences let you chain tweens together. Three different ways to build them:
- Params Syntax:
```csharp
// alarm sequence: camera shakes, lights turn red, audio fades out
Tween.Sequence(mainCamera.TweenRotate(new Vector3(0F, 15F, 0F), 0.3F).SetEase(Ease.Type.InOutSine), sirenLight.TweenColor(Color.red, 0.5F), alarmSource.TweenVolume(0F, 1F)).Play();
```
- Append Syntax:
```csharp
// victory screen: fade in background, bounce trophy icon, then fill progress bar, fade in victory text
Tween.Sequence()
    .Append(victoryBackground.TweenFade(1F, 0.5F))
    .Append(trophyIcon.TweenScale(Vector3.one * 1.2F, 0.7F).SetEase(Ease.Type.OutBack))
    .Append(progressBar.TweenFill(1F, 2F))
    .Append(victoryText.TweenAlpha(1F, 0.5F))
    .Play();
```
- Then Syntax
```csharp
// player damage feedback: flash red, knock back, restore
playerSprite.TweenColor(Color.red, 0.2F).Then(playerTransform.TweenMoveX(playerTransform.position.x - 1.5F, 0.3F).SetEase(Ease.Type.OutQuad)).Then(playerSprite.TweenColor(originalColor, 0.2F)).Play();
```
### Parallel
Run multiple tweens at the same time.
```csharp
// boss entrance: camera zooms in, music fades down, spotlight brightens simultaneously
var intensityTween = Tween.Value(() => spotlight.intensity, 5F, 1F, value => spotlight.intensity = value).SetDelay(0.2F);

Tween.Parallel(mainCamera.TweenFOV(30F, 1.5F), bossTheme.TweenVolume(0.3F, 1.5F), intensityTween).Play();
```
### Delay
```csharp
// timed delay: wait 1.5 seconds before fading UI out
Tween.Sequence(notificationPanel.TweenFade(1F, 0.5F), Tween.Delay(1.5F), notificationPanel.TweenFade(0F, 0.5F)).Play();

// conditional delay: wait until player presses any key before continuing
Tween.Sequence()
    .Append(introText.TweenAlpha(1F, 0.8F))
    .Append(Tween.Delay(() => Input.anyKeyDown))
    .Append(Tween.Parallel(introText.TweenAlpha(0F, 0.5F), mainMenu.TweenFade(1F, 1F)))
    .Play();
```
### Looping
Tweens can repeat indefinitely or for a set number of cycles.
```csharp
// endless pulsing button with yoyo loop
playButton.TweenScale(Vector3.one * 1.1F, 0.5F)
    .SetEase(Ease.Curves.Pop)
    .SetLoop(new Loop(
        type: Loop.Type.Yoyo,  // ping-pong back and forth
        count: -1,             // -1 = infinite
        interval: 0.25F        // wait 0.25s between cycles
    )).Play();
```
### Invoke
Schedule an action to run inside a tween flow.
```csharp
// fade out music then trigger a level load
musicSource.TweenVolume(0F, 1.5F).Then(Tween.Invoke(() => SceneManager.LoadScene("NextLevel"))).Play();
```
### Control
```csharp
IElement tween = rectTransform.TweenSize(Vector2.one * 5F, 2F).Play();
tween.Pause();  // stop temporarily
tween.Resume(); // continue
tween.Kill();   // stop and dispose
```
## Lifecycle
- Tweens update during Unity's `Update` loop by default.
-  You can switch to `Delta.Unscaled` for timeScale-independent animations.
- Tweens can optionally be bound to their target object via the `link` parameter. Bound tweens are automatically killed when the target is destroyed.
- Tweens do not persist across scene loads unless you manage them manually.
