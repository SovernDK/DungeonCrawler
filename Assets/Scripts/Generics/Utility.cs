using System.Text.RegularExpressions;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Retrieves the base name of a Unity object by removing the numeric suffix added when duplicating objects.
    /// </summary>
    /// <param name="obj">The Unity object whose name is to be processed.</param>
    /// <returns>The base name of the object without any numeric suffix.</returns>
    public static string GetObjectNameWithoutSuffix(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("ObjectNameUtility.GetObjectNameWithoutSuffix: The GameObject provided is null.");
            return string.Empty;
        }

        // Regex pattern to find and remove numeric suffix in the format " (number)"
        string pattern = @"\s\(\d+\)$";
        return Regex.Replace(obj.name, pattern, "");
    }
}