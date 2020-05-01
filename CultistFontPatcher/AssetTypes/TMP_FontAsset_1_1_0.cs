using System;
using System.Collections.Generic;
using System.Text;
using UnityAssetLib.Serialization;
using UnityAssetLib.Types;

namespace CultistFontPatcher.AssetTypes
{
    [UnitySerializable]
    public class TMP_FontAsset_1_1_0 : MonoBehaviour
    {
        public int hashCode;
        public PPtr material;
        public int materialHashCode;

        public string m_version;

        public string m_SourceFontGUID;
        public PPtr m_SourceFontFile;

        public int m_AtlasPopulationMode;

        public FaceInfo m_FaceInfo;

        public Glyph[] m_GlyphTable;

        public TMP_Character[] m_CharacterTable;

        public PPtr[] m_AtlasTextures;
        public int m_AtlasTextureIndex;

        public GlyphRect[] m_UsedGlyphRects;
        public GlyphRect[] m_FreeGlyphRects;

        public FaceInfo_Legacy m_fontInfo;

        public PPtr atlas;

        public int m_AtlasWidth;
        public int m_AtlasHeight;
        public int m_AtlasPadding;

        public int m_AtlasRenderMode;

        public TMP_Glyph[] m_glyphInfoList;

        public KerningTable m_KerningTable;

        public TMP_FontFeatureTable m_FontFeatureTable;

        public PPtr[] fallbackFontAssets;

        public PPtr[] m_FallbackFontAssetTable;

        public FontAssetCreationSettings m_CreationSettings;

        public TMP_FontWeightPair[] m_FontWeightTable;
        public TMP_FontWeightPair[] fontWeights;

        public float normalStyle;
        public float normalSpacingOffset;
        public float boldStyle;
        public float boldSpacing;
        public byte italicStyle;
        public byte tabSize;

        [UnitySerializable]
        public class Glyph
        {
            public uint m_Index;
            public GlyphMetrics m_Metrics;
            public GlyphRect m_GlyphRect;
            public float m_Scale;
            public int m_AtlasIndex;
        }

        [UnitySerializable]
        public class FaceInfo
        {
            public string m_FamilyName;
            public string m_StyleName;
            public int m_PointSize;
            public float m_Scale;
            public float m_LineHeight;
            public float m_AscentLine;
            public float m_CapLine;
            public float m_MeanLine;
            public float m_Baseline;
            public float m_DescentLine;
            public float m_SuperscriptOffset;
            public float m_SuperscriptSize;
            public float m_SubscriptOffset;
            public float m_SubscriptSize;
            public float m_UnderlineOffset;
            public float m_UnderlineThickness;
            public float m_StrikethroughOffset;
            public float m_StrikethroughThickness;
            public float m_TabWidth;
        }

        [UnitySerializable]
        public class FaceInfo_Legacy
        {
            public string Name;
            public float PointSize;
            public float Scale;
            public int CharacterCount;
            public float LineHeight;
            public float Baseline;
            public float Ascender;
            public float CapHeight;
            public float Descender;
            public float CenterLine;
            public float SuperscriptOffset;
            public float SubscriptOffset;
            public float SubSize;
            public float Underline;
            public float UnderlineThickness;
            public float strikethrough;
            public float strikethroughThickness;
            public float TabWidth;
            public float Padding;
            public float AtlasWidth;
            public float AtlasHeight;
        }

        [UnitySerializable]
        public class TMP_Glyph
        {
            public int id;

            public float x;
            public float y;

            public float width;
            public float height;

            public float xOffset;
            public float yOffset;

            public float xAdvance;

            public float scale;
        }

        [UnitySerializable]
        public class GlyphMetrics
        {
            public float m_Width;
            public float m_Height;
            public float m_HorizontalBearingX;
            public float m_HorizontalBearingY;
            public float m_HorizontalAdvance;
        }

        [UnitySerializable]
        public class GlyphRect
        {
            public int x;
            public int y;

            public int width;
            public int height;
        }

        [UnitySerializable]
        public class TMP_Character
        {
            public int m_ElementType;
            public uint m_Unicode;
            public uint m_GlyphIndex;
            public float m_scale;
        }

        [UnitySerializable]
        public class KerningTable
        {
            public KerningPair[] kerningPairs;
        }

        [UnitySerializable]
        public class KerningPair
        {
            public uint m_FirstGlyph;
            public Rectf m_FirstGlyphAdjustments;

            public uint m_SecondGlyph;
            public Rectf m_SecondGlyphAdjustments;

            public float xOffset;

            public bool m_IgnoreSpacingAdjustments;
        }

        [UnitySerializable]
        public class FontAssetCreationSettings
        {
            public string sourceFontFileName;
            public string sourceFontFileGUID;
            public int pointSizeSamplingMode;
            public int pointSize;
            public int padding;
            public int packingMode;
            public int atlasWidth;
            public int atlasHeight;
            public int characterSetSelectionMode;
            public string characterSequence;
            public string referencedFontAssetGUID;
            public string referencedTextAssetGUID;
            public int fontStyle;
            public float fontStyleModifier;
            public int renderMode;
            public bool includeFontFeatures;
        }

        [UnitySerializable]
        public class TMP_FontWeightPair
        {
            public PPtr regularTypeface;
            public PPtr italicTypeface;
        }

        [UnitySerializable]
        public class TMP_FontFeatureTable
        {
            public TMP_GlyphPairAdjustmentRecord[] m_GlyphPairAdjustmentRecords;
        }

        [UnitySerializable]
        public class TMP_GlyphPairAdjustmentRecord
        {
            public TMP_GlyphAdjustmentRecord m_FirstAdjustmentRecord;
            public TMP_GlyphAdjustmentRecord m_SecondAdjustmentRecord;
            public int m_FeatureLookupFlags;
        }

        [UnitySerializable]
        public class TMP_GlyphAdjustmentRecord
        {
            public uint m_GlyphIndex;
            public TMP_GlyphValueRecord m_GlyphValueRecord;
        }

        [UnitySerializable]
        public class TMP_GlyphValueRecord
        {
            public float m_XPlacement;
            public float m_YPlacement;
            public float m_XAdvance;
            public float m_YAdvance;
        }
    }
}
