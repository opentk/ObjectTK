using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using ObjectTK.GLObjects;

namespace ObjectTK.Extensions.Variables {
	public static class UniformInfoExtensions {

		public static void Set<T>(this ShaderUniformInfo<T> shaderUniform, T Value) {
			shaderUniform.SetAction?.Invoke(shaderUniform.Location, Value);
		}
	}

	public class ShaderUniformInfo<T> : ShaderUniformInfo {

		internal Action<int, T> SetAction;

		public ShaderUniformInfo(int ProgramHandle, string Name, int Location, int UniformSize, ActiveUniformType UniformType, bool Active) :
			base(Name, Location, UniformSize, UniformType) {
			SetAction = UniformSetAction.FindAction<T>();
		}

		public class UniformSetAction {

			public static Action<int, V> FindAction<V>() {
				if (SetActions.TryGetValue(typeof(V), out UniformSetAction SetAction)) {
					return (SetAction as GenericUniformSetAction<V>).Action;
				}
				throw new Exception($"Uniform type not supported: {typeof(T).FullName}");
			}

			private static Dictionary<Type, UniformSetAction> SetActions;
			static UniformSetAction() {
				SetActions = new Dictionary<Type, UniformSetAction>()
				{
					{ typeof(bool),         new GenericUniformSetAction<bool>       ((Location, Value) => GL.Uniform1(Location, Value ? 1 : 0)) },
					{ typeof(int),          new GenericUniformSetAction<int>        (GL.Uniform1) },
					{ typeof(uint),         new GenericUniformSetAction<uint>       (GL.Uniform1) },
					{ typeof(float),        new GenericUniformSetAction<float>      (GL.Uniform1) },
					{ typeof(double),       new GenericUniformSetAction<double>     (GL.Uniform1) },
					{ typeof(Half),         new GenericUniformSetAction<Half>       ((Location, Half) => GL.Uniform1(Location, Half)) },
					{ typeof(Color),        new GenericUniformSetAction<Color>      ((Location, Color) => GL.Uniform4(Location, Color)) },
					{ typeof(Vector2),      new GenericUniformSetAction<Vector2>    (GL.Uniform2) },
					{ typeof(Vector3),      new GenericUniformSetAction<Vector3>    (GL.Uniform3) },
					{ typeof(Vector4),      new GenericUniformSetAction<Vector4>    (GL.Uniform4) },
					{ typeof(Vector2d),     new GenericUniformSetAction<Vector2d>   ((Location, Vector) => GL.Uniform2(Location, Vector.X, Vector.Y)) },
					{ typeof(Vector2h),     new GenericUniformSetAction<Vector2h>   ((Location, Vector) => GL.Uniform2(Location, Vector.X, Vector.Y)) },
					{ typeof(Vector3d),     new GenericUniformSetAction<Vector3d>   ((Location, Vector) => GL.Uniform3(Location, Vector.X, Vector.Y, Vector.Z)) },
					{ typeof(Vector3h),     new GenericUniformSetAction<Vector3h>   ((Location, Vector) => GL.Uniform3(Location, Vector.X, Vector.Y, Vector.Z)) },
					{ typeof(Vector4d),     new GenericUniformSetAction<Vector4d>   ((Location, Vector) => GL.Uniform4(Location, Vector.X, Vector.Y, Vector.Z, Vector.W)) },
					{ typeof(Vector4h),     new GenericUniformSetAction<Vector4h>   ((Location, Vector) => GL.Uniform4(Location, Vector.X, Vector.Y, Vector.Z, Vector.W)) },
					{ typeof(Matrix2),      new GenericUniformSetAction<Matrix2>    ((Location, Matrix) => GL.UniformMatrix2(Location, false, ref Matrix)) },
					{ typeof(Matrix3),      new GenericUniformSetAction<Matrix3>    ((Location, Matrix) => GL.UniformMatrix3(Location, false, ref Matrix)) },
					{ typeof(Matrix4),      new GenericUniformSetAction<Matrix4>    ((Location, Matrix) => GL.UniformMatrix4(Location, false, ref Matrix)) },
					{ typeof(Matrix2x3),    new GenericUniformSetAction<Matrix2x3>  ((Location, Matrix) => GL.UniformMatrix2x3(Location, false, ref Matrix)) },
					{ typeof(Matrix2x4),    new GenericUniformSetAction<Matrix2x4>  ((Location, Matrix) => GL.UniformMatrix2x4(Location, false, ref Matrix)) },
					{ typeof(Matrix3x2),    new GenericUniformSetAction<Matrix3x2>  ((Location, Matrix) => GL.UniformMatrix3x2(Location, false, ref Matrix)) },
					{ typeof(Matrix3x4),    new GenericUniformSetAction<Matrix3x4>  ((Location, Matrix) => GL.UniformMatrix3x4(Location, false, ref Matrix)) },
					{ typeof(Matrix4x2),    new GenericUniformSetAction<Matrix4x2>  ((Location, Matrix) => GL.UniformMatrix4x2(Location, false, ref Matrix)) },
					{ typeof(Matrix4x3),    new GenericUniformSetAction<Matrix4x3>  ((Location, Matrix) => GL.UniformMatrix4x3(Location, false, ref Matrix)) }
				};
			}

			public class GenericUniformSetAction<U> : UniformSetAction {
				public Action<int, U> Action { get; set; }
				public GenericUniformSetAction(Action<int, U> Action) {
					this.Action = Action;
				}
			}
		}
	}
}
