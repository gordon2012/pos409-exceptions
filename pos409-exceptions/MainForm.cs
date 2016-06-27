// Exception Handling
// Gordon Doskas
// POS/409
// June 27, 2016
// Carole Mckinney

// Program Description
// ===================
// This program is a Budget Calculator. It allows the user to input a
// decimal number as a budget and multiple decimal numbers for expenses.
// It validates the input by use of Exception Handling, not allowing
// non-number or negative number input. Once it at least has a valid
// number for the budget, it calculates the remaining funds by subtracting
// each expense from the budget and displays it in the indicated label.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos409_exceptions
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            lblBudget.Text = "";
            lblRemaining.Text = "";
        }

        // Method that parses the input of the provided control, ensuring
        // that it is a valid number, returning a currency formatted string
        //
        private string ParseInput(Control control)
        {
            // Resets the error provider
            //
            errorProvider1.Clear();

            string output = "";

            try
            {
                double num = double.Parse(control.Text);

                // Throw custom exception if number is negative
                //
                if(num < 0)
                {
                    throw new NegativeException("Negative numbers not permitted");
                }
                // Convert string to currency
                //
                output = num.ToString("C");
                
                // Delete input string on success
                //
                control.Text = "";
            }
            catch(FormatException)
            {
                errorProvider1.SetError(control, "Not a valid number");
            }
            // Default catch block, will catch the NegativeException and
            // any others not thought of
            //
            catch(Exception ex)
            {
                errorProvider1.SetError(control, ex.Message);
            }

            return output;
        }

        // Method that recalculates the remaining amount. A better way might
        // be to store the numeric value instead of string so this much conversion
        // does not need to take place, but it is only called by click events, so
        // efficiency is not an issue
        //
        private void CalculateRemaining()
        {
            if (!lblBudget.Text.Equals(""))
            {
                // Converts the budget currency string back into a number
                //
                double remaining = double.Parse(lblBudget.Text, System.Globalization.NumberStyles.Currency);

                foreach (string expense in lstExpenses.Items)
                {   
                    // Converts each expense back into numbers and subtracts them from the budget
                    //
                    remaining -= double.Parse(expense, System.Globalization.NumberStyles.Currency);
                }

                // Converts it yet again into a currency string
                //
                lblRemaining.Text = remaining.ToString("C");
            }
        }



        private void btnSetBudget_Click(object sender, EventArgs e)
        {
            string output = ParseInput(txtInput);

            if (!output.Equals(""))
            {
                lblBudget.Text = output;
            }
            CalculateRemaining();
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            string output = ParseInput(txtExpense);

            if (!output.Equals(""))
            {
                lstExpenses.Items.Add(output);
            }
            CalculateRemaining();
        }
    }

    // Custom Exception class thrown if an input is a negative number
    //
    class NegativeException : Exception
    {
        public NegativeException(string message)
            : base(message) { }
    }

}
