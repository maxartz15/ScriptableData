# ScriptableData
## Data
ScriptableData stores runtime data to be accessed by scripts that reference the ScriptableObject. It has a OnValueChangedEvent to subscribe when data changes.
With this workflow you can remove dependencies and increase flexibility.
### Extendable Data:
```C#
public class ScriptableData<T0> : ScriptableObject {}
public class ScriptableData<T0, T1> : ScriptableObject {}
public class ScriptableData<T0, T1, T2> : ScriptableObject {}
public class ScriptableData<T0, T1, T2, T3> : ScriptableObject {}
```
### Example:
```C#
[CreateAssetMenu(menuName = "ScriptableData/Data/Vector3", order = 147)]
public class SDVector3 : ScriptableData<Vector3> {}
```
## Events
ScriptableEvent does not contain any runtime data but can be used to send events (with data) around.
### Base Event
```C#
[CreateAssetMenu(menuName = "ScriptableData/Event/Event", order = 147)]
public class ScriptableEvent : ScriptableObject {}
```
### Extendable Events:
```C#
public class ScriptableEvent<T0> : ScriptableObject {}
public class ScriptableEvent<T0, T1> : ScriptableObject {}
public class ScriptableEvent<T0, T1, T2> : ScriptableObject {}
public class ScriptableEvent<T0, T1, T2, T3> : ScriptableObject {}
```
### Example:
```C#
[CreateAssetMenu(menuName = "ScriptableData/Event/Vector3", order = 147)]
public class SEVector3 : ScriptableEvent<Vector3> {}
```
## ExtendedScriptableObjectDrawer
If you don't want to use the extended drawer, use the `[NonExpandable]` attribute.
### Example:
```C#
[NonExpandable]
public SEVector3 myVector3;
```

## Install
[Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

[Unitypackage](https://github.com/maxartz15/ScriptableData/releases)

## LICENSE
Overall package is licensed under [MIT](/LICENSE.md), unless otherwise noted in the [3rd party licenses](/THIRD%20PARTY%20NOTICES.md) file and/or source code.