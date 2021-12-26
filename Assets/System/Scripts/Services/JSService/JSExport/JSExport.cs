using System;

[AttributeUsage(AttributeTargets.All, Inherited = true)]
public sealed class JSExportAttribute : Attribute
{}
[AttributeUsage(AttributeTargets.All, Inherited = true)]
public sealed class JSNotExportAttribute : Attribute
{}