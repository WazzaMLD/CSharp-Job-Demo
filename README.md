# Lokel Digital Job System Demo

***Demo Code and Scenes for Unity's C# Job System***

(c) Copyright 2020 Lokel Digital Pty Ltd.
https://www.lokeldigital.com

## Purpose

To make use of IJob, IJobParallelFor and IJobParallelForTransform
job types to perform some compute workload in background threads.
In the process of building this out, the restrictions and limitations
of the C# Job System and the various collections where discovered.

## Features

The Scenes folder has multiple scenes:
- Sinusoid
- Shockwave
- Shockwave-Physics

The Add Component menu is extended with GameObject components under
"Lokel Utils" or "Lokel" group.

### Sinusoid Scene

Demonstrates the ***Sinusoidal Tiles Controller*** and classes
in the `Lokel.Sinusoidal` namespace for making a prefab
be instantiated in a grid of the size configured in the Inspector
and wiggle in a 3D sinusoidal pattern.

### Shockwave Scene

Demonstrates the ***Shockwave Physics*** in the `Lokel.Shockwave` namespace
working with a Random Shockwave Trigger. Click the Trigger Shockwave button
on the bottom-right of the screen to see a shockwave triggered somewhere on the
generated floor.

### Shockwave-Physics Scene

Demonstrates the ***Shockwave Physics*** in the `Lokel.Shockwave` namespace
using a collision to trigger a shockwave via the Collision Shockwave Trigger
component. This is added to the floor tile prefab and tells the Shockwave Physics
component to insert a shockwave at a given position on the floor.

This scene includes a rudimentary cannon and sounds just to bring it together.

## Option 1 - Via Package Manager

The easiest way to cleanly use this package is via the Unity Package Manager...
However, this needs to be done in two steps because git packages listed as a dependency
will NOT be installed by the package manager, so you need to do it in two steps:

| Step                                  | URL to use with Unity Package Manager                       |
|---------------------------------------|-------------------------------------------------------------|
| Install the Lokel Collections package | https://github.com/WazzaMLD/Lokel-JobSystem-Collections.git |
| Install this package                  | https://github.com/WazzaMLD/CSharp-Job-Demo.git             |

1. Open the Package Manager tab from the menu `Window | Package Manager`
2. In the top-left you will see a plus sign (`+`) with
a little pull-down menu. Click this to choose the package install
method; select `add package from Git URL...` 
3. Copy/Paste the URL from above into the empty text field box and click the `Add` button on the right of it.
4. The package should import and the Unity asset import dialog box should appear briefly.

If the steps above don't make sense, take a look at this with
screenshots and step-by-step instructions
[Installing from a Git URL - Unity Manual](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

## Options 2 - Download the Code Into Your Project

Alternatively, you may want to download a ZIP from the GitHub
repository and unpack that into your Unity Project. Just make sure you have it in a subfolder of your ***Assets*** folder to avoid
collisions with your own work.

# Contributions

Contributions are welcome. Please add yourself to the list.

By forking this repo and extending it, you are creating
a derivative work and that should have the same Creative Commons
By Attribution license which keeps the contributions compatible
with the original repo. From this fork, raise a pull-request.

## Contributors

- Warwick Molloy - [Lokel Digital Pty Ltd](https://www.lokeldigital.com)

# Final Note on UPM and Acknowledgement

After racking my brain on why defining the dependency of this package on the
Lokel JobSystem Collections package in GitHub would fail (try but fail) via
the Unity Package Manager (UPM) ***unless it was already installed*** I found this
project that extends UPM to provide the missing support.... FACEPALM. Unity's own
documentation seems to promise that it should work but it doesn't so kudos to the
author of this repo for doing something about it.

https://github.com/mob-sakai/GitDependencyResolverForUnity
