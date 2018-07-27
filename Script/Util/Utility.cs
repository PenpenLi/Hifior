using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Utility
{
	public enum NameResult
	{
		Vaild,
		Null,
		OutOfLength,
		InVaildChar
	}

	public enum enDTFormate
	{
		FULL,
		DATE,
		TIME
	}

	public const long UTC_OFFSET_LOCAL = 28800L;

	public const long UTCTICK_PER_SECONDS = 10000000L;

	public static readonly int MIN_EN_NAME_LEN = 1;

	public static readonly int MAX_EN_NAME_LEN = 12;

	public static uint s_daySecond = 86400u;

	private static ulong[] _DW = new ulong[]
	{
		10000000000uL,
		100000000uL,
		1000000uL,
		10000uL,
		100uL
	};

	private static readonly int CHINESE_CHAR_START = Convert.ToInt32("4e00", 16);

	private static readonly int CHINESE_CHAR_END = Convert.ToInt32("9fff", 16);

	private static readonly char[] s_ban_chars = new char[]
	{
		Convert.ToChar(10),
		Convert.ToChar(13)
	};

	public static int TimeToFrame(float time)
	{
		return (int)Math.Ceiling((double)(time / Time.fixedDeltaTime));
	}

	public static float FrameToTime(int frame)
	{
		return (float)frame * Time.fixedDeltaTime;
	}

	public static Vector3 GetGameObjectSize(GameObject obj)
	{
		Vector3 result = Vector3.zero;
		if (obj.GetComponent<Renderer>() != null)
		{
			result = obj.GetComponent<Renderer>().bounds.size;
		}
		Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			Vector3 size = renderer.bounds.size;
			result.x = Math.Max(result.x, size.x);
			result.y = Math.Max(result.y, size.y);
			result.z = Math.Max(result.z, size.z);
		}
		return result;
	}

	public static GameObject FindChild(GameObject p, string path)
	{
		if (p != null)
		{
			Transform transform = p.transform.Find(path);
			return (!(transform != null)) ? null : transform.gameObject;
		}
		return null;
	}

	public static GameObject FindChildSafe(GameObject p, string path)
	{
		if (p)
		{
			Transform transform = p.transform.Find(path);
			if (transform)
			{
				return transform.gameObject;
			}
		}
		return null;
	}

	public static T GetComponetInChild<T>(GameObject p, string path) where T : MonoBehaviour
	{
		if (p == null || p.transform == null)
		{
			return (T)((object)null);
		}
		Transform transform = p.transform.Find(path);
		if (transform == null)
		{
			return (T)((object)null);
		}
		return transform.GetComponent<T>();
	}

	public static GameObject FindChildByName(Component component, string childpath)
	{
		return Utility.FindChildByName(component.gameObject, childpath);
	}

	public static GameObject FindChildByName(GameObject root, string childpath)
	{
		GameObject result = null;
		string[] array = childpath.Split(new char[]
		{
			'/'
		});
		GameObject gameObject = root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = array2[i];
			bool flag = false;
			IEnumerator enumerator = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					if (transform.gameObject.name == text)
					{
						gameObject = transform.gameObject;
						flag = true;
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			if (!flag)
			{
				break;
			}
		}
		if (gameObject != root)
		{
			result = gameObject;
		}
		return result;
	}

	public static void SetChildrenActive(GameObject root, bool active)
	{
		IEnumerator enumerator = root.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				if (transform != root.transform)
				{
					transform.gameObject.SetActive(active);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void StringToByteArray(string str, ref byte[] buffer)
	{
		byte[] bytes = Encoding.Default.GetBytes(str);
		Array.Copy(bytes, buffer, Math.Min(bytes.Length, buffer.Length));
		buffer[buffer.Length - 1] = 0;
	}

	public static string UTF8Convert(string str)
	{
		return str;
	}

	public static string UTF8Convert(byte[] p)
	{
		return StringHelper.UTF8BytesToString(ref p);
	}

	public static byte[] BytesConvert(string s)
	{
		return Encoding.UTF8.GetBytes(s.TrimEnd(new char[1]));
	}

	public static byte[] SByteArrToByte(sbyte[] p)
	{
		byte[] array = new byte[p.Length];
		for (int i = 0; i < p.Length; i++)
		{
			array[i] = (byte)p[i];
		}
		return array;
	}

	public static string UTF8Convert(sbyte[] p, int len)
	{
		byte[] p2 = Utility.SByteArrToByte(p);
		return Utility.UTF8Convert(p2);
	}

	public static DateTime ToUtcTime2Local(long secondsFromUtcStart)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		return dateTime.AddTicks((secondsFromUtcStart + 28800L) * 10000000L);
	}

	public static uint ToUtcSeconds(DateTime dateTime)
	{
		DateTime dateTime2 = new DateTime(1970, 1, 1);
		if (dateTime.CompareTo(dateTime2) < 0)
		{
			return 0u;
		}
		if ((dateTime - dateTime2).TotalSeconds > 28800.0)
		{
			return (uint)(dateTime - dateTime2).TotalSeconds - 28800u;
		}
		return 0u;
	}

	public static string GetUtcToLocalTimeStringFormat(ulong secondsFromUtcStart, string format)
	{
		return Utility.ToUtcTime2Local((long)secondsFromUtcStart).ToString(format);
	}

	public static uint GetLocalTimeFormattedStrToUtc(string localTimeFormattedStr)
	{
		DateTime dateTime = new DateTime(Convert.ToInt32(localTimeFormattedStr.Substring(0, 4)), Convert.ToInt32(localTimeFormattedStr.Substring(4, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(6, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(8, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(10, 2)), Convert.ToInt32(localTimeFormattedStr.Substring(12, 2)));
		return Utility.ToUtcSeconds(dateTime);
	}

	public static uint GetNewDayDeltaSec(int nowSec)
	{
		DateTime dateTime = Utility.ToUtcTime2Local((long)nowSec);
		DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 1);
		DateTime dateTime3 = dateTime2.AddSeconds(86400.0);
		return (uint)(dateTime3 - dateTime).TotalSeconds;
	}

	public static bool IsSameDay(long secondsFromUtcStart1, long secondsFromUtcStart2)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dateTime2 = dateTime.AddTicks((secondsFromUtcStart1 + 28800L) * 10000000L);
		DateTime dateTime3 = dateTime.AddTicks((secondsFromUtcStart2 + 28800L) * 10000000L);
		return DateTime.Equals(dateTime2.Date, dateTime3.Date);
	}

	public static bool IsSameWeek(DateTime dtmS, DateTime dtmE)
	{
		double totalDays = (dtmE - dtmS).TotalDays;
		int num = Convert.ToInt32(dtmE.DayOfWeek);
		if (num == 0)
		{
			num = 7;
		}
		return totalDays < 7.0 && totalDays < (double)num;
	}

	public static bool IsSameWeek(long secondsFromUtcStart1, long secondsFromUtcStart2)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dtmS = dateTime.AddTicks((secondsFromUtcStart1 + 28800L) * 10000000L);
		DateTime dtmE = dateTime.AddTicks((secondsFromUtcStart2 + 28800L) * 10000000L);
		return Utility.IsSameWeek(dtmS, dtmE);
	}

	public static long GetZeroBaseSecond(long utcSec)
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		DateTime dateTime2 = dateTime.AddTicks((utcSec + 28800L) * 10000000L);
		DateTime dateTime3 = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
		return (long)(dateTime3 - dateTime).TotalSeconds - 28800L;
	}

	public static int Hours2Second(int hour)
	{
		return hour * 60 * 60;
	}

	public static int Minutes2Seconds(int min)
	{
		return min * 60;
	}

	public static DateTime ULongToDateTime(ulong dtVal)
	{
		ulong[] array = new ulong[6];
		for (int i = 0; i < Utility._DW.Length; i++)
		{
			array[i] = dtVal / Utility._DW[i];
			dtVal -= array[i] * Utility._DW[i];
		}
		array[Utility._DW.Length] = dtVal;
		return new DateTime((int)array[0], (int)array[1], (int)array[2], (int)array[3], (int)array[4], (int)array[5]);
	}

	public static DateTime SecondsToDateTime(int y, int m, int d, int secondsInDay)
	{
		int num = secondsInDay / 3600;
		secondsInDay %= 3600;
		return new DateTime(y, m, d, num, secondsInDay / 60, secondsInDay % 60);
	}

	public static DateTime StringToDateTime(string dtStr, DateTime defaultIfNone)
	{
		ulong dtVal;
		if (ulong.TryParse(dtStr, out dtVal))
		{
			return Utility.ULongToDateTime(dtVal);
		}
		return defaultIfNone;
	}

	public static string DateTimeFormatString(DateTime dt, Utility.enDTFormate fm)
	{
		if (fm == Utility.enDTFormate.DATE)
		{
			return string.Format("{0:D4}-{1:D2}-{2:D2}", dt.Year, dt.Month, dt.Day);
		}
		if (fm == Utility.enDTFormate.TIME)
		{
			return string.Format("{0:D2}:{1:D2}:{2:D2}", dt.Hour, dt.Minute, dt.Second);
		}
		return string.Format("{0:D4}-{1:D2}-{2:D2}", dt.Year, dt.Month, dt.Day) + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", dt.Hour, dt.Minute, dt.Second);
	}

	public static string SecondsToTimeText(uint secs)
	{
		uint num = secs / 3600u;
		secs -= num * 3600u;
		uint num2 = secs / 60u;
		secs -= num2 * 60u;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, secs);
	}

	public static bool IsOverOneDay(int timeSpanSeconds)
	{
		TimeSpan timeSpan = new TimeSpan((long)timeSpanSeconds * 10000000L);
		return timeSpan.Days > 0;
	}

	public static bool IsBanChar(char key)
	{
		for (int i = 0; i < Utility.s_ban_chars.Length; i++)
		{
			if (Utility.s_ban_chars[i] == key)
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasBanCharInString(string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (Utility.IsBanChar(str[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsChineseChar(char key)
	{
		int num = Convert.ToInt32(key);
		return num >= Utility.CHINESE_CHAR_START && num <= Utility.CHINESE_CHAR_END;
	}

	public static bool IsSpecialChar(char key)
	{
		return !Utility.IsChineseChar(key) && !char.IsLetter(key) && !char.IsNumber(key);
	}

	public static bool IsValidText(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (Utility.IsSpecialChar(text[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static int GetByteCount(string inputStr)
	{
		int num = 0;
		for (int i = 0; i < inputStr.Length; i++)
		{
			if (Utility.IsQuanjiaoChar(inputStr.Substring(i, 1)))
			{
				num += 2;
			}
			else
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsQuanjiaoChar(string inputStr)
	{
		return Encoding.Default.GetByteCount(inputStr) > 1;
	}

	public static bool IsNullOrEmpty(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return true;
		}
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			if (c != ' ')
			{
				return false;
			}
		}
		return true;
	}

	public static Utility.NameResult CheckRoleName(string inputName)
	{
		int byteCount = Utility.GetByteCount(inputName);
		if (Utility.IsNullOrEmpty(inputName))
		{
			return Utility.NameResult.Null;
		}
		if (Utility.HasBanCharInString(inputName))
		{
			return Utility.NameResult.InVaildChar;
		}
		if (byteCount > Utility.MAX_EN_NAME_LEN || byteCount < Utility.MIN_EN_NAME_LEN)
		{
			return Utility.NameResult.OutOfLength;
		}
		return Utility.NameResult.Vaild;
	}

	public static Type GetType(string typeName)
	{
		if (string.IsNullOrEmpty(typeName))
		{
			return null;
		}
		Type type = Type.GetType(typeName);
		if (type != null)
		{
			return type;
		}
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Assembly assembly = assemblies[i];
			type = assembly.GetType(typeName);
			if (type != null)
			{
				return type;
			}
		}
		return null;
	}

	public static byte[] ObjectToBytes(object obj)
	{
		byte[] buffer;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			buffer = memoryStream.GetBuffer();
		}
		return buffer;
	}

	public static object BytesToObject(byte[] Bytes)
	{
		object result;
		using (MemoryStream memoryStream = new MemoryStream(Bytes))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			result = binaryFormatter.Deserialize(memoryStream);
		}
		return result;
	}

	public static byte[] ReadFile(string path)
	{
		FileStream fileStream = null;
		if (!CFileManager.IsFileExist(path))
		{
			return null;
		}
		try
		{
			fileStream = new FileStream(path, FileMode.Open, FileAccess.Read,FileShare.None);
			int num = (int)fileStream.Length;
			byte[] array = new byte[num];
			fileStream.Read(array, 0, num);
			fileStream.Close();
			fileStream.Dispose();
			return array;
		}
		catch (Exception)
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
		}
		return null;
	}

	public static bool WriteFile(string path, byte[] bytes)
	{
		FileStream fileStream = null;
		bool result;
		try
		{
			if (!CFileManager.IsFileExist(path))
			{
				fileStream = new FileStream(path,FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			}
			else
			{
				fileStream = new FileStream(path, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
			}
			fileStream.Write(bytes, 0, bytes.Length);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
			result = true;
		}
		catch (Exception)
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream.Dispose();
			}
			result = false;
		}
		return result;
	}

	public static string CreateMD5Hash(string input)
	{
		MD5 mD = MD5.Create();
		byte[] bytes = Encoding.ASCII.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static float Divide(uint a, uint b)
	{
		if (b == 0u)
		{
			return 0f;
		}
		return a / b;
	}

	public static int GetCpuCurrentClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/scaling_cur_freq");
	}

	public static int GetCpuMaxClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_max_freq");
	}

	public static int GetCpuMinClock()
	{
		return Utility.GetCpuClock("/sys/devices/system/cpu/cpu0/cpufreq/cpuinfo_min_freq");
	}

	private static int GetCpuClock(string cpuFile)
	{
		string fileContent = Utility.getFileContent(cpuFile);
		int num = 0;
		if (!int.TryParse(fileContent, out num))
		{
			num = 0;
		}
		return Mathf.FloorToInt((float)(num / 1000));
	}

	private static string getFileContent(string path)
	{
		string result;
		try
		{
			string text = File.ReadAllText(path);
			result = text;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			result = null;
		}
		return result;
	}

	public static bool IsValidPlayer(ulong uid, int logicWorldId)
	{
		return uid > 0uL && logicWorldId > 0;
	}

	public static T DeepCopyByReflection<T>(T obj)
	{
		Type type = obj.GetType();
		if (obj is string || type.IsValueType)
		{
			return obj;
		}
		if (type.IsArray)
		{
			Type type2 = Type.GetType(type.FullName.Replace("[]", string.Empty));
			Array array = obj as Array;
			Array array2 = Array.CreateInstance(type2, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				array2.SetValue(Utility.DeepCopyByReflection<object>(array.GetValue(i)), i);
			}
			return (T)((object)Convert.ChangeType(array2, obj.GetType()));
		}
		object obj2 = Activator.CreateInstance(obj.GetType());
		PropertyInfo[] properties = obj.GetType().GetProperties((BindingFlags)(60));
		PropertyInfo[] array3 = properties;
		for (int j = 0; j < array3.Length; j++)
		{
			PropertyInfo propertyInfo = array3[j];
			object value = propertyInfo.GetValue(obj, null);
			if (value != null)
			{
				propertyInfo.SetValue(obj2, Utility.DeepCopyByReflection<object>(value), null);
			}
		}
		FieldInfo[] fields = obj.GetType().GetFields((BindingFlags)60);
		FieldInfo[] array4 = fields;
		for (int k = 0; k < array4.Length; k++)
		{
			FieldInfo fieldInfo = array4[k];
			try
			{
				fieldInfo.SetValue(obj2, Utility.DeepCopyByReflection<object>(fieldInfo.GetValue(obj)));
			}
			catch
			{
			}
		}
		return (T)((object)obj2);
	}

	public static T DeepCopyBySerialization<T>(T obj)
	{
		object obj2;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, obj);
			memoryStream.Seek(0L, 0);
			obj2 = binaryFormatter.Deserialize(memoryStream);
			memoryStream.Close();
		}
		return (T)((object)obj2);
	}
}
