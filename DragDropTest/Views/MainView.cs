using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragDropTest
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();

            SetupDataModels();
            SetupGridViews();

            dataGridView1.MouseDown += (s, e) => dataGridView1_OnMouseDown(s, e);
            dataGridView1.DragOver += (s, e) => dataGridView_OnDragOver(s, e);
            dataGridView2.DragEnter += (s, e) => DataGridView2_OnDragEnter(s, e);
            dataGridView2.DragOver += (s, e) => dataGridView_OnDragOver(s, e);
            dataGridView2.DragDrop += (s, e) => DataGridVie2_OnDragDrop(s, e);

        }

        /// <summary>
        /// Handles the Mouse down event for a control
        /// </summary>
        /// <param name="sender">Control from which the event was fired</param>
        /// <param name="e">MouseEventArgs</param>
        private void dataGridView1_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dataGridView1.DoDragDrop(dataGridView1.SelectedRows, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Handles the Mouse drag over event for a control
        /// </summary>
        /// <param name="sender">Control from which the event was fired</param>
        /// <param name="e">MouseEventArgs</param>
        private void dataGridView_OnDragOver(object sender, DragEventArgs e)
        {
            if (e != null)
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Handles the Drag enter event for a control
        /// </summary>
        /// <param name="sender">Control from which the event was fired</param>
        /// <param name="e">MouseEventArgs</param>
        private void DataGridView2_OnDragEnter(object sender, DragEventArgs e)
        {
           e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Handles the Drag and drop event for a control
        /// </summary>
        /// <param name="sender">Control from which the event was fired</param>
        /// <param name="e">MouseEventArgs</param>
        private void DataGridVie2_OnDragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                var rowCollection = e.Data.GetData(typeof(DataGridViewSelectedRowCollection)) as DataGridViewSelectedRowCollection;
                if (rowCollection != null)
                {
                    if (_dataModel2 == null)
                    {
                        _dataModel2 = new List<DataModel>();
                    }

                    foreach (var row in rowCollection)
                    {
                        var rowData = (row as DataGridViewRow).DataBoundItem as DataModel;
                        if (rowData != null)
                        {
                            if (!_dataModel2.Contains(rowData))
                            {
                                _dataModel2.Add(rowData);
                            }
                        }
                    }

                    /// Rebind the updated data model to _dataGridView2
                    if (_dataModel2.Count > 0)
                    {
                        var bindingSource = new BindingSource();
                        bindingSource.DataSource = _dataModel2;
                        dataGridView2.DataSource = bindingSource;
                        bindingSource.ResetBindings(false);
                    }
                }
            }
        }

        /// <summary>
        /// Set up our two data models and fill them with dummy data
        /// </summary>
        private void SetupDataModels()
        {
            _dataModel1 = new List<DataModel>
            {
                new DataModel
                {
                    ID = 1,
                    Text = "Test text 1",
                },
                new DataModel
                {
                    ID = 2,
                    Text = "Test text 2",
                },
            };

            _dataModel2 = new List<DataModel>();
        }

        /// <summary>
        /// Set up and configure the two DataGridViews
        /// </summary>
        private void SetupGridViews()
        {
            // Setup dataGridView1
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = _dataModel1;
            dataGridView1.Columns["ID"].DataPropertyName = "ID";
            dataGridView1.Columns["Text"].DataPropertyName = "Text";
            dataGridView1.Columns["Text"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;

            // Setup dataGridView2
            dataGridView2.Columns.Clear();
            dataGridView2.DataSource = _dataModel2;
            dataGridView2.Columns["ID"].DataPropertyName = "ID";
            dataGridView2.Columns["Text"].DataPropertyName = "Text";
            dataGridView2.Columns["Text"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AllowUserToResizeRows = false;
        }

        private List<DataModel> _dataModel1;
        private List<DataModel> _dataModel2;
    }
}
