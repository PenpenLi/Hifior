using System;
using UnityEngine;


public interface IGeographicInterface
{
	Vector3 Position { get; set; }
	float BodyRadius { get;}
	float Yaw { get; set; }

	float Aim(Vector3 pos);
	float Aim(GeographicObject obj);
	void TurnAngle(float deltaAngle);
	float GetAngleFromLocation(Vector3 pos);
	float GetAngleFromDirection(float deltaX, float deltaY);
	bool IsInFront(Vector3 pos);
	float GetDistance(Vector3 kPos);
	float GetDistance(GeographicObject obj);
	Vector3 GetIntersectionPosByDist(Vector3 toPos, float distance);
	Vector3 GetIntersectionPosByPerc(Vector3 toPos, float perc);
	Vector3 GetRoundPos(float angle, float distance);
}



public abstract class GeographicObject :  IGeographicInterface
{
	//+-------------------------------------------------------------------------------------------------------------------------------------------------------------
	public static readonly Vector3 FRONT = new Vector3(RPGMath2D.FRONT_X, RPGMath2D.FRONT_Y, 0);
	public static readonly Vector3 VERTICAL = new Vector3(0, 0, 1);

	public static float GetDirection(float x, float y)
	{
		return RPGMath2D.GetRotateAngle(RPGMath2D.FRONT_X, RPGMath2D.FRONT_Y, x, y);
	}
	public static void GetRotate(float angle, ref float x, ref float y)
	{
		RPGMath2D.Rotate(RPGMath2D.FRONT_X, RPGMath2D.FRONT_Y, angle, ref x, ref y);
	}
	public static float GetDistance(Vector3 pos1, Vector3 pos2)
	{
		return GetHorizontalDistance(pos1, pos2);
	}
	public static float GetDistance2(Vector3 pos1, Vector3 pos2)
	{
		return GetHorizontalDistance2(pos1, pos2);
	}

	public static float GetHorizontalDistance(Vector3 pos1, Vector3 pos2)
	{
		float deltaX = pos1.x - pos2.x;
		float deltaY = pos1.y - pos2.y;
		return Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
	}
	public static float GetHorizontalDistance2(Vector3 pos1, Vector3 pos2)
	{
		float deltaX = pos1.x - pos2.x;
		float deltaY = pos1.y - pos2.y;
		return ((deltaX * deltaX) + (deltaY * deltaY));
	}
	//+-------------------------------------------------------------------------------------------------------------------------------------------------------------

	public abstract Vector3 Position { get; set; }
	public abstract float BodyRadius { get; }
	public abstract float Yaw { get; set; }

	public float Aim(Vector3 pos)
	{
		float angle = GetAngleFromLocation(pos);
		TurnAngle(angle);
		return angle;
	}
	public float Aim(GeographicObject obj)
	{
		if (obj == null)
		{
			return 0.0f;
		}
		else if (obj != this)
		{
			return Aim(obj.Position);
		}
		else
		{
			return 0.0f;
		}
	}
	public void TurnAngle(float deltaAngle)
	{
		Yaw = (Yaw + deltaAngle);
	}

	public float GetAngleFromLocation(Vector3 pos)
	{
		float deltaX = pos.x - Position.x;
		float deltaY = pos.y - Position.y;
		return GetAngleFromDirection(deltaX, deltaY);
	}
	public float GetAngleFromDirection(float deltaX, float deltaY)
	{
		if (Math.Abs(deltaX) < 0.1f && Math.Abs(deltaY) < 0.1f)
		{
			return 0.0f;
		}
		float deltaAngle = GetDirection(deltaX, deltaY) - Yaw;
		return deltaAngle;
	}
	public bool IsInFront(Vector3 pos)
	{
		float angle = GetAngleFromLocation(pos);
		if (-Mathf.PI/2 < angle && angle < Mathf.PI / 2)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public float GetDistance(Vector3 pos)
	{
		float diff = GetHorizontalDistance(pos, Position);
		return Mathf.Max(0.0f, diff - BodyRadius);
	}
	public float GetDistance(GeographicObject obj)
	{
		if (obj == null)
		{
			return 1.0f;
		}
		float diff = GetHorizontalDistance(Position, obj.Position);
		return Mathf.Max(0.0f, diff - BodyRadius - obj.BodyRadius);
	}
	public Vector3 GetIntersectionPosByDist(Vector3 toPos, float distance)	//! fDistance = 0, reach this, fDistance < 0 , penetrate this
	{
		return GetIntersectionPosByDist(Position, toPos, distance);
	}
	public static Vector3 GetIntersectionPosByDist(Vector3 rootPos, Vector3 toPos, float distance)//! fDistance = 0, reach kRootPos, fDistance< 0 , penetrate this
	{
		float fSpan = GetHorizontalDistance(rootPos, toPos);
		if (fSpan != 0.0f)
		{
			float fPerc = distance / fSpan;
			float fDeltaX = toPos.x - rootPos.x;
			float fDeltaY = toPos.y - rootPos.y;
			Vector3 kDest = new Vector3(rootPos.x + fDeltaX * fPerc, rootPos.y + fDeltaY * fPerc, rootPos.z);
			float diff = GetHorizontalDistance(rootPos, kDest);
			UnityEngine.Assertions.Assert.IsTrue(Math.Abs(diff - Math.Abs(distance)) < 0.1f);
			return new Vector3(rootPos.x + fDeltaX * fPerc, rootPos.y + fDeltaY * fPerc, rootPos.z);
		}
		else
		{
			return rootPos;
		}
	}
	public Vector3 GetIntersectionPosByPerc(Vector3 toPos, float perc)	//! fPerc = 0  Reach this, fPerc = 1 Reach From, fPerc < 0 behind self
	{
		float fDeltaX = toPos.x - Position.x;
		float fDeltaY = toPos.y - Position.y;
		return new Vector3(Position.x + fDeltaX * perc, Position.y + fDeltaY * perc, Position.z);
	}
	public Vector3 GetRoundPos(float angle, float distance)
	{
		float x = 0.0f;
		float y = 0.0f;
		GetRotate(Yaw + angle, ref x, ref y);
		x *= distance;
		y *= distance;
		return new Vector3(x, y, 0) + Position;
	}
}