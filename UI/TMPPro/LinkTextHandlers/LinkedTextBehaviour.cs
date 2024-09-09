using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtils.PropertyAttributes;
using static UnityEngine.RectTransform;

namespace UnityUtils.UI.TMPPro
{
	[RequireComponent(typeof(TMP_Text)), ExecuteAlways]
	public class LinkedTextBehaviour : MonoBehaviour, IPointerClickHandler
	{
		[field: SerializeField]
		protected TMP_Text Label { get; private set; }

		[field: SerializeReference, Polymorphic]
		public ILinkHandler Handler { get; private set; }

		public string Text
		{
			get => Label.text;
			set => Label.text = value;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (Handler == null)
				return;

			int linkIndex = TMP_TextUtilities.FindIntersectingLink(Label, Input.mousePosition, null);
			if (linkIndex == -1)
				return;

			TMP_LinkInfo link = Label.textInfo.linkInfo[linkIndex];
			Handler.OnLinkClick(link);
		}

		public float CalculateSize(float size, Axis axis)
		{
			return axis switch
			{
				Axis.Horizontal => Label.GetPreferredValues(Text, 0, size).x,
				Axis.Vertical => Label.GetPreferredValues(Text, size, 0).y,
				_ => 0,
			};
		}
	}
}
