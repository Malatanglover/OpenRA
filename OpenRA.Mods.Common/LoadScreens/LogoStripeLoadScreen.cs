#region Copyright & License Information
/*
 * Copyright 2007-2021 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using System.Collections.Generic;
using OpenRA.Graphics;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Primitives;

namespace OpenRA.Mods.Common.LoadScreens
{
	public sealed class LogoStripeLoadScreen : SheetLoadScreen
	{
#if !DEBUG_ON
		Rectangle bgRect;
		Sprite background;
#else
		Rectangle stripeRect;
		float2 logoPos;
		Sprite stripe, logo;
#endif
		Sheet lastSheet;
		int lastDensity;
		Size lastResolution;

		string[] messages = { "Loading..." };

		public override void Init(ModData modData, Dictionary<string, string> info)
		{
			base.Init(modData, info);

			if (info.ContainsKey("Text"))
				messages = info["Text"].Split(',');
		}

		public override void DisplayInner(Renderer r, Sheet s, int density)
		{
			if (s != lastSheet || density != lastDensity)
			{
				lastSheet = s;
				lastDensity = density;
#if !DEBUG_ON
				background = CreateSprite(s, new Rectangle(0, 0, s.Size.Width, s.Size.Height), r.Resolution);
#else
				logo = CreateSprite(s, density, new Rectangle(0, 0, 256, 256));
				stripe = CreateSprite(s, density, new Rectangle(258, 0, 253, 256));
#endif
			}

			if (r.Resolution != lastResolution)
			{
				lastResolution = r.Resolution;
#if !DEBUG_ON
				bgRect = new Rectangle(0, 0, lastResolution.Width, lastResolution.Height);
#else
				stripeRect = new Rectangle(0, lastResolution.Height / 2 - 128, lastResolution.Width, 256);
				logoPos = new float2(lastResolution.Width / 2 - 128, lastResolution.Height / 2 - 128);
#endif
			}
#if !DEBUG_ON
			if (background != null)
				WidgetUtils.FillRectWithSprite(bgRect, background);
#else
			if (stripe != null)
				WidgetUtils.FillRectWithSprite(stripeRect, stripe);

			if (logo != null)
				r.RgbaSpriteRenderer.DrawSprite(logo, logoPos);
#endif
			if (r.Fonts != null)
			{
				var text = messages.Random(Game.CosmeticRandom);
				var textSize = r.Fonts["Bold"].Measure(text);
				r.Fonts["Bold"].DrawText(text, new float2(r.Resolution.Width - textSize.X - 20, r.Resolution.Height - textSize.Y - 20), Color.White);
			}
		}
	}
}
