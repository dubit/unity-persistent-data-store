using System;
using System.IO;
using UnityEngine;

namespace DUCK.PersistentDataStore
{
	/// <summary>
	/// A persistent data store for serializable objects, identified by their type, or optionally by a unique ID.
	/// </summary>
	public static class SerializedData
	{
		/// <summary>
		/// The data path used to save persistent objects.
		/// </summary>
		private static readonly string saveDataPath = Application.persistentDataPath + "/PersistentData/";

		/// <summary>
		/// Check if an object of type T exists in the persistent store, optionally with a unique ID.
		/// <param name="uid">The unique ID (optional) - will check for the default object name if not specified.</param>
		/// <returns>bool - whether or not the specified object exists.</returns>
		/// </summary>
		public static bool Exists<T>(string uid = "")
		{
			return File.Exists(GetFilePath<T>(uid));
		}

		/// <summary>
		/// Save an object of type T in the store, optionally identified by a unique ID.
		/// <param name="dataObject">The object to save (or update) in the data store.</param>
		/// <param name="uid">The unique ID (optional) - will write to the default object name if not specified.</param>
		/// </summary>
		public static void Save<T>(T dataObject, string uid = "")
		{
			if (dataObject == null) throw new ArgumentException("Cannot save a null object.");

			File.WriteAllText(GetFilePath<T>(uid), JsonUtility.ToJson(dataObject));
		}

		/// <summary>
		/// Load an object of type T from the store, optionally identified by a unique ID.
		/// <param name="uid">The unique ID (optional) - will load the default object name if not specified.</param>
		/// <returns>T - the object which was retrieved.</returns>
		/// </summary>
		public static T Load<T>(string uid = "")
		{
			var path = GetFilePath<T>(uid);

			try
			{
				return JsonUtility.FromJson<T>(File.ReadAllText(path));
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// Deletes a stored object from the persistent store, optionally identified by a unique ID.
		/// <param name="uid">The unique ID (optional) - will attempt to delete the default object name if not specified.</param>
		/// <returns>bool - whether or not a matching object was deleted.</returns>
		/// </summary>
		public static bool Delete<T>(string uid = "")
		{
			var path = GetFilePath<T>(uid);

			if (!File.Exists(path)) return false;

			File.Delete(path);
			return true;

		}

		// Files identified by a unique ID will have this string appended to their file path. This function determines how.
		private static string GetFilePath<T>(string uid)
		{
			if (!Directory.Exists(saveDataPath))
			{
				Directory.CreateDirectory(saveDataPath);
			}

			var name = typeof(T).ToString()
				+ ((!string.IsNullOrEmpty(uid))
				? "-" + uid
				: "");

			return saveDataPath + name + ".json";
		}
	}
}
