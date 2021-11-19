using System;
using System.Collections.Generic;
using System.Reflection;
using Urho;

namespace Urho
{
	public abstract class BaseUniforms
	{
		private List<FieldInfo> floats = new List<FieldInfo>();
		private List<FieldInfo> vec2s = new List<FieldInfo>();
		private List<FieldInfo> vec3s = new List<FieldInfo>();
		private List<FieldInfo> vec4s = new List<FieldInfo>();

		protected BaseUniforms()
		{
			foreach (FieldInfo f in GetType().GetRuntimeFields())
			{
				if (f.FieldType == typeof(float)) floats.Add(f);
				else if (f.FieldType == typeof(Vector2)) vec2s.Add(f);
				else if (f.FieldType == typeof(Vector3)) vec3s.Add(f);
				else if (f.FieldType == typeof(Vector4)) vec4s.Add(f);
				else throw new Exception("Unknown uniform type!");
			}
		}

		public void SetInMaterial(Material material)
		{
			foreach (FieldInfo f in floats)
			{
				material.SetShaderParameter(f.Name, (float)f.GetValue(this));
			}
			foreach (FieldInfo f in vec2s)
			{
				material.SetShaderParameter(f.Name, (Vector2)f.GetValue(this));
			}
			foreach (FieldInfo f in vec3s)
			{
				material.SetShaderParameter(f.Name, (Vector3)f.GetValue(this));
			}
			foreach (FieldInfo f in vec4s)
			{
				material.SetShaderParameter(f.Name, (Vector4)f.GetValue(this));
			}
		}
	}
}
