using UnityEngine;
using SystemColor = System.Drawing.Color;

namespace UnityUtils.UI.Colors
{
	public static class ColorUtils
	{
		private const float size = 255f;

		public static Color Saturate(this Color color, float saturation)
		{
			float avg = (color.r + color.g + color.b) / 3;
			float _Saturate(float value) => avg + (value - avg) * saturation;
			return new Color(_Saturate(color.r), _Saturate(color.g), _Saturate(color.b));
		}
		public static Color Darken(this Color color, float darken)
		{
			return Color.Lerp(color, Color.black, darken);
		}

		public static Color SetAlpha(this Color color, float a)
		{
			return new Color(color.r, color.g, color.b, a);
		}

		public static Color ToUnityColor(this SystemColor color)
		{
			return new Color(color.R / size, color.G / size, color.B / size, color.A / size);
		}
		public static SystemColor ToSystemColor(this Color color)
		{
			static int ToInt(float f)
			{
				return (int)(f * size);
			}

			return SystemColor.FromArgb(ToInt(color.a), ToInt(color.r), ToInt(color.g), ToInt(color.b));
		}
	}
}
