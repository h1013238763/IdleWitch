using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class UTMgrBase
{
   // A simple class to test the MgrBase functionality
    private class TestClass
    {
        public int Value { get; set; }
    }

    [UnityTest]
    public IEnumerator SingletonInstanceIsCreated()
    {
        var instance = MgrBase<TestClass>.Mgr();
        Assert.IsNotNull(instance);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SingletonInstanceIsSameAcrossMultipleCalls()
    {
        var instance1 = MgrBase<TestClass>.Mgr();
        var instance2 = MgrBase<TestClass>.Mgr();
        Assert.AreSame(instance1, instance2);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SingletonInstanceRetainsProperties()
    {
        var instance = MgrBase<TestClass>.Mgr();
        instance.Value = 42;
        var sameInstance = MgrBase<TestClass>.Mgr();
        Assert.AreEqual(42, sameInstance.Value);
        yield return null;
    }

}