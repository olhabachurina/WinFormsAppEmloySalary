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
                new Employee("Авдеенко Виталий Петрович", 160, 100.0),
                new Employee("Бабевский Аркадий Тимурович ", 140, 120.0),
                new Employee("Витенко Владимир Владимирович", 150, 110.0),
                new Employee("Григоренко Олег Олегович ", 190, 130.0),
                new Employee("Дмитрук Ирина Васильевна", 144, 125.0),
                new Employee("Емельянов Виктор Викторович",100,115.0),

            };
            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("Часы", typeof(int));
            dataTable.Columns.Add("Тариф", typeof(double));
            dataTable.Columns.Add("Сверхурочно", typeof(int));
            dataTable.Columns.Add("Заработная плата", typeof(double));
            dataTable.Columns.Add("Подоходный налог", typeof(double));
            dataTable.Columns.Add("К выплате", typeof(double));


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
            totalRow["ФИО"] = "Итог";
            totalRow["Часы"] = employees.Sum(e => e.Hours);
            totalRow["Тариф"] = 0; // Вы можете установить значение тарифа по умолчанию или оставить пустым
            totalRow["Сверхурочно"] = employees.Sum(e => Math.Max(e.Hours - 144, 0));
            totalRow["Заработная плата"] = totalSalary;
            totalRow["Подоходный налог"] = totalTaxedSalary;
            totalRow["К выплате"] = totalPayment;
            dataTable.Rows.Add(totalRow);
            totalRow["Заработная плата"] = dataTable.Compute("SUM([Заработная плата])", string.Empty);
            totalRow["Подоходный налог"] = dataTable.Compute("SUM([Подоходный налог])", string.Empty);
            totalRow["К выплате"] = dataTable.Compute("SUM([К выплате])", string.Empty);
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