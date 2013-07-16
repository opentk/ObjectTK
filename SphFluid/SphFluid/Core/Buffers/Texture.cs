using System;
using OpenTK.Graphics.OpenGL;

namespace SphFluid.Core.Buffers
{
    public class Texture
        : ITexture
        , IReleasable
    {
        public bool IsInitialized { get; private set; }

        public int FboHandle
        {
            get
            {
                if (!IsInitialized) throw new ApplicationException("Texture not initialized.");
                return _fboHandle;
            }
        }
        
        public int TextureHandle
        {
            get
            {
                if (!IsInitialized) throw new ApplicationException("Texture not initialized.");
                return _textureHandle;
            }
        }

        private int _fboHandle;
        private int _textureHandle;
        private int _colorBuffer;
        private readonly int _numComponents;
        
        public Texture(int numComponents)
        {
            IsInitialized = false;
            _numComponents = numComponents;
        }

        public void Initialize(int width, int height, int depth)
        {
            var is3D = depth > 0;
            // create fbo
            GL.GenFramebuffers(1, out _fboHandle);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fboHandle);
            // create texture
            _textureHandle = GL.GenTexture();
            if (is3D)
            {
                GL.BindTexture(TextureTarget.Texture3D, _textureHandle);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, _textureHandle);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            // set pixel format according to the number of components required
            PixelInternalFormat internalformat;
            PixelFormat format;
            GetPixelFormats(_numComponents, out internalformat, out format);
            if (is3D)
            {
                GL.TexImage3D(TextureTarget.Texture3D, 0, internalformat, width, height, depth, 0, format, PixelType.HalfFloat, IntPtr.Zero);
            }
            else
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, internalformat, width, height, 0, format, PixelType.HalfFloat, IntPtr.Zero);
            }
            Utility.Assert("Unable to create texture");
            // create color render buffer and attach it to the fbo
            GL.GenRenderbuffers(1, out _colorBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _colorBuffer);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, _textureHandle, 0);
            Utility.Assert("Unable to attach color buffer");
            Utility.Assert(() => GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer), FramebufferErrorCode.FramebufferComplete, "Unable to create FBO");
            // clear color buffer
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            // mark as initialized
            IsInitialized = true;
        }

        private static void GetPixelFormats(int numComponents, out PixelInternalFormat internalFormat, out PixelFormat format)
        {
            switch (numComponents)
            {
                case 1:
                    internalFormat = PixelInternalFormat.R16f;
                    format = PixelFormat.Red;
                    break;
                case 2:
                    internalFormat = PixelInternalFormat.Rg16f;
                    format = PixelFormat.Rg;
                    break;
                case 3:
                    internalFormat = PixelInternalFormat.Rgb16f;
                    format = PixelFormat.Rgb;
                    break;
                case 4:
                    internalFormat = PixelInternalFormat.Rgba16f;
                    format = PixelFormat.Rgba;
                    break;
                default:
                    throw new ArgumentException("Invalid number of components", "numComponents");
            }
        }

        public void Release()
        {
            IsInitialized = false;
            GL.DeleteFramebuffers(1, ref _fboHandle);
            GL.DeleteRenderbuffers(1, ref _colorBuffer);
            GL.DeleteTextures(1, ref _textureHandle);
        }

        public void Clear()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _fboHandle);
            GL.ClearColor(0, 0, 0, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
    }
}