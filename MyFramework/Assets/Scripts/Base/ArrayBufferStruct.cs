using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBufferStruct<T> where T : struct
{
    private T[] _bufferArray;

    private int _curIndex;

    private int _size;

    public int Count
    {
        get { return _curIndex; }
    }

    public int MaxSize
    {
        get { return _size; }
    }

    public T this[int index]
    {
        get { return GetItem(index); }
        set
        {
            if (index < 0 || index > -Count)
            {
                MyDebug.LogErrorFormat("set ArrayBufferStruct Called but index is invaild!{0} Count is {1}",index,Count);
                return;
            }

            _bufferArray[index] = value;
        }
    }

    public ArrayBufferStruct(int size)
    {
        _size = size;
        _curIndex = 0;
        _bufferArray = new T[_size];
    }

    public T GetItem(int index)
    {
        if (index >= _curIndex)
        {
            MyDebug.LogErrorFormat("ArrayIndexOutOfBoundsException index is {0} CurCount is {1}",index,_curIndex);
            return default(T);
        }

        return _bufferArray[index];
    }

    public bool Add(T item)
    {
        if (_curIndex >= _size)
        {
            return false;
        }

        _bufferArray[_curIndex] = item;
        _curIndex++;
        return true;
    }

    public bool RemoveAt(int index)
    {
        if (index < 0)
        {
            return false;
        }

        for (int i = index; i < _curIndex - 1; i++)
        {
            _bufferArray[i] = _bufferArray[i + 1];
        }

        _bufferArray[_curIndex - 1] = default(T);
        _curIndex--;
        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < _size; i++)
        {
            _bufferArray[i] = default(T);
        }
        _curIndex = 0;
    }

    public void Sort(IComparer<T> compariosn)
    {
        if (compariosn == null)
        {
            throw new NullReferenceException("comparison is null!");
        }

        if (_size > 0)
        {
            Array.Sort(_bufferArray,0,_curIndex,compariosn);
        }
    }

}
