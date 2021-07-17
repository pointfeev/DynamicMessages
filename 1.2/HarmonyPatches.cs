using System.Collections.Generic;
using Verse;
using HarmonyLib;
using TacticalGroups;
using UnityEngine;

namespace DynamicMessages
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var harmony = new Harmony("pointfeev.dynamicmessages");

            harmony.Patch(
                original: AccessTools.Method(typeof(Messages), "MessagesDoGUI"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), "MessagesDoGUI")
            );
        }

        public static bool MessagesDoGUI(List<Message> ___liveMessages)
        {
            DynamicMessages.DoMessagesGUI(___liveMessages);
            return false;
        }
    }
}
