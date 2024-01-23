using System;
using System.Collections.Generic;

public static class ListPool<T>
{
    private static readonly Stack<List<T>> mListStack = new(8);

    /// <returns></returns>
    public static List<T> Get()
    {
        if (mListStack.Count == 0) return new List<T>(8);

        return mListStack.Pop();
    }

    public static void Release(List<T> toRelease)
    {
        if (mListStack.Contains(toRelease))
            throw new InvalidOperationException("The List is released even though it is in the pool");

        toRelease.Clear();
        mListStack.Push(toRelease);
    }
}

public static class ListPoolExtensions
{
    public static void Release2Pool<T>(this List<T> self)
    {
        ListPool<T>.Release(self);
    }
}