using System;

namespace Csn.Retail.Editorial.Web.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoBindAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoBindSelfAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoBindSelfAsSingletonAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoBindAsPerRequestAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class AutoBindAsSingletonAttribute : Attribute
    {
    }
}
