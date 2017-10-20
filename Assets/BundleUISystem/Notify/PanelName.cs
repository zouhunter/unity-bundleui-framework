using UnityEngine;
using System.Collections;
using System.Reflection;

public partial class PanelName
{
    static PanelName()
    {
        var fields = typeof(PanelName).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
        foreach (var item in fields)
        {
            item.SetValue(null, item.Name, null);
        }
    }

}