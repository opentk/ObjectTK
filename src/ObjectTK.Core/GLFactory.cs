namespace ObjectTK {
    /// Extensible, top-level class responsible for creating all OpenGL objects.<br></br>
    /// ------<br></br>
    /// Usage: GLFactory.Shader.VertexFrag()<br></br>
    ///  <br></br>
    /// To extend this class, add extension methods to the GLxFactory types, where 'x' is one of the field names.<br></br>
    /// e.g. <see cref="GLShaderFactory"/> or <see cref="GLVertexArrayFactory"/>
    public static class GLFactory {

        public static readonly GLShaderFactory Shader = GLShaderFactory.Instance;
        public static readonly GLBufferFactory Buffer = GLBufferFactory.Instance;
        public static readonly GLVertexArrayFactory VertexArray = GLVertexArrayFactory.Instance;
        public static readonly GLTextureFactory Texture = GLTextureFactory.Instance;
    }
    
}
