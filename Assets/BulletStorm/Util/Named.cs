using System;
using System.Collections.Generic;

namespace BulletStorm.Util
{
	/// <summary>
	/// An object which is named should be able to be found by its name.
	/// This class provides a dictionary to store named objects.
	/// </summary>
	public abstract class Named<T> where T : Named<T>
	{
		/// <summary>
		/// Name of the instance.
		/// </summary>
		public string Name { get => name; set => SetName(value); }

		/// <summary>
		/// Remove from <see cref="Instances"/> when destroy.
		/// </summary>
		~Named()
		{
			SetName(null);
		}

		/// <summary>
		/// Find an instance by name.
		/// </summary>
		/// <param name="name">The instance name.</param>
		/// <returns>If not found, return default.</returns>
		/// <exception cref="ArgumentNullException">Parameter 'name' is null</exception>
		public static T Find(string name)
		{
			if (name == null) { throw new ArgumentNullException(nameof(name)); }

			if (Instances.TryGetValue(name, out Named<T> instance)) { return instance as T; }
			else { return default; }
		}

		/// <summary>
		/// Set the instance name.
		/// </summary>
		/// <param name="name">New name.</param>
		/// <exception cref="ArgumentException">Name already exist</exception>
		public void SetName(string name)
		{
			if (name != null)
			{
				try { Instances.Add(name, this); }
				catch (ArgumentException e) { throw e; }
			}
			if (this.name != null) { Instances.Remove(this.name); }
			this.name = name;
		}

		/// <summary>
		/// Try to set the instance name.
		/// </summary>
		/// <param name="name">Neww name.</param>
		/// <returns>Setted or not.</returns>
		public bool TrySetName(string name)
		{
			try { SetName(name); }
			catch (ArgumentException) { return false; }
			return true;
		}

		private static readonly Dictionary<string, Named<T>> Instances = new Dictionary<string, Named<T>>();
		private string name;
	}
}
