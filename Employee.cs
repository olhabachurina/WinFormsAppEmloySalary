using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAppEmloySalary
{
    public class Employee
    {
        public string FullName { get; }
        public int Hours { get; }
        public double HourlyRate { get; }
        public double Salary { get; }
        public decimal TaxedSalary { get; internal set; }

        public Employee(string fullName, int hours, double hourlyRate)
        {
            FullName = fullName;
            Hours = hours;
            HourlyRate = hourlyRate;
           
        }

          }
}

