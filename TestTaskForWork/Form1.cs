using Microsoft.EntityFrameworkCore;
using TestTaskForWork.Context;
using TestTaskForWork.Modules;

namespace TestTaskForWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int _lastOrderId;
        private int _currentOrderId;
        private void AddLabelToPanel(string labelText)
        {
            Label lastLabel = null;
            if (panel1.Controls.Count > 0)
            {
                lastLabel = panel1.Controls[panel1.Controls.Count - 1] as Label;
            }

            Label newLabel = new Label();
            newLabel.Text = labelText;
            newLabel.ForeColor = Color.Black;
            newLabel.AutoSize = true;

            // ������������� ����������
            if (lastLabel != null)
            {
                // ���� ���� ���������� �����, ���������� ����� ���� �� � ��������
                newLabel.Location = new Point(lastLabel.Location.X, lastLabel.Location.Y + lastLabel.Height + 10);
            }
            else
            {
                // ���� ����� ��� ���, ������� ������ ����� � ������� ����� ������
                newLabel.Location = new Point(10, 10);
            }

            panel1.Controls.Add(newLabel);
        }

        private void tbQuantity_KeyPress(object sender, KeyPressEventArgs e) // ������������ ������� ����
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            using (var context = new AssemblyContext())
            {
                
                var orders = context.order.ToList();
                foreach (var n in orders)
                {
                    _lastOrderId = n.id;
                }
                var dtime = DateTime.Now;
                context.Database.ExecuteSqlRaw("CALL AddOrder({0})", dtime);
                AddLabelToPanel($"����� � {_lastOrderId} �� {dtime} �������.");
            }
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {

            if (_currentOrderId <= 0)
            {
                MessageBox.Show("������� �������� �����.");
                return;
            }

            using (var context = new AssemblyContext())
            {

                var order = context.order.Find(_currentOrderId);
                if (order != null)
                {
                    if (order.iscancelled)
                    {
                        MessageBox.Show("����� ��� �������.");
                        return;
                    }

                    // ������������� ������ ������
                    order.iscancelled = true;
                    context.SaveChanges();

                    string cancellationMessage = $"����� � {order.id} ��� ������� �������.";
                    AddLabelToPanel(cancellationMessage);
                }
                else
                {
                    MessageBox.Show("����� �� ������.");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var context = new AssemblyContext())
            {
                // �������� ��� ������
                var orders = context.order.ToList();
                // ������� ���������� � �������
                foreach (var order in orders)
                {
                    string canceled = "";
                    if (order.iscancelled == true) canceled = "(�������)";
                    string orderStr = $"����� � {order.id} �� {order.orderdate} {canceled}";
                    AddLabelToPanel(orderStr);
                    _currentOrderId = order.id;
                }

                var nomenclature = context.nomenclature.ToList();
                foreach (var n  in nomenclature)
                {
                    comboBox1.Items.Add(n.name);
                }
            }
        }
    }
}
