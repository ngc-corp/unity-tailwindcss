using UnityEditor;

namespace NGCCorp.TailwindCSS
{
  public static class PersistentData
  {
    public static void SavePersistentConfig<T>(string key, T value)
    {
      if (typeof(T) == typeof(string))
      {
        EditorPrefs.SetString(key, value as string);
      }
      else if (typeof(T) == typeof(bool))
      {
        EditorPrefs.SetBool(key, (bool)(object)value);
      }
      else if (typeof(T) == typeof(int))
      {
        EditorPrefs.SetInt(key, (int)(object)value);
      }
      else
      {
        throw new System.NotSupportedException($"Type {typeof(T)} is not supported for persistent config.");
      }
    }

    public static T LoadPersistentConfig<T>(string key)
    {
      if (EditorPrefs.HasKey(key))
      {
        if (typeof(T) == typeof(string))
        {
          return (T)(object)EditorPrefs.GetString(key);
        }
        else if (typeof(T) == typeof(bool))
        {
          return (T)(object)EditorPrefs.GetBool(key);
        }
        else if (typeof(T) == typeof(int))
        {
          return (T)(object)EditorPrefs.GetInt(key);
        }
        else
        {
          throw new System.NotSupportedException($"Type {typeof(T)} is not supported for persistent config.");
        }
      }

      return default;
    }
  }
}
