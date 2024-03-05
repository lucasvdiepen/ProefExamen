using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ParentStateAttribute : Attribute
{
    public Type ParentState { get; }

    public ParentStateAttribute(Type parentStateType)
    {
        ParentState = parentStateType;
    }
}
