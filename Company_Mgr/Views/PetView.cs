using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Company_Mgr.Views
{
    public partial class PetView : Form, IPetView
    {
        private static PetView instance;
        public static PetView Instance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new PetView();
                instance.MdiParent = parentContainer;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized) instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
        private string message;
        private bool isSuccessful;
        private bool isEdit;

        private PetView()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
            //先隐藏第二个标签选项  因为只有在添加宠物时才会显示这个
            tabControl1.TabPages.Remove(tabPage2);
            btnClose.Click += (e, s) =>
            {
                this.Close();
            };
        }

        private void AssociateAndRaiseViewEvents()
        {
            btnSearch.Click += delegate { SearchEvent?.Invoke(this, EventArgs.Empty); };
            txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) SearchEvent?.Invoke(this, EventArgs.Empty);
            };

            btnAdd.Click += delegate
            {
                AddNewEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Add(tabPage2);
                tabPage2.Text = "添加新宠物";
            };
            btnEdit.Click += delegate
            {
                EditEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Add(tabPage2);
                tabPage2.Text = "编辑宠物";
            };
            btnDel.Click += delegate 
            { 
                var result = MessageBox.Show("你确定要删除选中项吗？","警告",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    DeleteEvent?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show(Message);
                }
            };
            btnSave.Click += delegate 
            {
                SaveEvent?.Invoke(this, EventArgs.Empty); 
                if(isSuccessful)
                {
                    tabControl1.TabPages.Remove(tabPage2);
                    tabControl1.TabPages.Add(tabPage1);
                }
                MessageBox.Show(Message);
            };
            btnCancel.Click += delegate 
            {
                CancelEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Add(tabPage1);
            };
        }

        public string PetID { get => txtPetID.Text; set => txtPetID.Text = value; }
        public string PetName { get => txtPetName.Text; set => txtPetName.Text = value; }
        public string PetType { get => txtPetType.Text; set => txtPetType.Text = value; }
        public string PetColor { get => txtPetColor.Text; set => txtPetColor.Text = value; }
        public string SearchValue { get => txtSearch.Text; set => txtSearch.Text = value; }
        public bool IsEdit { get => isEdit; set => isEdit = value; }
        public bool IsSuccessful { get => isSuccessful; set => isSuccessful = value; }
        public string Message { get => message; set => message = value; }

        public event EventHandler SearchEvent;
        public event EventHandler AddNewEvent;
        public event EventHandler EditEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SaveEvent;
        public event EventHandler CancelEvent;

        public void SetPetListBindingSource(BindingSource petList)
        {
            dataGridView1.DataSource = petList;
        }
    }
}
