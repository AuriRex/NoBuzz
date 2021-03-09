using HarmonyLib;
using IPA;
using System.Reflection;
using UnityEngine;

namespace NoBuzz
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static string hid = "com.aurirex.nobuzz";

        internal Harmony harmony;

        internal IPA.Logging.Logger Log { get; private set; }

        [Init]
        public Plugin(IPA.Logging.Logger logger)
        {
            Log = logger;
        }

        [OnEnable]
        public void OnEnable()
        {
            harmony = new Harmony(hid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void OnDisable() => harmony.UnpatchAll(hid);

        [HarmonyPatch(typeof(FlickeringNeonSign))]
        [HarmonyPatch(nameof(FlickeringNeonSign.Awake), MethodType.Normal)]
        public class FlickeringNeonSignPatch
        {
            private static RandomObjectPicker<AudioClip> rop;

            public static void Postfix(ref RandomObjectPicker<AudioClip> ____sparksAudioClipPicker)
            {
                if(rop == null)
                {
                    AudioClip[] aca = { null };
                    rop = new RandomObjectPicker<AudioClip>(aca, 0.1f);
                }
                ____sparksAudioClipPicker = rop;
            }
        }
    }
}
