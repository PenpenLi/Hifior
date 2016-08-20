using System;
using System.Collections.Generic;

public struct VEnum<T>
{
	public Int32 Value { get { return _value; } }

	public VEnum(Int32 value)
	{
		_value = value;
	}
	public static implicit operator Int32(VEnum<T> data)
	{
		return data.Value;
	}
	public static implicit operator UInt32(VEnum<T> data)
	{
		return (UInt32)data.Value;
	}
	Int32 _value;
}