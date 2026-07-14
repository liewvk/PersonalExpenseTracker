using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace PersonalExpenseTracker
{
    public partial class Form1 : Form
    {
        private DataTable expensesTable = new DataTable();
        private void UpdateTotal()
        {
            decimal total = 0;

            foreach (DataRow row in expensesTable.Rows)
            {
                total += Convert.ToDecimal(row["Amount"]);
            }

            lblTotal.Text = $"$ {total:0.00}";
        }
        private void ClearInputFields()
        {
            dtpDate.Value = DateTime.Today;
            cmbCategory.SelectedIndex = -1;
            txtDescription.Clear();
            txtAmount.Clear();

            txtDescription.Focus();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.Add("Food");
            cmbCategory.Items.Add("Transport");
            cmbCategory.Items.Add("Shopping");
            cmbCategory.Items.Add("Utilities");
            cmbCategory.Items.Add("Education");
            cmbCategory.Items.Add("Entertainment");
            cmbCategory.Items.Add("Medical");
            cmbCategory.Items.Add("Others");

            cmbCategory.SelectedIndex = -1;

            expensesTable.Columns.Add("Date", typeof(string));
            expensesTable.Columns.Add("Category", typeof(string));
            expensesTable.Columns.Add("Description", typeof(string));
            expensesTable.Columns.Add("Amount", typeof(decimal));

            dgvExpenses.DataSource = expensesTable;

            dgvExpenses.Columns["Amount"].DefaultCellStyle.Format = "0.00";

            dtpDate.Value = DateTime.Today;
            txtDescription.Focus();

            UpdateTotal();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string category = "";
            string description = txtDescription.Text.Trim();
            string amountText = txtAmount.Text.Trim();

            if (cmbCategory.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a category.",
                                "Missing Category",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return;
            }

            category = cmbCategory.SelectedItem.ToString();

            if (description == "")
            {
                MessageBox.Show("Please enter a description.",
                                "Missing Description",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtDescription.Focus();
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount))
            {
                MessageBox.Show("Please enter a valid amount.",
                                "Invalid Amount",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtAmount.Focus();
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Amount must be greater than zero.",
                                "Invalid Amount",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                txtAmount.Focus();
                return;
            }

            string date = dtpDate.Value.ToShortDateString();

            expensesTable.Rows.Add(date, category, description, amount);

            UpdateTotal();
            ClearInputFields();

            MessageBox.Show("Expense added successfully.",
                            "Expense Added",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

        }

        private void btnClearInput_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvExpenses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an expense to delete.",
                                "No Expense Selected",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected expense?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int rowIndex = dgvExpenses.SelectedRows[0].Index;

                dgvExpenses.Rows.RemoveAt(rowIndex);

                UpdateTotal();

                MessageBox.Show("Expense deleted successfully.",
                                "Expense Deleted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (expensesTable.Rows.Count == 0)
            {
                MessageBox.Show("There are no expenses to clear.",
                                "Empty List",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to clear all expenses?",
                                                  "Confirm Clear All",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                expensesTable.Rows.Clear();

                UpdateTotal();
                ClearInputFields();

                MessageBox.Show("All expenses have been cleared.",
                                "Expenses Cleared",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?",
                                      "Confirm Exit",
                                      MessageBoxButtons.YesNo,
                                      MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }
    }
}
