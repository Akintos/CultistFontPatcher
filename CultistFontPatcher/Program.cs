using CultistFontPatcher.AssetTypes;
using System;
using System.IO;
using UnityAssetLib;
using UnityAssetLib.Serialization;
using UnityAssetLib.Types;

namespace CultistFontPatcher
{
    class Program
    {
        private const string GAME_DIR_NAME = "Cultist Simulator";
        private const string GAME_EXE_NAME = "cultistsimulator.exe";
        private const string GAME_DATA_DIR_NAME = "cultistsimulator_Data";

        private const string RESOURCES_FILENAME = "sharedassets0.assets";
        private const string FONT_TEXTURE_FILENAME = "KRFontTexture.bin";

        private static readonly string[] defaultPaths = {
            @".",
            @"..",
            @"..\..",
        };

        private static readonly string[] replaceFontNames = {
            "MenuTitle_Belgrad",
            "Text_Philosopher",
            // "Numbers_Titania",
            // "NotoSans-Regular SDF"
        };

        private static readonly string[] addFallbackFontNames = {
            // "MenuTitle_Belgrad",
            // "NotoSans-Regular SDF",
            // "NotoSansCJKsc-Regular",
            "Numbers_Titania",
            // "Text_Philosopher",
            // "LiberationSans SDF"
        };

        static void Main(string[] args)
        {
#if DEBUG
            Patch();
#else
            try
            {
                Patch();
            }
            catch (AssetNotFoundException e)
            {
                Console.Error.WriteLine($"{e.Message} 폰트를 찾을 수 없습니다. 이미 패치된 파일인지 확인하세요.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            Console.WriteLine("아무 키나 누르세요...");
            Console.ReadKey();
#endif
        }

        public static void Patch()
        {
            Console.WriteLine("컬티스트 시뮬레이터 한글 폰트 패치\n\t\t\t\t\t제작자 akintos\n");
            var gamePath = FindGame();
            if (gamePath == null)
            {
                Console.WriteLine("게임을 찾지 못했습니다. 패치 프로그램을 게임 설치 경로에 넣어주세요.");
                return;
            }

            var gameDataDir = Path.Combine(gamePath, GAME_DATA_DIR_NAME);
            var resourcesAssetPath = Path.Combine(gameDataDir, RESOURCES_FILENAME);

            Console.WriteLine();

            if (!File.Exists(resourcesAssetPath))
            {
                Console.WriteLine($"{resourcesAssetPath} 파일을 찾을 수 없습니다. 게임 파일을 다시 확인하세요.");
                return;
            }

            if (IsFileLocked(resourcesAssetPath))
            {
                Console.WriteLine($"{resourcesAssetPath} 파일이 사용중입니다. 게임이 켜져있는지 확인하고 다시 실행해 주세요.");
                return;
            }

            PatchSDF(resourcesAssetPath);

            File.WriteAllBytes(Path.Combine(gameDataDir, FONT_TEXTURE_FILENAME), Properties.Resources.FontTexture);
            Console.WriteLine("한글 폰트 패치가 완료되었습니다.");
        }

        private static string FindGame()
        {
            Console.WriteLine("게임 설치 경로 탐색중...");

            foreach (var possibleDir in defaultPaths)
            {
                if (File.Exists(Path.Combine(possibleDir, GAME_EXE_NAME)))
                {
                    return Path.GetFullPath(possibleDir);
                }
            }

            try
            {
                var steamPath = SteamFinder.FindSteamPath();
                if (steamPath != null)
                {
                    Console.WriteLine("스팀 설치 경로를 찾았습니다.");
                    string[] libraryPaths = SteamFinder.GetLibraryPaths(steamPath);

                    foreach (string libraryPath in libraryPaths)
                    {
                        var matches = Directory.GetDirectories(libraryPath, GAME_DIR_NAME);
                        if (matches.Length >= 1)
                            return matches[0];
                    }
                }
            }
            catch
            {
                
            }

            return null;
        }

        private static void PatchSDF(string path)
        {
            string backupPath = path + ".bak";
            string tempPath = path + ".tmp";

            using (AssetsFile f = AssetsFile.Open(path))
            {
                UnitySerializer serializer = new UnitySerializer(f);
                TMP_FontAsset_3_0 newFont = serializer.Deserialize<TMP_FontAsset_3_0>(Properties.Resources.FontDef);

                long patchedFontPathId = -1;

                foreach (var assetName in replaceFontNames)
                {
                    AssetInfo fontAsset = f.GetAssetByName(assetName);
                    if (fontAsset == null)
                        throw new AssetNotFoundException(assetName);
                    patchedFontPathId = fontAsset.pathID;
                    TMP_FontAsset_3_0 oldFont = serializer.Deserialize<TMP_FontAsset_3_0>(fontAsset);

                    newFont.m_Script = oldFont.m_Script;
                    newFont.material = oldFont.material;
                    newFont.atlas    = oldFont.atlas;
                    newFont.m_AtlasTextures = oldFont.m_AtlasTextures;

                    f.ReplaceAsset(fontAsset.pathID, serializer.Serialize(newFont));

                    int imageSize = newFont.m_AtlasWidth;

                    var atlas = serializer.Deserialize<Texture2D_2020_2_1_f1>(f.assets[oldFont.m_AtlasTextures[0].m_PathID]);
                    atlas.m_Width = imageSize;
                    atlas.m_Height = imageSize;
                    atlas.m_CompleteImageSize = imageSize * imageSize;
                    atlas.m_StreamData.offset = 0;
                    atlas.m_StreamData.size = (uint)(imageSize * imageSize);
                    atlas.m_StreamData.path = FONT_TEXTURE_FILENAME;

                    atlas.imageData = new byte[0];

                    f.ReplaceAsset(oldFont.m_AtlasTextures[0].m_PathID, serializer.Serialize(atlas));
                }

                foreach (var assetName in addFallbackFontNames)
                {
                    AssetInfo fontAsset = f.GetAssetByName(assetName);
                    TMP_FontAsset_3_0 font = serializer.Deserialize<TMP_FontAsset_3_0>(fontAsset);

                    font.m_FallbackFontAssetTable = new[] { new PPtr() { m_FileID = 0, m_PathID = patchedFontPathId } };
                    font.fallbackFontAssets = new[] { new PPtr() { m_FileID = 0, m_PathID = patchedFontPathId } };

                    f.ReplaceAsset(fontAsset.pathID, serializer.Serialize(font));
                }

                f.Save(tempPath);
            }

            if (File.Exists(backupPath)) File.Delete(backupPath);
            File.Move(path, backupPath);
            File.Move(tempPath, path);
        }

        protected static bool IsFileLocked(string path)
        {
            try
            {
                using (FileStream stream = new FileInfo(path).Open(FileMode.Open, FileAccess.Read, FileShare.None))
                    stream.Close();
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        public class AssetNotFoundException : Exception
        {
            public AssetNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
