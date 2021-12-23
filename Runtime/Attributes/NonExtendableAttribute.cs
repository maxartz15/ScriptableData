using System;
using UnityEngine;

namespace ScriptableData
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class NonExtendableAttribute : PropertyAttribute { }
}