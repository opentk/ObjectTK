DerpGL
======

DerpGL is an abstraction layer on top of OpenTK to provide OpenGL features in an object-oriented and type-safe manner with modern C#-style.

The main functioning parts currently are:
* Buffer object (VBO, UBO, SSBO)
* Shader (Vertex, Geometry, Fragment, Compute)
* Texture (all 11 texture types of OpenGL are supported)
* Sampler object
* Query object
* Framebuffer and Renderbuffer

## Examples
### Shader abstraction
DerpGL provides a base class for shaders which does all the work to get shaders up and running while also simplifying interop with uniforms, vertex attributes, etc.
Most of OpenGLs newer features are supported, e.g. uniform buffers (UBO), shader storage buffers (SSBO), image load/store, compute shaders, etc.
```C#
[VertexShaderSource("ExampleShader.vs")]
[FragmentShaderSource("ExampleShader.fs")]
public class ExampleShader
    : Shader
{
    [VertexAttrib(3, VertexAttribPointerType.Float)]
    public VertexAttrib InVertex { get; protected set; }

    public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
}

// initialize shader (load sources, create/compile/link shader program, error checking)
var shader = new ExampleShader();
```

### Buffer object abstraction
DerpGL also provides a generic type which encapsulates all the work necessary to set up, update, clear and copy buffer objects.

```C#
// create some vertices
var vertices = new[] { new Vector3(-1,-1,0), new Vector3(1,-1,0), new Vector3(0,1,0) };

// create buffer object and upload vertex data
var vbo = new Buffer<Vector3>();
vbo.Init(BufferTarget.ArrayBuffer, vertices);
```

### Render the buffer using the shader above
Once the shader type is set up its usage is straight forward and much less error prone than doing all the stuff manually via OpenTK/OpenGL.
```C#
shader.Use();
shader.ModelViewProjectionMatrix.Set(ModelView*Projection);
shader.InVertex.Bind(vbo);
GL.DrawArrays(PrimitiveType.Triangles, 0, vbo.ElementCount);
```
