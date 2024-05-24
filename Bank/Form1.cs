using Microsoft.VisualBasic.Logging;
using System.Data.SQLite;
using System.Diagnostics;

namespace Bank
{
    public partial class Form1 : Form
    {
        string connectionPath = @"Data Source=\\\OSDEEQAB097723\db\bank.db;Version=3;";
        public Form1()
        {
            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            InitializeComponent();
        }

        //Login
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Allow Numbers In Pincode
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Allow Numbers In AccountNumber
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public async void button1_Click(object sender, EventArgs e)
        {
            string accountNumber = textBox1.Text;
            string pinCode = textBox2.Text;

            //Check for Entered First Name And Pincode
            string query = "SELECT AccountNumber, Balance, FirstName, LastName FROM Users WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode;";
            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(query, connection);
            connection.Open();

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string aNumber = reader["AccountNumber"].ToString();
                    double balance = Convert.ToDouble(reader["Balance"]);
                    string fname = reader["FirstName"].ToString();
                    string lname = reader["LastName"].ToString();

                    //Hide Login Page
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label6.Visible = false;
                    button1.Visible = false;

                    //Show Main Page
                    ShowMain();

                    //Update Name and Balance
                    label1.Text = balance.ToString() + "$";
                    label12.Text = fname.ToString() + " " + lname.ToString();
                }
                else
                {
                    //Show Failed To Login Message
                    label5.Visible = true;
                    await Task.Delay(2000);
                    label5.Visible = false;
                }
            }
        }

        public async void UpdateBalance()
        {
            string accountNumber = textBox1.Text;
            string pinCode = textBox2.Text;

            //Check for Entered First Name And Pincode
            string query = "SELECT AccountNumber, Balance, FirstName, LastName FROM Users WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode;";
            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(query, connection);
            connection.Open();

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string aNumber = reader["AccountNumber"].ToString();
                    double balance = Convert.ToDouble(reader["Balance"]);
                    string fname = reader["FirstName"].ToString();
                    string lname = reader["LastName"].ToString();

                    //Hide Login Page
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label6.Visible = false;
                    button1.Visible = false;

                    //Update Name and Balance
                    label1.Text = balance.ToString() + "$";
                    label12.Text = fname.ToString() + " " + lname.ToString();
                }
                else
                {
                    //Show Failed To Login Message
                    label5.Visible = true;
                    await Task.Delay(2000);
                    label5.Visible = false;
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //Show Create Account Page
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            button2.Visible = true;
            label10.Visible = true;

            //Hide Login Page
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button1.Visible = false;
        }

        //Create Account
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Don't allow spaces when creating first name
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Don't allow spaces when creating last name
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }
        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Only Allow Numbers When Creating Pincode
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string query = "SELECT AccountNumber, Balance, FirstName, LastName FROM Users WHERE FirstName = @FirstName AND PinCode = @PinCode;";
            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(query, connection);
            connection.Open();

            if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text.Count() == 4)
            {
                //Hide Create Account
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                button2.Visible = false;
                label10.Visible = false;
                label11.Visible = false;

                //Show Login Page
                label3.Visible = true;
                label4.Visible = true;
                label6.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                button1.Visible = true;

                //Add Account To Database
                string firstName = textBox3.Text;
                string lastName = textBox4.Text;
                string pinCode = textBox5.Text;

                Random random = new Random();
                string accountNumber = random.Next(100000000, 999999999).ToString();

                command.CommandText = "INSERT INTO Users (FirstName, LastName, PinCode, AccountNumber) VALUES (@FirstName, @LastName, @PinCode, @AccountNumber);";

                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@PinCode", pinCode);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                command.ExecuteNonQuery();
            }
            else
            {
                //Show Failed to Create Account Message
                label11.Visible = true;
                await Task.Delay(2000);
                label11.Visible = false;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            //Show Login Page
            label3.Visible = true;
            label4.Visible = true;
            label6.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            button1.Visible = true;

            //Hide Create Account Page
            textBox3.Visible = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            button2.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
        }

        //Show Main Page
        public void ShowMain()
        {
            label1.Visible = true;
            label2.Visible = true;
            label12.Visible = true;
            panel1.Visible = true;
            pictureBox1.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;

            string firstName = textBox3.Text;
            string pinCode = textBox5.Text;

            string query = "SELECT AccountNumber, Balance, FirstName, LastName FROM Users WHERE FirstName = @FirstName AND PinCode = @PinCode;";
            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(query, connection);
            connection.Open();

            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    double balance = Convert.ToDouble(reader["Balance"]);
                    string fname = reader["FirstName"].ToString();
                    string lname = reader["LastName"].ToString();

                    label1.Text = balance.ToString() + "$";
                    label12.Text = fname.ToString() + " " + lname.ToString();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;

            button6.Visible = true;
            button7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            button10.Visible = true;
            button11.Visible = true;
            button12.Visible = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;

            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            button15.Visible = false;
            button16.Visible = false;
            button17.Visible = false;
            button18.Visible = false;
            button12.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;

            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 50 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 500 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 2500 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 10000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 50000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + 100000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 50 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 2500 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 50000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 500 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 100000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string accountNumber;
            string pinCode;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;


            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - 10000 WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);

            command.ExecuteNonQuery();

            UpdateBalance();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;

            button13.Visible = true;
            button14.Visible = true;
            button15.Visible = true;
            button16.Visible = true;
            button17.Visible = true;
            button18.Visible = true;
            button12.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;

            button19.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string toAccountNumber;
            string amount;

            toAccountNumber = textBox6.Text;
            amount = textBox7.Text;

            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance + @Amount WHERE AccountNumber = @ToAccountNumber";

            command.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
            command.Parameters.AddWithValue("@Amount", amount);

            command.ExecuteNonQuery();

            button3.Visible = true;
            button4.Visible = true;
            button5.Visible = true;

            button19.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            label13.Visible = false;
            label14.Visible = false;


            transactionUpdate();
        }

        public void transactionUpdate()
        {
            string accountNumber;
            string pinCode;
            string amount;

            accountNumber = textBox1.Text;
            pinCode = textBox2.Text;
            amount = textBox7.Text;

            string connectionString = connectionPath;
            using var connection = new SQLiteConnection(connectionString);
            using var command = new SQLiteCommand(connection);
            connection.Open();

            command.CommandText = "UPDATE Users SET Balance = Balance - @Amount WHERE AccountNumber = @AccountNumber AND PinCode = @PinCode";

            command.Parameters.AddWithValue("@AccountNumber", accountNumber);
            command.Parameters.AddWithValue("@PinCode", pinCode);
            command.Parameters.AddWithValue("@Amount", amount);

            command.ExecuteNonQuery();

            UpdateBalance();
        }
    }
}