using System;
using System.Collections.Generic;
using Verse;
using UnityEngine;
using TacticalGroups;
using HarmonyLib;

namespace DynamicMessages
{
    public class DynamicMessages
    {
		public static void DoMessagesGUI(List<Message> liveMessages)
        {
			Text.Font = GameFont.Small;

			int xOffsetStandard = 12;
			int yOffsetStandard = 12;

			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int yOffset = (int)Messages.MessagesTopLeftStandard.y;

			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				yOffset += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}

			// Getting the largest X of all the messages, for determining whether to move messages downwards or not
			float largestRectX = xOffsetStandard;
			for (int i = liveMessages.Count - 1; i >= 0; i--)
			{
				Rect messageRect = liveMessages[i].CalculateRect(xOffset, yOffset);
				largestRectX = Math.Max(largestRectX, xOffsetStandard + messageRect.x + messageRect.width);
			}

			// Function that checks whether a rect should be used, and if so, uses it
			void checkRect(Rect rect)
            {
				if (largestRectX > rect.x)
                {
					yOffset = (int)Math.Max(yOffset, yOffsetStandard + rect.y + rect.height);
				}
			}

			// Pawn draw locs
			List<Rect> drawLocs = null;
			try { drawLocs = TacticUtils.TacticalColonistBar.DrawLocs; } catch { }
			if (drawLocs != null)
			{
				foreach (Rect rect in drawLocs)
				{
					checkRect(rect);
				}
			}

			// Colonist groups
			List<ColonistGroup> colonistGroups = null;
			try { colonistGroups = TacticUtils.AllGroups; } catch { }
			if (colonistGroups != null)
            {
				foreach (ColonistGroup colonistGroup in colonistGroups)
				{
					// Colonist group
					Rect curRect = colonistGroup.curRect;
					if (colonistGroup.isSubGroup)
                    {
						curRect.width /= 2f;
						curRect.height /= 2f;
					}
					checkRect(curRect);

					// Colonist group name
					if (!colonistGroup.isSubGroup && !colonistGroup.bannerModeEnabled && !colonistGroup.hideGroupIcon)
					{
						float groupNameHeight = Text.CalcHeight(colonistGroup.curGroupName, (float)colonistGroup.groupBanner.width);
						checkRect(new Rect(curRect.x, curRect.y + curRect.height, curRect.width, groupNameHeight));
					}

					// Colonist group pawn dots
					if (!colonistGroup.hidePawnDots)
                    {
						List<PawnDot> pawnDots = colonistGroup.GetPawnDots(curRect);
						if (pawnDots.Count > 0)
						{
							foreach (PawnDot pawnDot in pawnDots)
							{
								checkRect(pawnDot.rect);
							}
						}
					}

					// Colonist group pawn rows
					if (colonistGroup.pawnWindowIsActive || colonistGroup.showPawnIconsRightClickMenu || colonistGroup.ShowExpanded)
					{
						foreach (KeyValuePair<Pawn, Rect> pawnRect in colonistGroup.pawnRects)
						{
							checkRect(pawnRect.Value);
						}
					}
				}
			}

			// Colonist group options menu
			try
			{
				OptionsMenu menu = Find.WindowStack.WindowOfType<OptionsMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group right click menu
			try
			{
				MainFloatMenu menu = Find.WindowStack.WindowOfType<MainFloatMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > work] menu
			try
			{
				WorkMenu menu = Find.WindowStack.WindowOfType<WorkMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > orders] menu
			try
			{
				OrderMenu menu = Find.WindowStack.WindowOfType<OrderMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > manage] menu
			try
			{
				ManageMenu menu = Find.WindowStack.WindowOfType<ManageMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > manage] options slide menu
			try
			{
				OptionsSlideMenu menu = Find.WindowStack.WindowOfType<OptionsSlideMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > manage > icon] menu
			try
			{
				IconMenu menu = Find.WindowStack.WindowOfType<IconMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
			catch { }

            // Colonist group [right click > manage > sort] menu
            try
            {
				SortMenu menu = Find.WindowStack.WindowOfType<SortMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > manage > management] menu
			try
			{
				ManagementMenu menu = Find.WindowStack.WindowOfType<ManagementMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Colonist group [right click > manage > prisoner menu] menu
			try
			{
				PrisonerMenu menu = Find.WindowStack.WindowOfType<PrisonerMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
			catch { }

			// Colonist group [right click > manage > animal menu] menu
			try
			{
				AnimalMenu menu = Find.WindowStack.WindowOfType<AnimalMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
			catch { }

			// Colonist group [right click > manage > guest menu] menu
			try
			{
				GuestMenu menu = Find.WindowStack.WindowOfType<GuestMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
			catch { }

			// Colonist group [right click > manage > preset] menu
			try
			{
				PresetMenu menu = Find.WindowStack.WindowOfType<PresetMenu>();
				if (menu != null)
				{
					checkRect(menu.windowRect);
				}
			}
            catch { }

			// Display the messages like normal
			for (int i = liveMessages.Count - 1; i >= 0; i--)
			{
				liveMessages[i].Draw(xOffset, yOffset);
				yOffset += 26;
			}
		}
    }
}
