using System.IO;
using UnityEditor;
using UnityEngine;

namespace KindredSDK.Editor
{
    public abstract class WizardAction
    {
        // public WizardActionState State { get; protected set; } = WizardActionState.Unconfigured;
        public string Name { get; }
        public string Description { get; protected set; }
        protected string PathToAndroid => Path.Combine(Application.dataPath, "Plugins/Android");

        public WizardAction(string name)
        {
            Name = name;

            var isConfigured = CheckIfActionIsConfigured();
            if(isConfigured)
            {
                //State = WizardActionState.Configured;
            }
        }

        public void Configure()
        {
            DoConfiguration();
            //State = WizardActionState.Configured;
        }

        protected abstract bool CheckIfActionIsConfigured();
        protected abstract void DoConfiguration();

        protected Texture2D ScaleAndMakeGrayscale(Texture2D src, int width, int height)
        {
            RenderTexture rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(src, rt);

            RenderTexture currentActiveRT = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height);

            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            var texColors = tex.GetPixels();
            for (int i = 0; i < texColors.Length; i++)
            {
                var grayValue = texColors[i].grayscale;
                texColors[i] = new Color(grayValue, grayValue, grayValue, texColors[i].a);
            }
            tex.SetPixels(texColors);
            tex.Apply();

            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = currentActiveRT;

            return tex;
        }

        protected Texture2D ScaleTexture(Texture src, int width, int height)
        {
            RenderTexture rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(src, rt);

            RenderTexture currentActiveRT = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height);

            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();

            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = currentActiveRT;

            return tex;
        }

        protected static void SaveTextureAsPNG(Texture2D texture, string fullPath)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        protected static Texture2D LoadTextureFromAssetDatabase(string path)
        {
#if UNITY_EDITOR
            return (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
#else
            return null;
#endif
        }
    }
}
