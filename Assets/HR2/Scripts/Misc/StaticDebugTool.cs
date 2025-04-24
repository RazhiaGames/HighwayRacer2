using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticDebugTool
{
    private class DebugEntry
    {
        public string Label;
        public Func<object> Getter;
        public float Interval;
        public Coroutine coroutine;
    }

    private static readonly Dictionary<string, DebugEntry> watchedValues = new Dictionary<string, DebugEntry>();

    public static void Watch(string label, Func<object> getter, float interval)
    {
        if (watchedValues.ContainsKey(label))
        {
            Debug.LogWarning($"Label {label} is already being watched. Overwriting.");
            Unwatch(label);
        }

        var entry = new DebugEntry
        {
            Label = label,
            Getter = getter,
            Interval = interval
        };

        Coroutine coroutine = StaticDebugRunner.Instance.StartCoroutine(DebugCoroutine(entry));
        entry.coroutine = coroutine;

        watchedValues[label] = entry;

    }

    public static void Unwatch(string label)
    {
        if(watchedValues.TryGetValue(label, out var entry))
        {
            StaticDebugRunner.Instance.StopCoroutine(entry.coroutine);
            watchedValues.Remove(label);
        }
    }

    private static IEnumerator DebugCoroutine(DebugEntry entry)
    {
        while (true)
        {
            Debug.Log($"[{entry.Label}] {entry.Getter()}");
            yield return new WaitForSeconds(entry.Interval);
        }
    }
}
