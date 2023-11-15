using System.Data;

namespace WinFormsAppEmloySalary
{
    public partial class Form1 : Form
    {
        private List<Employee> employees;
        public Form1()
        {
            InitializeComponent();
            employees = new List<Employee>
            {
                new Employee("�������� ������� ��������", 160, 100.0),
                new Employee("��������� ������� ��������� ", 140, 120.0),
                new Employee("������� �������� ������������", 150, 110.0),
                new Employee("���������� ���� �������� ", 190, 130.0),
                new Employee("������� ����� ����������", 144, 125.0),
                new Employee("��������� ������ ����������",100,115.0),

            };
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("���", typeof(string));
            dataTable.Columns.Add("����", typeof(int));
            dataTable.Columns.Add("�����", typeof(double));
            dataTable.Columns.Add("�����������", typeof(int));
            dataTable.Columns.Add("���������� �����", typeof(double));
            dataTable.Columns.Add("���������� �����", typeof(double));
            dataTable.Columns.Add("� �������", typeof(double));


            foreach (Employee employee in employees)
            {
                int overtimeHours = Math.Max(employee.Hours - 144, 0);
                double salary = CalculateSalary(employee.Hours, employee.HourlyRate);
                double taxedSalary = CalculateTaxedSalary(salary);

                dataTable.Rows.Add(
                    employee.FullName,
                    employee.Hours,
                    employee.HourlyRate,
                    overtimeHours,
                    salary,
                    taxedSalary,
                    salary - taxedSalary

                );

            }
            decimal totalSalary = employees.Sum(e => (decimal)e.Salary);
            decimal totalTaxedSalary = employees.Sum(e => (decimal)e.TaxedSalary);
            decimal totalPayment = totalSalary - totalTaxedSalary;

            dataGridView1.DataSource = dataTable;
            DataRow totalRow = dataTable.NewRow();
            totalRow["���"] = "����";
            totalRow["����"] = employees.Sum(e => e.Hours);
            totalRow["�����"] = 0; // �� ������ ���������� �������� ������ �� ��������� ��� �������� ������
            totalRow["�����������"] = employees.Sum(e => Math.Max(e.Hours - 144, 0));
            totalRow["���������� �����"] = totalSalary;
            totalRow["���������� �����"] = totalTaxedSalary;
            totalRow["� �������"] = totalPayment;
            dataTable.Rows.Add(totalRow);
            totalRow["���������� �����"] = dataTable.Compute("SUM([���������� �����])", string.Empty);
            totalRow["���������� �����"] = dataTable.Compute("SUM([���������� �����])", string.Empty);
            totalRow["� �������"] = dataTable.Compute("SUM([� �������])", string.Empty);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.DataSource = dataTable;
        }

        private double CalculateSalary(int hours, double hourlyRate)
        {
            int regularHours = Math.Min(hours, 144);
            int overtimeHours = Math.Max(hours - 144, 0);

            double salary = (regularHours * hourlyRate) + (overtimeHours * hourlyRate * 2);
            return salary;
        }

        private double CalculateTaxedSalary(double salary)
        {
            const double taxRate = 12.0;
            double taxedSalary = salary * (taxRate / 100);
            return taxedSalary;
        }
    }

}