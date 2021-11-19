using System;

using Urho.IO;

namespace Urho
{
	public static class Files
	{
		public static string ReadText(this File file)
		{
			return file.ReadText(System.Text.Encoding.UTF8);
		}

		public static string ReadText(this File file, System.Text.Encoding encoding)
		{
			byte[] data = file.ReadBinary();
			return encoding.GetString(data, 0, data.Length);
		}

		public static byte[] ReadBinary(this File file)
		{
			uint size = file.Size;
			byte[] data = new byte[size];
			uint count = file.Read(data, size);
			if (count != size)
				throw new Exception("Files.ReadBinary failed: " + file.TypeName);
			return data;
		}
	}
}
