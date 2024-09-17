using TestTaskForWork.Context;

namespace TestTaskForWork
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool connected = false;

            while (!connected)
            {
                try
                {
                    using (var context = new AssemblyContext())
                    {
                        // ������� ��������� ������ � ���� ������
                        context.Database.CanConnect();
                        connected = true;
                    }
                }
                catch
                {
                    // ���� ����������� ����������, ���������� ����� ��� ����� ������
                    using (var settingsForm = new ConnectionSettingsForm())
                    {
                        if (settingsForm.ShowDialog() == DialogResult.OK)
                        {
                            var connectionString = settingsForm.ConnectionString;
                            // ��������� ������ ����������� � ��������� ������
                            AssemblyContext.UpdateConnectionString(connectionString);
                        }
                        else
                        {
                            // ���� ������������ �������, ��������� ����������
                            Application.Exit();
                            return;
                        }
                    }
                }
            }

            Application.Run(new Form1());
        }
    }
}