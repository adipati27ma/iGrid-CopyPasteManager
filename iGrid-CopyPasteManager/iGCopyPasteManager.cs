using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

namespace iGrid_CopyPasteManager
{
	public class iGCopyPasteManager
	{
		#region Private fields

		private iGrid fGrid;
		private bool fCopyColumnHeaders = false;
		private System.Windows.Forms.ContextMenuStrip fContextMenu;
		private System.Windows.Forms.ToolStripMenuItem fMenuItemCut;
		private System.Windows.Forms.ToolStripMenuItem fMenuItemCopy;
		private System.Windows.Forms.ToolStripMenuItem fMenuItemPaste;

		#endregion

		#region Constructors

		public iGCopyPasteManager(iGrid grid) : this(grid, true)
		{
		}

		public iGCopyPasteManager(iGrid grid, bool extraScrollbarButtons) : this(grid, true, false)
		{
		}

		public iGCopyPasteManager(iGrid grid, bool extraScrollbarButtons, bool copyColumnHeaders)
		{
			fGrid = grid;
			fCopyColumnHeaders = copyColumnHeaders;

			// Grid events

			fGrid.CellMouseUp += new iGCellMouseUpEventHandler(fGrid_CellMouseUp);
			fGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(fGrid_KeyDown);

			// Copy/paste context menu

			fContextMenu = new System.Windows.Forms.ContextMenuStrip();
			fMenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
			fMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
			fMenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();

			fMenuItemCut.Text = "Cut";
			fMenuItemCut.Click += new System.EventHandler(fMenuItemCut_Click);

			fMenuItemCopy.Text = "Copy";
			fMenuItemCopy.Click += new System.EventHandler(fMenuItemCopy_Click);

			fMenuItemPaste.Text = "Paste";
			fMenuItemPaste.Click += new System.EventHandler(fMenuItemPaste_Click);

			fContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripMenuItem[] {
		fMenuItemCut, fMenuItemCopy, fMenuItemPaste});

			// Extra buttons on the vertical scroll bar

			if (extraScrollbarButtons)
			{
				// Add the "Select all"/"Deselect all" custom buttons
				fGrid.VScrollBar.CustomButtons.AddRange(new iGScrollBarCustomButton[] {
					new iGScrollBarCustomButton(iGScrollBarCustomButtonAlign.Far, iGActions.SelectAllCells, -1, "Select all", true, null),
					new iGScrollBarCustomButton(iGScrollBarCustomButtonAlign.Far, iGActions.DeselectAllCells, -1, "Deselect all", true, null)});
				// We need to make our extra scroll bar buttons always visible
				fGrid.VScrollBar.Visibility = iGScrollBarVisibility.Always;
			}
		}

		#endregion

		#region Public properties

		public bool CopyColumnHeaders
		{
			get { return fCopyColumnHeaders; }
			set { fCopyColumnHeaders = value; }
		}

		#endregion

		#region Main copy/paste functionality

		public bool CanCopyToClipboard()
		{
			// Check whether some cells are selected.
			if (fGrid.RowMode)
				return fGrid.SelectedRows.Count > 0;
			else
				return fGrid.SelectedCells.Count > 0;
		}

		public void CopyToClipboard()
		{
			CopyToClipboardInternal(false);
		}

		public void CutToClipboard()
		{
			CopyToClipboardInternal(true);
		}

		private void CopyToClipboardInternal(bool cutCells)
		{
			if (!CanCopyToClipboard())
				return;

			int myFirstSelectedColOrder, myLastSelectedColOrder;
			if (fGrid.RowMode)
			{
				myFirstSelectedColOrder = 0;
				myLastSelectedColOrder = fGrid.Cols.Count - 1;
			}
			else
			{
				GetFirstLastSelectedColOrder(out myFirstSelectedColOrder, out myLastSelectedColOrder);
			}

			// Create the string to pass to the clipboard.
			// This string represents a block of the cells.
			// Each row of cells is separated with the "\r\n" 
			// symbols (new line); each cell in a row is 
			// separated with the '\t' symbol (tabulation).
			// Each row should contain the same number of cells.

			StringBuilder myStringBuilder = new StringBuilder();

			if (fCopyColumnHeaders)
			{
				for (int myColOrder = myFirstSelectedColOrder; myColOrder <= myLastSelectedColOrder; myColOrder++)
				{
					iGCol myCol = fGrid.Cols.FromOrder(myColOrder);
					if (myCol.Visible || fGrid.SelectInvisibleCells)
					{
						myStringBuilder.Append(myCol.Text);
						if (myColOrder != myLastSelectedColOrder)
							myStringBuilder.Append('\t');
					}
				}
				myStringBuilder.Append("\r\n");
			}


			int myFirstSelectedRowIndex, myLastSelectedRowIndex;

			// The SelectedRows/SelectedCells collections are always sorted by RowIndex/ColIndex,
			// so we can easily to know the first/last selected row (or the row with selected cells):
			if (fGrid.RowMode)
			{
				myFirstSelectedRowIndex = fGrid.SelectedRows[0].Index;
				myLastSelectedRowIndex = fGrid.SelectedRows[fGrid.SelectedRows.Count - 1].Index;
			}
			else
			{
				myFirstSelectedRowIndex = fGrid.SelectedCells[0].RowIndex;
				myLastSelectedRowIndex = fGrid.SelectedCells[fGrid.SelectedCells.Count - 1].RowIndex;
			}

			for (int myRowIndex = myFirstSelectedRowIndex; myRowIndex <= myLastSelectedRowIndex; myRowIndex++)
			{
				if (fGrid.Rows[myRowIndex].Visible || fGrid.SelectInvisibleCells)
				{
					for (int myColOrder = myFirstSelectedColOrder; myColOrder <= myLastSelectedColOrder; myColOrder++)
					{
						iGCol myCol = fGrid.Cols.FromOrder(myColOrder);
						if (myCol.Visible || fGrid.SelectInvisibleCells)
						{
							iGCell myCell = fGrid.Cells[myRowIndex, myCol.Index];
							bool myIsCellSelected;
							if (fGrid.RowMode)
								myIsCellSelected = fGrid.SelectedRows.Contains(fGrid.Rows[myRowIndex]);
							else
								myIsCellSelected = myCell.Selected;
							if (myIsCellSelected)
							{
								myStringBuilder.Append(myCell.Text);
								if (cutCells)
									myCell.Value = null;
							}
							if (myColOrder != myLastSelectedColOrder)
								myStringBuilder.Append('\t');
						}
					}
					myStringBuilder.Append("\r\n");
				}
			}

			// Pass the string to the clipboard.
			Clipboard.SetDataObject(myStringBuilder.ToString(), true);
		}

		public bool CanPasteFromClipboard()
		{
			// Check whether the clipboard contains a compatible object.
			IDataObject myClipboardObject = Clipboard.GetDataObject();
			return myClipboardObject != null && myClipboardObject.GetDataPresent(typeof(string));
		}

		public void PasteFromClipboard()
		{
			if (!CanPasteFromClipboard())
				return;

			if (fGrid.Rows.Count == 0 || fGrid.Cols.Count == 0)
				return;

			// Determine the start point to paste the data.
			int myStartRowIndex, myStartColOrder;
			if (fGrid.CurCell != null)
			{
				myStartRowIndex = fGrid.CurCell.RowIndex;
				myStartColOrder = fGrid.CurCell.Col.Order;
			}
			else
			{
				myStartRowIndex = 0;
				myStartColOrder = 0;
			}

			// Get the string to paste from the clipboard.
			IDataObject myClipboardObject = Clipboard.GetDataObject();
			string myString = myClipboardObject.GetData(typeof(string)) as string;

			if (fCopyColumnHeaders)
				myString = myString.Substring(myString.IndexOf("\r\n") + 2);

			// Deselect all the selected cells/rows.
			if (fGrid.RowMode)
				fGrid.PerformAction(iGActions.DeselectAllRows);
			else
				fGrid.PerformAction(iGActions.DeselectAllCells);

			// Fill cells from the clipboard string.
			string[][] myCells = GetCellsFromClipboardString(myString);
			int myEndRowIndex = -1, myEndColOrder = -1;
			if (myCells != null)
			{
				for (int myRowIndex = myStartRowIndex; myRowIndex < fGrid.Rows.Count && myRowIndex - myStartRowIndex < myCells.Length; myRowIndex++)
				{
					myEndRowIndex = myRowIndex;
					if (fGrid.RowMode)
						fGrid.Rows[myRowIndex].Selected = true;
					for (int myColOrder = myStartColOrder; myColOrder < fGrid.Cols.Count && myColOrder - myStartColOrder < myCells[myRowIndex - myStartRowIndex].Length; myColOrder++)
					{
						myEndColOrder = myColOrder;
						iGCell myCell = fGrid.Cells[myRowIndex, fGrid.Cols.FromOrder(myColOrder).Index];
						myCell.Value = myCells[myRowIndex - myStartRowIndex][myColOrder - myStartColOrder];
						if (!fGrid.RowMode)
							myCell.Selected = true;
					}
				}
			}
			else
				fGrid.Cells[myStartRowIndex, fGrid.Cols.FromOrder(myStartColOrder).Index].Value = myString;

			// Trying to display the inserted block of selected cells/rows as much as possible.
			if (fGrid.RowMode)
			{
				if (myEndRowIndex >= 0)
					fGrid.Rows[myEndRowIndex].EnsureVisible();
				fGrid.Rows[myStartRowIndex].EnsureVisible();
			}
			else
			{
				if (myEndRowIndex >= 0 && myEndColOrder >= 0)
					fGrid.Cells[myEndRowIndex, fGrid.Cols.FromOrder(myEndColOrder).Index].EnsureVisible();
				fGrid.Cells[myStartRowIndex, fGrid.Cols.FromOrder(myStartColOrder).Index].EnsureVisible();
			}
		}

		/// <summary>
		/// Retrieve the cell values from the specified clipboard string.
		/// </summary>
		private string[][] GetCellsFromClipboardString(string value)
		{
			// Get the rows.
			string[] myRows = value.Split(new char[] { '\n' });

			// Remove '\t' characters from the end of the rows.
			for (int myIndex = 0; myIndex < myRows.Length; myIndex++)
			{
				if (myRows[myIndex].Length > 0)
				{
					if (myRows[myIndex][myRows[myIndex].Length - 1] == '\r')
						myRows[myIndex] = myRows[myIndex].Substring(0, myRows[myIndex].Length - 1);
				}
			}

			// If the last row is empty, remove it.
			int myRowCount = myRows.Length;
			if (myRows[myRows.Length - 1].Length == 0)
				myRowCount--;

			// Retrieve the cell values from each row.
			string[][] myCells = new string[myRowCount][];
			int myColCount = -1;
			for (int myIndex = 0; myIndex < myRowCount; myIndex++)
			{
				if (myRows[myIndex].Length == 0)
				{
					// If the row is empty, fill up the cells with empty values.

					// If there were no rows before an empty one,
					// return nothing (because it is abnormal case).
					if (myColCount < 0)
						return null;

					myCells[myIndex] = new string[myColCount];
					for (int myColIndex = 0; myColIndex < myColCount; myColIndex++)
						myCells[myIndex][myColIndex] = string.Empty;
				}
				else
					myCells[myIndex] = myRows[myIndex].Split('\t');

				// Set up the column count and check whether all the rows
				// have the same number of cell.
				if (myColCount >= 0)
				{
					if (myCells[myIndex].Length != myColCount)
						return null;
				}
				else
					myColCount = myCells[myIndex].Length;
			}

			return myCells;
		}

		/// <summary>
		/// Returns the first and last column in the range of selected cells.
		/// </summary>
		private void GetFirstLastSelectedColOrder(out int firstSelectedColOrder, out int lastSelectedColOrder)
		{
			firstSelectedColOrder = int.MaxValue;
			lastSelectedColOrder = int.MinValue;

			foreach (iGCell myCell in fGrid.SelectedCells)
			{
				if (myCell.Col.Order > lastSelectedColOrder)
					lastSelectedColOrder = myCell.Col.Order;

				if (myCell.Col.Order < firstSelectedColOrder)
					firstSelectedColOrder = myCell.Col.Order;
			}
		}

		#endregion

		#region Context menu

		private void fMenuItemCut_Click(object sender, System.EventArgs e)
		{
			CutToClipboard();
		}

		private void fMenuItemCopy_Click(object sender, System.EventArgs e)
		{
			CopyToClipboard();
		}

		private void fMenuItemPaste_Click(object sender, System.EventArgs e)
		{
			PasteFromClipboard();
		}

		#endregion

		#region Grid events

		private void fGrid_CellMouseUp(object sender, iGCellMouseUpEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (fGrid.RowMode)
				{
					if (!fGrid.Rows[e.RowIndex].Selected)
					{
						fGrid.PerformAction(iGActions.DeselectAllRows);
						fGrid.Rows[e.RowIndex].Selected = true;
						fGrid.SetCurCell(e.RowIndex, e.ColIndex);
					}
				}
				else
				{
					if (!fGrid.Cells[e.RowIndex, e.ColIndex].Selected)
					{
						fGrid.PerformAction(iGActions.DeselectAllCells);
						fGrid.Cells[e.RowIndex, e.ColIndex].Selected = true;
						fGrid.SetCurCell(e.RowIndex, e.ColIndex);
					}
				}

				bool myCanCopy = CanCopyToClipboard();
				bool myCanPaste = CanPasteFromClipboard();

				if (!myCanCopy && !myCanPaste)
					return;

				fMenuItemCut.Enabled = myCanCopy;
				fMenuItemCopy.Enabled = myCanCopy;
				fMenuItemPaste.Enabled = myCanPaste;

				fContextMenu.Show(fGrid, e.MousePos);
			}
		}

		private void fGrid_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Control && e.KeyCode == Keys.X)
			{
				CutToClipboard();
			}
			else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
			{
				CopyToClipboard();
			}
			else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
			{
				PasteFromClipboard();
			}
			else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
			{
				fGrid.PerformAction(iGActions.SelectAllCells);
			}
			else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.D)
			{
				fGrid.PerformAction(iGActions.DeselectAllCells);
			}
		}

		#endregion
	}
}
