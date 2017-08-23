using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarioLevelMaker.source
{
    public partial class NewFileDialog : Form
    {
        public NewFileDialog()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            this.width = Convert.ToInt32(this.WidthBox.Value);
            this.height = Convert.ToInt32(this.HeightBox.Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public int width = 20;
        public int height = 12;
    }
}
