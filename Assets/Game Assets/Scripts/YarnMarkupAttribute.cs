using System;

/// <summary>
/// Marks method to be used as a MarkupAttribute.
/// <remarks>Must be implemented with signature : bool, IReadonlyDictionary(string, MarkupValue)</remarks>
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class YarnMarkupAttribute : Attribute
{
    public readonly string AttrName;

    public YarnMarkupAttribute(string attrName)
    {
        AttrName = attrName;
    }
}