using ObjectTK.Data.Variables;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Extensions.Variables {
	public static class VertexAttributeInfoExtensions {


	}

    public class VertexAttributeInfo<T> : VertexAttributeInfo {
        public VertexAttributeInfo(int ProgramHandle, string Name, bool Active, int Index, int Components, ActiveAttribType Type, bool Normalized) :
            base(ProgramHandle, Name, Active, Index, Components, Type, Normalized) {
        }
    }
}
