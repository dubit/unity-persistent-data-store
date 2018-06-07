using System;
using NUnit.Framework;
using UnityEngine;

namespace DUCK.PersistentDataStore.Tests
{
	public class PersistentDataStoreTests
	{
		private const string EXAMPLE_UID_1 = "abc123";
		private const string EXAMPLE_UID_2 = "456xyz";

		[Serializable]
		public class DummyObject
		{
			[SerializeField]
			private string name;

			[SerializeField]
			private int value;

			public DummyObject(string name, int value)
			{
				this.name = name;
				this.value = value;
			}

			public bool DataEquals(DummyObject other)
			{
				return name.Equals(other.name) && value.Equals(other.value);
			}
		}

		[Test]
		public void ExpectNonExistentObjectsToNotExist()
		{
			Assert.IsFalse(SerializedData.Exists<DummyObject>());
			Assert.IsFalse(SerializedData.Exists<DummyObject>(EXAMPLE_UID_1));
		}

		[Test]
		public void ExpectLoadingNonExistentObjectsToFail()
		{
			var loadedGenericObject = SerializedData.Load<DummyObject>();
			Assert.IsNull(loadedGenericObject);

			var loadedSpecificObject = SerializedData.Load<DummyObject>(EXAMPLE_UID_1);
			Assert.IsNull(loadedSpecificObject);
		}

		[Test]
		public void ExpectDeletingNonExistentObjectsToReturnFalse()
		{
			var didDeleteGenericObject = SerializedData.Delete<DummyObject>();
			Assert.IsFalse(didDeleteGenericObject);

			var didDeleteSpecificObject = SerializedData.Delete<DummyObject>(EXAMPLE_UID_1);
			Assert.IsFalse(didDeleteSpecificObject);
		}

		[Test]
		public void ExpectSavingNullObjectToFail()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				DummyObject dummyObject = null;

				// ReSharper disable once ExpressionIsAlwaysNull
				SerializedData.Save(dummyObject);
			});
		}

		[Test]
		public void ExpectSavingAndLoadingGenericObjectToSucceed()
		{
			ExpectSavingAndLoadingObjectToRetainCorrectData();

			Assert.IsFalse(SerializedData.Exists<DummyObject>(EXAMPLE_UID_1));
			Assert.IsFalse(SerializedData.Exists<DummyObject>(EXAMPLE_UID_2));
		}

		[Test]
		public void ExpectSavingAndLoadingSpecificObjectToSucceed()
		{
			ExpectSavingAndLoadingObjectToRetainCorrectData(EXAMPLE_UID_1);

			Assert.IsFalse(SerializedData.Exists<DummyObject>());
			Assert.IsFalse(SerializedData.Exists<DummyObject>(EXAMPLE_UID_2));
		}

		[TearDown]
		public void DeleteAll()
		{
			SerializedData.Delete<DummyObject>();
			SerializedData.Delete<DummyObject>(EXAMPLE_UID_1);
			SerializedData.Delete<DummyObject>(EXAMPLE_UID_2);
		}

		private static void ExpectSavingAndLoadingObjectToRetainCorrectData(string uid = "")
		{
			var dummyObject = new DummyObject("david", 31);

			Assert.DoesNotThrow(() => SerializedData.Save(dummyObject, uid));

			Assert.IsTrue(SerializedData.Exists<DummyObject>(uid));

			var retrievedObject = SerializedData.Load<DummyObject>(uid);
			Assert.IsNotNull(retrievedObject);
			Assert.IsTrue(retrievedObject.DataEquals(dummyObject));
		}
	}
}