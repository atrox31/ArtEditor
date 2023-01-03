using System;
using System.Reflection;
using System.Windows.Forms;

namespace ArtCore_Editor.Functions
{
    public static class ControlExtensions
    {
        public static void InvokeMethod(this Control sender, string action)
        {
            Type senderType = sender.GetType();
            MethodInfo theMethod = senderType.GetMethod(action);
            theMethod?.Invoke(senderType, null);
        }
    }
}
