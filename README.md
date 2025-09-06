# Version: 1.0.2
A high-performance, composable tweening API for Unity that emphasizes clean architecture and developer experience.

## Features
- Minimal per-frame overhead with optimized update loops.
- Generic Value<T> system supports any struct type.
- Built-in support for Unity primitives (Vector3, Color, Quaternion, etc.).
- Build complex animations with Sequence and Parallel.
- Extensive extension methods for common Unity components.
- IntelliSense-friendly with comprehensive XML documentation.

## Installation
Clone or download this repository into your Unity project. No external dependencies required, works with Unity 2021.3 LTS and newer.

## Usage
> [!IMPORTANT]
> All tweens are **lazy**, they won’t start until you call `.Play()` or `Factory.Play(IElement)`.
```csharp
using Emp37.Tweening;

// run a tween without chaining Play()
IElement tween = rectTransform.TweenMoveY(200F, 1F);
Factory.Play(tween);
```
> [!Note]
> Everything in this API is an `IElement`- a composable unity that can be played, paused or killed.
### Extensions
```csharp
// fade a UI element
canvasGroup.TweenFade(target: 0F, duration: 1.5F).Play();

// move transform with custom easing
transform.TweenMove(Vector3.zero, 3F).SetEase(Ease.Type.OutBounce).OnComplete(() => print("Finished")).Play();
```
### Value\<T>
```csharp
// built-in types work automatically
Tween.Value(init: () => Time.timeScale, target: 2F, duration: 5F, update: value => Time.timeScale = value).Play();

// custom types need an evaluator function
Tween.Value<MyStruct>(() => current, targetStruct, 2F, value => currentStruct = value, evaluator: (a, b, t) => MyStruct.Lerp(a, b, t)).Play();
```
### Sequence
A sequence allows playing tweens sequentially. There are three different syntax to build them:
- Params Syntax
```csharp
// alarm sequence: camera shakes, lights turn red, audio fades out
Tween.Sequence(mainCamera.TweenRotate(new Vector3(0F, 15F, 0F), 0.3F).SetEase(Ease.Type.InOutSine), sirenLight.TweenColor(Color.red, 0.5F), alarmSource.TweenVolume(0F, 1F)).Play();
```
- Append Syntax
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
Parallels run multiple tweens at the same time under one `IElement`.
```csharp
var intensityTween = Tween.Value(() => spotlight.intensity, 5F, 1F, value => spotlight.intensity = value).SetDelay(0.2F);

// boss entrance: camera zooms in, music fades down, spotlight brightens simultaneously
Tween.Parallel(mainCamera.TweenFOV(30F, 1.5F), bossTheme.TweenVolume(0.3F, 1.5F), intensityTween).Play();
```
### Delay
```csharp
// timed delay: wait 1.5 seconds before fading UI out
Tween.Sequence(notificationPanel.TweenFade(1F, 0.5F), Tween.Delay(1.5F), notificationPanel.TweenFade(0F, 0.5F)).Play();

// conditional delay: wait until player presses any key before contuining the sequence
Tween.Sequence()
    .Append(introText.TweenAlpha(1F, 0.8F))
    .Append(Tween.Delay(() => Input.anyKeyDown))
    .Append(Tween.Parallel(introText.TweenAlpha(0F, 0.5F), mainMenu.TweenFade(1F, 1F)))
    .Play();
```
### Invoke
```csharp
// fade out music then trigger a level load
musicSource.TweenVolume(0F, 1.5F).Then(Tween.Invoke(() => SceneManager.LoadScene("NextLevel"))).Play();
```
---
### Looping
```csharp
// infinite ping-pong with delay between cycles
tween.SetLoop(new Loop(type: Loop.Type.Yoyo, count: -1, interval: 0.5F));

// repeat 3 times
tween.SetLoop(new Loop(Loop.Type.Repeat, count: 3));

// one-shot return to start (common for UI effects)
tween.SetReturnOnce();
```
### Lifecycle Events and Control
```csharp
var tween = transform.TweenMove(target, 2f).OnStart(() => Debug.Log("Moving!")).OnUpdate(progress => UpdateProgressBar(progress)).OnComplete(() => TriggerParticles()).Play();

tween.Pause();
tween.Resume();
tween.Kill();           // immediate stop
tween.TerminateLoop();  // stop looping, finish current cycle
```
### Memory Management & Linking
```csharp
// tween automatically dies if link is destroyed
var tween = Tween.Value(() => enemy.health, target: 0F, duration: 1F, update: v => enemy.health = v, link: enemy.gameObject);

// Capacity management
Factory.MaxTweens = 256;  // increase pool size if needed
```
## Performance Characteristics
- **Memory**: Zero allocations during playback (after initial setup).
- **CPU**: ~0.02ms for 100 active tweens on mid-range mobile hardware.
- **Capacity**: Default pool supports 64 concurrent tweens, easily configurable.
- **Startup**: <1ms initialization cost via RuntimeInitializeOnLoadMethod.
## API Design Philosophy
- **Lazy Execution**: Tweens are configured but don't consume resources until `.Play()` is called.
- **Composition**: Complex animations are built by combining simple elements rather than creating monolithic tween classes.
- **Fail-Safe**: Invalid parameters return empty tweens rather than throwing exceptions, preventing runtime crashes.
- **Fluency**: Method chaining enables expressive, readable animation code.

## License
MIT License - see [LICENSE](LICENSE) for details.
