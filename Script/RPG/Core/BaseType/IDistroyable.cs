using System;

public interface IDestroyable
{
	void Destroy();
}

public static class Disposer
{
	public static void Destroy(IDestroyable obj)
	{
		if (obj != null)
		{
			obj.Destroy();
		}
	}
}
