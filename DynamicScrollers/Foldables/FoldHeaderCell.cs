using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.DynamicScrollers.Foldables
{
	public class FoldHeaderCell : FoldableCell
	{
		[SerializeField] private TMP_Text headerText;
		[SerializeField] private Button button;
		[SerializeField] private Image toggleIcon;

		public Button.ButtonClickedEvent OnClick => button.onClick;
		
		public override Task<bool> SetData(IScrollerCellData data)
		{
			if (data is IHeaderData headerData)
			{
				headerText.text = headerData.HeaderText;    
			}
            
			return base.SetData(data);
		}

		public override void Clear()
		{
			headerText.text = null;
			base.Clear();
		}

		public override void SetFolded(bool isFolded)
		{
			base.SetFolded(isFolded);
			if (toggleIcon)
				toggleIcon.transform.localScale = new Vector3(1, IsFolded ? 1 : -1, 1);
		}
	}
}