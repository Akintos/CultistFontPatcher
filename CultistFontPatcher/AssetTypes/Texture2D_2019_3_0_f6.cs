using System;
using System.Collections.Generic;
using System.Text;
using UnityAssetLib.Serialization;
using UnityAssetLib.Types;

namespace CultistFontPatcher.AssetTypes
{
    [UnitySerializable]
    public class Texture2D_2019_3_0_f6 : UnityAssetLib.Types.Object
    {
        public TextureFormat Format
        {
            get => (TextureFormat)m_TextureFormat;
            set => m_TextureFormat = (int)value;
        }

        public bool HasMips { get => m_MipCount > 0; }

        public string m_Name;
        [UnityMinVersion(2017, 3)]
        public int m_ForcedFallbackFormat;
        [UnityMinVersion(2017, 3)]
        public bool m_DownscaleFallback;
        public int m_Width;
        public int m_Height;
        public int m_CompleteImageSize;
        public int m_TextureFormat;
        public int m_MipCount;
        [UnityDoNotAlign]
        public bool m_IsReadable;
        [UnityDoNotAlign]
        public bool m_StreamingMipmaps;
        public int m_StreamingMipmapsPriority;
        public int m_ImageCount;
        public int m_TextureDiemnsion;

        public GLTextureSettings m_TextureSettings;

        public int m_LightmapFormat;
        public int m_ColorSpace;

        public byte[] imageData;

        public StreamingInfo m_StreamData;

        [UnitySerializable]
        public class GLTextureSettings
        {
            public int m_FilterMode;
            public int m_Aniso;
            public float m_MipBias;
            public int m_WrapU;
            [UnityMinVersion(2017)]
            public int m_WrapV;
            [UnityMinVersion(2017)]
            public int m_WrapW;
        }
    }
}
