using System;
using UnityEngine;
public class RPGMath2D
{
    public static readonly float FRONT_X = 0.0f;
    public static readonly float FRONT_Y = -1.0f;

    public static float Length(float x, float y)
    {
        return (float)Math.Sqrt(x * x + y * y);
    }

    public static float Length2(float x, float y)
    {
        return (x * x + y * y);
    }
    public static float Length(float fSrcX, float fSrcY, float fDesX, float fDesY)
    {
        float deltaX = fDesX - fSrcX;
        float deltaY = fDesY - fSrcY;
        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public static float Length(Vector3 pos1, Vector3 pos2)
    {
        return Length(pos1.x, pos1.y, pos2.x, pos2.y);
    }

    public static float Length2(float fSrcX, float fSrcY, float fDesX, float fDesY)
    {
        float deltaX = fDesX - fSrcX;
        float deltaY = fDesY - fSrcY;
        return (deltaX * deltaX + deltaY * deltaY);
    }

    public static float Length2(Vector3 pos1, Vector3 pos2)
    {
        return Length2(pos1.x, pos1.y, pos2.x, pos2.y);
    }

    public static float Rad2Deg(float radian)
    {
        float degree = 180f - radian / Mathf.PI * 180f;
        return degree;
    }

    public static float Deg2Rad(float degree)
    {
        float radian = (180.0f - degree) * Mathf.PI / 180.0f;
        return radian;
    }

    public static float GetYaw(float radian)
    {
        float angle = Rad2Deg(radian);
        Vector3 direction = new Vector3(0, 1.0f, 0);
        RPGMath2D.Rotate(direction.x, direction.y, -angle * Mathf.Deg2Rad, ref direction.x, ref direction.y);
        return GeographicObject.GetDirection(direction.x, direction.y);
    }
    public static float GetAngle(float fX, float fY)
    {
        float angle = (float)Math.Atan2(fX, -fY);
        return angle;
    }
    public static void CaculateAngle(float fAngle, ref float fX, ref float fY)
    {
        fY = -(float)Math.Cos(fAngle);
        fX = (float)Math.Sin(fAngle);
    }
    public static float GetRotateAngle(float fSrcX, float fSrcy, float fDesX, float fDesY)
    {
        //if(abs(fSrcX) <= 0.001f && abs(fSrcy) <= 0.001f 
        //	|| abs(fDesX) <= 0.001f && abs(fDesY) <= 0.001f)
        //{
        //	return 0.0f;
        //}
        float fDesAngle = GetAngle(fDesX, fDesY);
        float fSrcAngle = GetAngle(fSrcX, fSrcy);
        float fRotateAngle = fDesAngle - fSrcAngle;
        // 映射到(-M_PI, M_PI)区间上来
        if (fRotateAngle > Mathf.PI)
            fRotateAngle -= Mathf.PI * 2;
        else if (fRotateAngle < -Mathf.PI)
            fRotateAngle += Mathf.PI * 2;
        return fRotateAngle;
    }

    public static void Rotate(float fSrcX, float fSrcy, float fRotateAngle, ref float fDesX, ref float fDesY)
    {
        //! 逆时针旋转
        float fSin = (float)Math.Sin(fRotateAngle);
        float fCon = (float)Math.Cos(fRotateAngle);
        //! 顺时针旋转
        //float fSin = sin(-fRotateAngle);
        //float fCon = cos(-fRotateAngle);
        fDesX = fCon * fSrcX - fSin * fSrcy;
        fDesY = fSin * fSrcX + fCon * fSrcy;
    }

    public static void Rotate(ref float fX, ref float fY, float fRotateAngle)
    {
        float fSin = (float)Math.Sin(fRotateAngle);
        float fCon = (float)Math.Cos(fRotateAngle);
        //! 顺时针旋转
        //float fSin = sin(-fRotateAngle);
        //float fCon = cos(-fRotateAngle);
        float fDesX = fCon * fX - fSin * fY;
        float fDesY = fSin * fX + fCon * fY;
        fX = fDesX;
        fY = fDesY;
    }

    public static void RotateRight90(ref float x, ref float y)
    {
        float temp = x;
        x = y;
        y = -temp;
    }
    public static void RotateLeft90(ref float x, ref float y)
    {
        float temp = x;
        x = -y;
        y = temp;
    }
    public static int GetSide(float fromX, float fromY, float toX, float toY, float x, float y)
    {
        float s = (fromX - x) * (toY - y) - (fromY - y) * (toX - x);
        if (s == 0)
        {
            return 0;
        }
        else if (s < 0)//! 右侧
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}

