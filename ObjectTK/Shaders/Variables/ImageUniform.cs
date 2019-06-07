//
// ImageUniform.cs
//
// Copyright (C) 2018 OpenTK
//
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
//

using ObjectTK.Textures;
using OpenTK.Graphics.OpenGL;

namespace ObjectTK.Shaders.Variables
{
    /// <summary>
    /// Represents an image uniform.
    /// </summary>
    public sealed class ImageUniform
        : Uniform<int>
    {
        internal ImageUniform()
            : base(GL.Uniform1)
        {
        }

        /// <summary>
        /// Binds the given buffer texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="textureBuffer">The buffer texture to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, TextureBuffer textureBuffer, TextureAccess access)
        {
            Bind(imageUnit, textureBuffer, 0, false, 0, access);
        }

        /// <summary>
        /// Binds a single face of the given texture level to an image unit.<br/>
        /// Calculates the index of the layer-face as 6 * arrayLayer + face.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="arrayLayer">The layer of the texture to bind.</param>
        /// <param name="face">The cube map face to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, TextureCubemapArray texture, int level, int arrayLayer, int face, TextureAccess access)
        {
            // note: the layer parameter indexes the layer-faces, hence the multiplication of the array-layer by 6
            Bind(imageUnit, texture, level, false, 6 * arrayLayer + face, access);
        }

        /// <summary>
        /// Binds a single face of the given texture level to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="face">The cube map face to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, TextureCubemap texture, int level, int face, TextureAccess access)
        {
            Bind(imageUnit, texture, level, false, face, access);
        }

        /// <summary>
        /// Binds a single layer of the given texture level to an image unit.<br/>
        /// Note that for cube maps and cube map arrays the <paramref name="layer"/> parameter actually indexes the layer-faces.<br/>
        /// Thus for cube maps the layer parameter equals the face to be bound.<br/>
        /// For cube map arrays the layer parameter can be calculated as 6 * arrayLayer + face, which is done automatically when using
        /// the corresponding overload <see cref="Bind(int,TextureCubemapArray,int,int,int,OpenTK.Graphics.OpenGL.TextureAccess)"/>.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layer">The layer of the texture to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, LayeredTexture texture, int level, int layer, TextureAccess access)
        {
            Bind(imageUnit, texture, level, false, layer, access);
        }

        /// <summary>
        /// Binds an entire level of the given texture to an image unit.<br/>
        /// The mipmap level defaults to zero.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, Texture texture, TextureAccess access)
        {
            Bind(imageUnit, texture, 0, true, 0, access);
        }

        /// <summary>
        /// Binds an entire level of the given texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, Texture texture, int level, TextureAccess access)
        {
            Bind(imageUnit, texture, level, true, 0, access);
        }

        /// <summary>
        /// Binds a single level of a texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="texture">The texture to bind.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        public void Bind(int imageUnit, Texture texture, int level, bool layered, int layer, TextureAccess access)
        {
            texture.AssertLevel(level);
            Bind(imageUnit, texture.Handle, level, layered, layer, access, texture.InternalFormat);
        }

        /// <summary>
        /// Binds a single level of a texture to an image unit.
        /// </summary>
        /// <param name="imageUnit">The image unit to use.</param>
        /// <param name="textureHandle">The handle of the texture.</param>
        /// <param name="level">The mipmap level to bind.</param>
        /// <param name="layered">Specifies whether a layered texture binding is to be established.</param>
        /// <param name="layer">If <paramref name="layered"/> is false, specifies the layer of the texture to be bound, ignored otherwise.</param>
        /// <param name="access">Specifies the type of access allowed on the image.</param>
        /// <param name="format">Specifies the format that the elements of the texture will be treated as.</param>
        public void Bind(int imageUnit, int textureHandle, int level, bool layered, int layer, TextureAccess access, SizedInternalFormat format)
        {
            Set(imageUnit);
            GL.BindImageTexture(imageUnit, textureHandle, level, layered, layer, access, format);
        }
    }
}