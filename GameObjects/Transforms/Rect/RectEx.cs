using UnityEngine;

namespace UnityUtils.RectUtils
{
	public static class RectEx
	{
		public static Rect SetSize(this Rect rect, float width, float height) => rect.SetSize(new Vector2(width, height));
		public static Rect SetSize(this Rect rect, Vector2 size)
		{
			rect.Set(rect.position.x, rect.position.y, size.x, size.y);
			return rect;
		}
		public static Rect SetWidth(this Rect rect, float width) => rect.SetSize(new Vector2(width, rect.size.y));
		public static Rect SetRemainderWidth(this Rect rect, float maxWidth) => rect.SetWidth(maxWidth - rect.position.x);
		public static Rect SetHeight(this Rect rect, float height) => rect.SetSize(new Vector2(rect.size.x, height));
		public static Rect AddSize(this Rect rect, Vector2 size) => rect.SetSize(rect.size + size);
		public static Rect AddWidth(this Rect rect, float width) => rect.AddSize(new Vector2(width, 0));
		public static Rect AddHeight(this Rect rect, float height) => rect.AddSize(new Vector2(0, height));

		public static Rect SetPosition(this Rect rect, Vector2 position)
		{
			return new Rect(position.x, position.y, rect.size.x, rect.size.y);
		}
		public static Rect SetX(this Rect rect, float x) => rect.SetPosition(new Vector2(x, rect.position.y));
		public static Rect SetY(this Rect rect, float y) => rect.SetPosition(new Vector2(rect.position.x, y));
		public static Rect Move(this Rect rect, Vector2 position) => rect.SetPosition(rect.position + position);
		public static Rect Move(this Rect rect, float x, float y) => rect.SetPosition(rect.position + new Vector2(x, y));
		public static Rect MoveX(this Rect rect, float x) => rect.Move(new Vector2(x, 0));
		public static Rect MoveY(this Rect rect, float y) => rect.Move(new Vector2(0, y));
	}
}
