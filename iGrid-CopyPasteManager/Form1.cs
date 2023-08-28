using TenTec.Windows.iGridLib;

namespace iGrid_CopyPasteManager;

public partial class Form1 : Form
{
    private iGCopyPasteManager fCopyPasteManager;

    public Form1()
    {
        InitializeComponent();
    }

    private void Form2_Load(object sender, EventArgs e)
    {
        fGrid.ReadOnly = true;

        // Allow multiple cells to be selected.
        fGrid.SelectionMode = iGSelectionMode.MultiExtended;

        // Add rows and columns.
        fGrid.Cols.AddRange(10);
        fGrid.Rows.AddRange(25);

        // Column header texts for test
        foreach (iGCol myCol in fGrid.Cols)
            myCol.Text = String.Format("Col{0}", myCol.Index);

        // Populate the cells with random values.
        Random myRandom = new Random();
        foreach (iGCell myCell in fGrid.Cells)
            myCell.Value = String.Format("R{0}C{1}", myCell.RowIndex, myCell.ColIndex);

        // Create an instance of the CopyPasteManager
        // and automatically attach it to the grid.
        fCopyPasteManager = new iGCopyPasteManager(fGrid);
        // Other options can be:
        // 1) Hide the extra scroll bar buttons
        //fCopyPasteManager = new iGCopyPasteManager(fGrd, false);
        // 2) Show the extra scroll bar buttons and copy the column headers
        //fCopyPasteManager = new iGCopyPasteManager(fGrd, true, true);
    }

    #region Interface controls to show some features

    private void fButtonCopy_Click(object sender, EventArgs e)
    {
        if (!fCopyPasteManager.CanCopyToClipboard())
        {
            MessageBox.Show(this, "No cells are selected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        fCopyPasteManager.CopyToClipboard();
    }

    private void fButtonPaste_Click(object sender, EventArgs e)
    {
        if (!fCopyPasteManager.CanPasteFromClipboard())
        {
            MessageBox.Show(this, "The clipboard does not contain a compatible object.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        fCopyPasteManager.PasteFromClipboard();
    }

    private void fCheckBoxCopyColumnHeaders_CheckedChanged(object sender, EventArgs e)
    {
        fCopyPasteManager.CopyColumnHeaders = fCheckBoxCopyColumnHeaders.Checked;
    }

    #endregion
}