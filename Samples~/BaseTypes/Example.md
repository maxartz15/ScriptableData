Types can be anything: classes, structs etc.
Replace {TYPE} with your type.

Data:
```C#
using UnityEngine;

namespace ScriptableData
{
	[CreateAssetMenu(menuName = "ScriptableData/Data/{TYPE}", order = 146)]
	public class SD{TYPE} : ScriptableData<{TYPE}> { }
}
```

Events:
```C#
using UnityEngine;

namespace ScriptableData
{
	[CreateAssetMenu(menuName = "ScriptableData/Event/{TYPE}", order = 147)]
	public class SE{TYPE} : ScriptableEvent<{TYPE}> {}
}
```