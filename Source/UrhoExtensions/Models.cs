using System.Collections.Generic;
using Urho;
using System;
using System.Linq;

namespace Urho
{
	public static class Models
	{
		public static Model ModelFromMesh<I, F>(IList<I> indices, IList<F> positions, IList<F> normals, string name = "")
		{
			var idata = indices.Select((_) => Convert.ToInt16(_)).ToArray();
			var pdata = positions.Select((_) => Convert.ToSingle(_)).ToArray();
			var ndata = normals.Select((_) => Convert.ToSingle(_)).ToArray();
			return ModelFromMesh(idata, pdata, ndata, name);
		}

		public static Model ModelFromMesh(short[] idata, float[] positions, float[] normals, string name = "")
		{
			float[] vdata = new float[positions.Length * 2];
			for (int i = 0; i < positions.Length; i += 3)
			{
				vdata[2 * i] = positions[i];
				vdata[2 * i + 1] = positions[i + 1];
				vdata[2 * i + 2] = positions[i + 2];
				vdata[2 * i + 3] = normals[i];
				vdata[2 * i + 4] = normals[i + 1];
				vdata[2 * i + 5] = normals[i + 2];
			}

			return ModelFromMesh(idata, vdata, name);
		}

		public static Model ModelFromMesh(short[] idata, float[] vdata, string name = "")
		{
			Model model = CoreAssets.Models.Box.Clone(name);

			IndexBuffer ibuf = model.IndexBuffers[0];
			ibuf.Shadowed = true;
			ibuf.SetSize((uint)idata.Length, false, false);
			ibuf.SetData(idata);

			VertexBuffer vbuf = model.VertexBuffers[0];
			vbuf.Shadowed = true;
			vbuf.SetSize((uint)(vdata.Length / 6), ElementMask.Position | ElementMask.Normal, false);
			vbuf.SetData(vdata);

			Geometry geo = new Geometry();
			geo.SetNumVertexBuffers(1);
			geo.SetVertexBuffer(0, vbuf);
			geo.IndexBuffer = ibuf;
			geo.SetDrawRange(PrimitiveType.TriangleList, 0, (uint)idata.Length, true);

			BoundingBox bbox = new BoundingBox(0, 0);
			for (int i = 0; i < vdata.Length; i += 6)
				bbox.Merge(new Vector3(vdata[i], vdata[i + 1], vdata[i + 2]));

			model.NumGeometries = 1;
			model.SetNumGeometryLodLevels(0, 1);
			model.SetGeometry(0, 0, geo);
			model.BoundingBox = bbox;

			return model;
		}
	}
}
