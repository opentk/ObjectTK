using System;
using OpenTK.Graphics.OpenGL;
using AAT = OpenTK.Graphics.OpenGL.ActiveAttribType;
using VAPT = OpenTK.Graphics.OpenGL.VertexAttribPointerType;


namespace ObjectTK {
    public static class AttributeExtensions {

        public static VAPT ToVertexAttribPointerType(this AAT attrib) {
            switch (attrib) {
                //
                case AAT.Int:
                case AAT.IntVec2:
                case AAT.IntVec3:
                case AAT.IntVec4:
                    return VAPT.Int;
                //
                case AAT.UnsignedInt:
                case AAT.UnsignedIntVec2:
                case AAT.UnsignedIntVec3:
                case AAT.UnsignedIntVec4:
                    return VAPT.UnsignedInt;
                //
                case AAT.Float:
                case AAT.FloatVec2:
                case AAT.FloatVec3:
                case AAT.FloatVec4:
                case AAT.FloatMat2:
                case AAT.FloatMat3:
                case AAT.FloatMat4:
                case AAT.FloatMat2x3:
                case AAT.FloatMat2x4:
                case AAT.FloatMat3x2:
                case AAT.FloatMat3x4:
                case AAT.FloatMat4x2:
                case AAT.FloatMat4x3:
                    return VAPT.Float;
                //
                case AAT.Double:
                case AAT.DoubleMat2:
                case AAT.DoubleMat3:
                case AAT.DoubleMat4:
                case AAT.DoubleMat2x3:
                case AAT.DoubleMat2x4:
                case AAT.DoubleMat3x2:
                case AAT.DoubleMat3x4:
                case AAT.DoubleMat4x2:
                case AAT.DoubleMat4x3:
                case AAT.DoubleVec2:
                case AAT.DoubleVec3:
                case AAT.DoubleVec4:
                    return VAPT.Double;
                case AAT.None:
                    throw new ArgumentOutOfRangeException(nameof(attrib), attrib, $"{nameof(ActiveAttribType.None)} does not have a valid conversion.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(attrib), attrib, null);
            }
        }
        
    }
}
