using System.Collections.Generic;
using System;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

[TestFixture]
public class MgrJsonTest
{
    private MgrJson mgr_json;
    private string test_file_path;
    private string test_folder_path;
    private string invalid_file_path;

    private class TestObject
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public TestObject()
        {
            Name = "Test";
            Value = 123;
        }
    }

    [SetUp]
    public void Setup()
    {
        mgr_json = new MgrJson();
        test_file_path = Application.persistentDataPath + "/test.json";
        test_folder_path = Application.persistentDataPath + "/TestFolder/";
        invalid_file_path = Application.persistentDataPath + "\\.invalid_path/test.json";

        if (!Directory.Exists(test_folder_path))
        {
            Directory.CreateDirectory(test_folder_path);
        }
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(test_file_path))
        {
            File.Delete(test_file_path);
        }

        if (Directory.Exists(test_folder_path))
        {
            Directory.Delete(test_folder_path, true);
        }

        if( Directory.Exists(invalid_file_path))
        {
            Directory.Delete(invalid_file_path, true);
        }
    }

    [Test]
    public void WriteFile_ShouldWriteJsonToFile()
    {
        TestObject test_object = new TestObject();
        mgr_json.WriteFile<TestObject>(test_object, "test.json");
        mgr_json.WriteFile<TestObject>(test_object, "test.json");

        Assert.IsTrue(File.Exists(test_file_path));
    }

    [Test]
    public void ReadFile_ShouldReadJsonFromFile()
    {
        TestObject test_object = new TestObject();

        mgr_json.WriteFile<TestObject>(test_object, "test.json");

        TestObject result = mgr_json.ReadFile<TestObject>("test.json");

        Assert.AreEqual(test_object.Name, result.Name);
        Assert.AreEqual(test_object.Value, result.Value);
    }

    [Test]
    public void DeleteFile_ShouldDeleteFile()
    {
        TestObject test_object = new TestObject();
        mgr_json.WriteFile<TestObject>(test_object, "test.json");

        mgr_json.DeleteFile("test.json");

        Assert.IsFalse(File.Exists(test_file_path));
    }

    [Test]
    public void ReadFolder_ShouldReturnAllFilePaths()
    {
        TestObject test_object = new TestObject();
        TestObject test_object2 = new TestObject();
        test_object2.Value = 456;
        mgr_json.WriteFile<TestObject>(test_object, "test1.json", "TestFolder/");
        mgr_json.WriteFile<TestObject>(test_object2, "test2.json", "TestFolder/");

        List<string> files = mgr_json.ReadFolder("TestFolder/");
        List<TestObject> objs = new List<TestObject>();
        foreach (string file in files)
        {
            objs.Add(mgr_json.ReadFile<TestObject>(file));
        }

        Assert.AreEqual(2, objs.Count);
        Assert.IsTrue(objs[0].Value == 123);
        Assert.IsTrue(objs[1].Value == 456);
    }

    [Test]
    public void WriteFile_Exception()
    {
        TestObject test_object = new TestObject();
        mgr_json.WriteFile<TestObject>(test_object, "test.json", "\\.invalid_path/");
        Assert.IsFalse(File.Exists(invalid_file_path));
    }

    [Test]
    public void ReadFile_Exception()
    {
        TestObject result = mgr_json.ReadFile<TestObject>("test.json", "\\.invalid_path/");
        Assert.IsTrue(result == default(TestObject));
    }

    [Test]
    public void DeleteFile_Exception()
    {
        mgr_json.DeleteFile("test.json", "\\.invalid_path/");
        Assert.IsFalse(File.Exists(invalid_file_path));
    }

    [Test]
    public void ReadFolder_Exception()
    {
        List<string> files = mgr_json.ReadFolder("\\.invalid_path/");
        Assert.IsTrue(files.Count == 0);
    }

    [Test]
    public void JsonToObject_Exception()
    {
        TestObject result = mgr_json.JsonToObject<TestObject>("invalid json");
        Assert.IsTrue(result == default(TestObject));
    }

    [Test]
    public void ObjectToJson_Exception()
    {
        string result = mgr_json.ObjectToJson<TestObject>(null);
        Assert.IsTrue(true);
    }

}