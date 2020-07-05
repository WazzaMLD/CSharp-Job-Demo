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

The Add Component menu is extended with GameObject components under
"Lokel Utils" or "Lokel" group.

### Sinusoid Scene

Demonstrates the ***Sinusoidal Tiles Controller*** and classes
in the `Lokel.Sinusoidal` namespace for making a prefab
be instantiated in a grid of the size configured in the Inspector
and wiggle in a 3D sinusoidal pattern.

### Shockwave Scene

Demonstrates the ***Shockwave Controller*** in the `Lokel.Shockwave` namespace
for making tiles that can be disrupted by a randomly placed shockwave.

# Using in Your Project

The easiest way to cleanly use this package is via the Unity Package Manager... [follow this link for instructions](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

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
