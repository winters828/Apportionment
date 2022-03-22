using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

/* Main notes on what needs to get done
             
    Required features.
        *All required goals met!              

    Optional nice additional features.
        - Adding a link box to the government website on apportionment would be nice https://www.census.gov/history/www/reference/apportionment/
              
    Finished (To feel more acomplished I guess)
        - After new input is entered you should be able to press update and it will do so with new information
        - the Items box should not be empty when update is clicked while combo box is custom
        - The apportionment method itself, the math and all.
        - Start on the Custom
        - population column should have a length limit, this should be easy
        - Apportioned is readOnly and always just newly updated output
        - Population must only be numbers
        - Fill out preset tables for the years in the combo box. took all night...
        - Nothing can be entered into the third column. good stuff!
        - added a filter to the textbox so that only numbers are allowed by end user.
        - I need some kind of user input box to get the amount of seats/items apportioned.


    General Notes
        - textbox1.Text is the fucking value entered by the end user ya dip
        Calling the datagrid and going through the methods is how you add rows and columns!
        
    Notes on apportionment 
        We're using the hamilton method of apportionment t
 */

namespace Apportionment
{
    public partial class Apportionment : System.Windows.Forms.Form
    {



        private double items = 0;

        public Apportionment() //The form itself
        {
            InitializeComponent(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {// Almost like a constructor, determine what happens when the form loads.
            comboBox1.SelectedItem = "Custom";
            dataGridView1.Rows.Add();   //A couple to start the end user with.
            dataGridView1.Rows.Add();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {//Here we won't just add a row, We'll initialize the newest one so they aren't null.

            //First add a new row
            if (comboBox1.Text.Equals("Custom"))
                  dataGridView1.Rows.Add();

            //get the number of rows to know to initialize the last one.
            int numRows = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) //May initialize each row to empty 
                numRows++;

            //initialize the newly added row
            int i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if(i==(numRows-1)) //Then it's the last row, initialize only this.
                {
                    row.Cells[1].Value = "";
                }
                i++;
            }
                

        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("Custom"))
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1); //Mind your indexing
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            //Make sure there's an item amount in the box, if not an error pops up and update is aborted
            if (Items.Text == "") { MessageBox.Show("  Please Enter something in the items field", "End User Error"); return; }
            items = double.Parse(Items.Text); //Convert Items value
            
            //Hamilton function.
            Hamilton(sender,e);

            if (comboBox1.Text.Equals("Custom"))
                label2.Text = "Number of Items:               " + this.Items.Text;
        }


        private void Hamilton(object sender, EventArgs e)
        {
            //Get the number of rows, this will be extremely helpful.
            int numRows = 0, i = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {numRows++;}

            double popTotal = 0;
            double[] rowsAsDubs = new double[numRows];
            string[] popColStr = new string[numRows]; //Now we have the proper space for the amount of data there is.
            
            //Take input data as string, we'll clean up the string and turn it to doubles and ints to be processed.
            foreach (DataGridViewRow row in dataGridView1.Rows) //This is how to access every row in a coloumn. Brilliant.
            {
                popColStr[i] = row.Cells[1].Value.ToString();
                i++; //keep the iterator at the end
            }


            for(i=0; i<numRows; i++)
            {
                //ends the update, sends a message and doesn't do anything if there aren't numbers filled out in the popbox
                if (popColStr[i] == "") {MessageBox.Show("  Please fill out all the population boxes with numbers","End User Error"); return;}
                rowsAsDubs[i] = double.Parse(popColStr[i]);

                //Getting the population total from all rows as doubles
                popTotal += rowsAsDubs[i];
            }

            //With a fully filtered set of ints "rowsAsInts[i]", popTotal and items value, we can now process the data.
            double lowerDif = 0; double[] decimals = new double[numRows]; int leftover = 0;
            double ppi = 0; double[] quota = new double[numRows]; int[] apportioned = new int[numRows];

            ppi = popTotal / items; // People per Item

            //Here we get our quota values
            for (i = 0; i < numRows; i++)
            {
                quota[i] = rowsAsDubs[i] / ppi;
                
                lowerDif = lowerDif + Math.Floor(quota[i]);
                apportioned[i] = (int) Math.Floor(quota[i]);
                decimals[i] = Math.Round(quota[i] % Math.Floor(quota[i]),3); //For later comparison
                if(quota[i]<1) { decimals[i] = quota[i]; } //You can't divide by zero, so if you floor 0.0345 you get null. This fixes that.
                Debug.WriteLine("Here's your Quota[" + i + "] -> " + quota[i] + " Here's your decimals[" + i + "] -> " + decimals[i]);
            }
            Debug.WriteLine("This is your floored total of items (test) -> " + lowerDif + "\nThis is your original amount of items -> " + items);
            
            //So This is where the Hamilton version of this method comes in.
            //We want to find the difference between how many representitives are left over
            leftover = (int)(items - lowerDif); //How many item(s) left over

            //Then find the descending highest decimals, and give 1 leftover seat/item to each quota, highest to lowest until there are none left.  
            double fine = 0.999;
            while ((fine>0)&&(leftover>0)) //Sorting out remaining items to the Highest decimals in descending order.
            {
                fine = Math.Round((fine - 0.001),3);

                for(int k=0; k<numRows; k++) //Checking each decimal to see which is highest
                {
                    if(fine==decimals[k]) //First one to equal is highest and that apportionment at k get's an additional item
                    {
                        apportioned[k] = apportioned[k] + 1;
                        leftover--;
                    }
                }
            }

            //Data we want to output qouta[], apportioned[]
            //Make sure to clear specifically the last two columns of the old data and update it with new data.
            i = 0; //foreach to output the new information
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[2].Value = Math.Round(quota[i],3);
                row.Cells[3].Value = apportioned[i];
                i++;
            }

        }// End of Hamilton function.


        private void Items_KeyPress(object sender, KeyPressEventArgs e)
        {//Everything that happens when end user presses a key, for now just ignoring non-numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            dataGridView1.AllowUserToAddRows = false; //Turns off auto rows
            int newInteger;
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText.Equals("Population"))
            {//It says the last condition is useless, it's not. This will refuse to enter anymore than 12 digits
                if (!int.TryParse(e.FormattedValue.ToString(), out newInteger) || newInteger < 0 || newInteger > 999999999999)
                {
                    e.Cancel = true;
                    dataGridView1.Rows[e.RowIndex].ErrorText = "the value must be a non-negative integer";
                }
            }


        }

        //When you change your selection in the combobox
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

            //This is where we also need to handle the data entered in the textbox "Items"
            //This is done with Items.Text


            //This is supposed to be the interactive part. Start with updates via the update button
            if (comboBox1.Text.Equals("Custom"))
            {

                //This allows Participants and Population to be customizable
                this.Participants.ReadOnly = false;
                this.Population.ReadOnly = false;

                //You might also need this later to clear old tables
                this.dataGridView1.Columns.Clear();
                this.dataGridView1.Columns.Add(Participants);
                this.dataGridView1.Columns.Add(Population);
                this.dataGridView1.Columns.Add(Quota);
                this.dataGridView1.Columns.Add(Apportioned);
                //For some strange reason, they force Apportioned between Quota and Population. So I forced it back to its proper position.
                dataGridView1.Columns["Quota"].DisplayIndex = 2;
                dataGridView1.Columns["Apportioned"].DisplayIndex = 3;


            }
            else
            {
                MyGridSetup(); //These are the presets when selected, large amount of data so placed into its own function
            }
            Debug.WriteLine("End of Combobox change, pretty much the meat and potatoes of the project");

        }//End of Combobox change

        private void MyGridSetup() 
        {
            //dataGridView1
            //MyGridSetup will be used to load preloaded grids for the user

            //These lines will clear the table before refreshing a new one. note that for 'Custom' option
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Columns.Add(Participants);
            this.dataGridView1.Columns.Add(Population);
            this.dataGridView1.Columns.Add(Apportioned);

            //2020    Updated and finished
            if (comboBox1.Text.Equals("Representitives Apportioned 2020"))
            {
                //Columns[0] = Participants   Columns[1] = Population   Columns[2] = Apportioned
                //If you do a column higher than [2] without the end user adding one, expect a crash

                //This is the presaved number of seats available in the house of representitives. 
                this.label2.Text = "Number of seats:               435";

                string[] row1 = new string[] {"Alabama","4,917,689","7"};
                string[] row2 = new string[] {"Alaska", "721,523", "1" };
                string[] row3 = new string[] { "Arizona", "7,338,461", "10" };
                string[] row4 = new string[] { "Arkansas", "3,052,453", "4" };
                string[] row5 = new string[] { "California", "40,258,486", "53" };
                string[] row6 = new string[] { "Colorado", "5,837,665", "8" };
                string[] row7 = new string[] { "Connecticut", "3,589,462", "5" };
                string[] row8 = new string[] { "Delaware", "897,934", "1" };
                string[] row9 = new string[] { "Florida", "21,965,514", "29" };
                string[] row10 = new string[] { "Georgia", "10,776,665", "14" };
                string[] row11 = new string[] { "Hawaii", "1,424,184", "2" };
                string[] row12 = new string[] { "Idaho", "1,827,502", "2" };
                string[] row13 = new string[] { "Illinois", "12,700,266", "17" };
                string[] row14 = new string[] { "Indiana", "6,765,755", "9" };
                string[] row15 = new string[] { "Iowa", "3,190,345", "4" };
                string[] row16 = new string[] { "Kansas", "2,929,419", "4" };
                string[] row17 = new string[] { "Kentucky", "4,505,741", "6" };
                string[] row18 = new string[] { "Louisiana", "4,678,927", "6" };
                string[] row19 = new string[] { "Maine", "1,353,064", "2" };
                string[] row20 = new string[] { "Maryland", "6,134,800", "8" };
                string[] row21 = new string[] { "Massachusetts", "6,968,593", "9" };
                string[] row22 = new string[] { "Michigan", "10,049,698", "13" };
                string[] row23 = new string[] { "Minnesota", "5,731,946", "7" };
                string[] row24 = new string[] { "Mississippi", "2,980,200", "4" };
                string[] row25 = new string[] { "Missouri", "6,180,834", "8" };
                string[] row26 = new string[] { "Montana", "1,085,911", "2" };
                string[] row27 = new string[] { "Nebraska", "1,957,636", "3" };
                string[] row28 = new string[] { "Neveda", "3,173,851", "4" };
                string[] row29 = new string[] { "New Hampshire", "1,366,264", "2" };
                string[] row30 = new string[] { "New Jersey", "9,087,190", "12" };
                string[] row31 = new string[] { "New Mexico", "2,096,116", "3" };
                string[] row32 = new string[] { "New York", "19,888,378", "26" };
                string[] row33 = new string[] { "North Carolina", "10,623,246", "14" };
                string[] row34 = new string[] { "North Dakota", "672,591", "1" };
                string[] row35 = new string[] { "Ohio", "11,767,761", "15" };
                string[] row36 = new string[] { "Oklahoma", "3,960,012", "5" };
                string[] row37 = new string[] { "Oregon", "4,312,909", "6" };
                string[] row38 = new string[] { "Pennsylvania", "12,862,405", "17" };
                string[] row39 = new string[] { "Rhode Island", "1,052,567", "1" };
                string[] row40 = new string[] { "South Carolina", "5,217,530", "7" };
                string[] row41 = new string[] { "South Dakota", "814,180", "7" };
                string[] row42 = new string[] { "Tennessee", "6,915,712", "9" };
                string[] row43 = new string[] { "Texas", "29,502,406", "38" };
                string[] row44 = new string[] { "Utah", "3,274,252", "4" };
                string[] row45 = new string[] { "Vermont", "625,741", "1" };
                string[] row46 = new string[] { "Virginia", "8,637,117", "11" };
                string[] row47 = new string[] { "Washington", "7,779,438", "10" };
                string[] row48 = new string[] { "West Virginia", "1,777,550", "2" };
                string[] row49 = new string[] { "Wisconsin", "5,863,772", "7" };
                string[] row50 = new string[] { "Wyoming", "563,626", "1" };

                object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12,
               row13, row14, row15, row16, row17, row18, row19, row20, row21, row22, row23, row24,row25, row26, row27, row28, row29, 
                    row30, row31, row32, row33, row34, row35, row36, row37, row38, row39, row40, row41, row42, row43, row44, row45, row46,
                    row47, row48, row49, row50};

                foreach (string[] rowArray in rows)
                {
                    dataGridView1.Rows.Add(rowArray);
                }



            }

            //2010   Updated and finished
            if (comboBox1.Text.Equals("Representitives Apportioned 2010"))
            {
                //Columns[0] = Participants   Columns[1] = Population   Columns[2] = Apportioned
                //If you do a column higher than [2] without the end user adding one, expect a crash

                this.label2.Text = "Number of seats:               435";

                string[] row1 = new string[] { "Alabama", "4,802,982", "7" };
                string[] row2 = new string[] { "Alaska", "721,523", "1" };
                string[] row3 = new string[] { "Arizona", "6,412,700", "9" };
                string[] row4 = new string[] { "Arkansas", "2,926,229", "4" };
                string[] row5 = new string[] { "California", "37,341,989", "53" };
                string[] row6 = new string[] { "Colorado", "5,044,930", "7" };
                string[] row7 = new string[] { "Connecticut", "3,581,628", "5" };
                string[] row8 = new string[] { "Delaware", "900,877", "1" };
                string[] row9 = new string[] { "Florida", "18,900,773", "27" };
                string[] row10 = new string[] { "Georgia", "9,727,566", "14" };
                string[] row11 = new string[] { "Hawaii", "1,366,862", "2" };
                string[] row12 = new string[] { "Idaho", "1,573,499", "2" };
                string[] row13 = new string[] { "Illinois", "12,864,380", "18" };
                string[] row14 = new string[] { "Indiana", "6,501,380", "9" };
                string[] row15 = new string[] { "Iowa", "3,053,787", "4" };
                string[] row16 = new string[] { "Kansas", "2,863,813", "4" };
                string[] row17 = new string[] { "Kentucky", "4,350,606", "6" };
                string[] row18 = new string[] { "Louisiana", "4,553,962", "6" };
                string[] row19 = new string[] { "Maine", "1,333,074", "2" };
                string[] row20 = new string[] { "Maryland", "5,789,929", "8" };
                string[] row21 = new string[] { "Massachusetts", "6,559,944", "9" };
                string[] row22 = new string[] { "Michigan", "9,911,626", "14" };
                string[] row23 = new string[] { "Minnesota", "5,314,879", "8" };
                string[] row24 = new string[] { "Mississippi", "2,978,240", "4" };
                string[] row25 = new string[] { "Missouri", "6,011,478", "8" };
                string[] row26 = new string[] { "Montana", "994,416", "1" };
                string[] row27 = new string[] { "Nebraska", "1,831,825", "3" };
                string[] row28 = new string[] { "Neveda", "2,709,432", "4" };
                string[] row29 = new string[] { "New Hampshire", "1,321,445", "2" };
                string[] row30 = new string[] { "New Jersey", "8,807,501", "12" };
                string[] row31 = new string[] { "New Mexico", "2,067,273", "3" };
                string[] row32 = new string[] { "New York", "19,421,055", "27" };
                string[] row33 = new string[] { "North Carolina", "9,565,781", "13" };
                string[] row34 = new string[] { "North Dakota", "675,905", "1" };
                string[] row35 = new string[] { "Ohio", "11,568,495", "16" };
                string[] row36 = new string[] { "Oklahoma", "3,764,882", "5" };
                string[] row37 = new string[] { "Oregon", "4,312,909", "6" };
                string[] row38 = new string[] { "Pennsylvania", "12,862,405", "18" };
                string[] row39 = new string[] { "Rhode Island", "1,052,567", "2" };
                string[] row40 = new string[] { "South Carolina", "5,217,530", "7" };
                string[] row41 = new string[] { "South Dakota", "6,375,431", "1" };
                string[] row42 = new string[] { "Tennessee", "25,268,418", "9" };
                string[] row43 = new string[] { "Texas", "2,770,765", "36" };
                string[] row44 = new string[] { "Utah", "630,337", "4" };
                string[] row45 = new string[] { "Vermont", "8,037,736", "1" };
                string[] row46 = new string[] { "Virginia", "6,753,369", "11" };
                string[] row47 = new string[] { "Washington", "1,859,815", "10" };
                string[] row48 = new string[] { "West Virginia", "5,698,230", "3" };
                string[] row49 = new string[] { "Wisconsin", "5,467,234", "8" };
                string[] row50 = new string[] { "Wyoming", "568,300", "1" };
                object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12,
               row13, row14, row15, row16, row17, row18, row19, row20, row21, row22, row23, row24,row25, row26, row27, row28, row29,
                    row30, row31, row32, row33, row34, row35, row36, row37, row38, row39, row40, row41, row42, row43, row44, row45, row46,
                    row47, row48, row49, row50};

                foreach (string[] rowArray in rows)
                {
                    dataGridView1.Rows.Add(rowArray);
                }


            }

            //2000   updated and finished
            if (comboBox1.Text.Equals("Representitives Apportioned 2000"))
            {
                //Columns[0] = Participants   Columns[1] = Population   Columns[2] = Apportioned
                //If you do a column higher than [2] without the end user adding one, expect a crash

                this.label2.Text = "Number of seats:               435";

                string[] row1 = new string[] { "Alabama", "4,461,130", "7" };
                string[] row2 = new string[] { "Alaska", "628,933", "1" };
                string[] row3 = new string[] { "Arizona", "5,140,683", "8" };
                string[] row4 = new string[] { "Arkansas", "2,679,733", "4" };
                string[] row5 = new string[] { "California", "33,930,798", "53" };
                string[] row6 = new string[] { "Colorado", "4,311,882", "7" };
                string[] row7 = new string[] { "Connecticut", "3,409,535", "5" };
                string[] row8 = new string[] { "Delaware", "785,068", "1" };
                string[] row9 = new string[] { "Florida", "16,028,890", "25" };
                string[] row10 = new string[] { "Georgia", "8,206,975", "13" };
                string[] row11 = new string[] { "Hawaii", "8,206,975", "2" };
                string[] row12 = new string[] { "Idaho", "1,573,499", "2" };
                string[] row13 = new string[] { "Illinois", "12,864,380", "19" };
                string[] row14 = new string[] { "Indiana", "6,501,380", "9" };
                string[] row15 = new string[] { "Iowa", "3,053,787", "5" };
                string[] row16 = new string[] { "Kansas", "2,863,813", "4" };
                string[] row17 = new string[] { "Kentucky", "4,350,606", "6" };
                string[] row18 = new string[] { "Louisiana", "4,553,962", "7" };
                string[] row19 = new string[] { "Maine", "1,333,074", "2" };
                string[] row20 = new string[] { "Maryland", "5,789,929", "8" };
                string[] row21 = new string[] { "Massachusetts", "6,559,944", "10" };
                string[] row22 = new string[] { "Michigan", "9,911,626", "15" };
                string[] row23 = new string[] { "Minnesota", "5,314,879", "8" };
                string[] row24 = new string[] { "Mississippi", "2,978,240", "4" };
                string[] row25 = new string[] { "Missouri", "6,011,478", "9" };
                string[] row26 = new string[] { "Montana", "994,416", "1" };
                string[] row27 = new string[] { "Nebraska", "1,831,825", "3" };
                string[] row28 = new string[] { "Neveda", "2,709,432", "3" };
                string[] row29 = new string[] { "New Hampshire", "1,321,445", "2" };
                string[] row30 = new string[] { "New Jersey", "8,807,501", "13" };
                string[] row31 = new string[] { "New Mexico", "2,067,273", "3" };
                string[] row32 = new string[] { "New York", "19,421,055", "29" };
                string[] row33 = new string[] { "North Carolina", "9,565,781", "13" };
                string[] row34 = new string[] { "North Dakota", "675,905", "1" };
                string[] row35 = new string[] { "Ohio", "11,568,495", "18" };
                string[] row36 = new string[] { "Oklahoma", "3,764,882", "5" };
                string[] row37 = new string[] { "Oregon", "4,312,909", "5" };
                string[] row38 = new string[] { "Pennsylvania", "12,300,670", "19" };
                string[] row39 = new string[] { "Rhode Island", "1,049,662", "2" };
                string[] row40 = new string[] { "South Carolina", "4,025,061", "6" };
                string[] row41 = new string[] { "South Dakota", "756,874", "1" };
                string[] row42 = new string[] { "Tennessee", "5,700,037", "9" };
                string[] row43 = new string[] { "Texas", "20,903,994", "32" };
                string[] row44 = new string[] { "Utah", "2,236,714", "3" };
                string[] row45 = new string[] { "Vermont", "609,890", "1" };
                string[] row46 = new string[] { "Virginia", "7,100,702", "11" };
                string[] row47 = new string[] { "Washington", "5,908,684", "9" };
                string[] row48 = new string[] { "West Virginia", "1,813,077", "3" };
                string[] row49 = new string[] { "Wisconsin", "5,371,210", "8" };
                string[] row50 = new string[] { "Wyoming", "495,304", "1" };
                object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12,
               row13, row14, row15, row16, row17, row18, row19, row20, row21, row22, row23, row24,row25, row26, row27, row28, row29,
                    row30, row31, row32, row33, row34, row35, row36, row37, row38, row39, row40, row41, row42, row43, row44, row45, row46,
                    row47, row48, row49, row50};

                foreach (string[] rowArray in rows)
                {
                    dataGridView1.Rows.Add(rowArray);
                }


            }

            //1990
            if (comboBox1.Text.Equals("Representitives Apportioned 1990"))
            {
                //Columns[0] = Participants   Columns[1] = Population   Columns[2] = Apportioned
                //If you do a column higher than [2] without the end user adding one, expect a crash

                this.label2.Text = "Number of seats:               435";

                string[] row1 = new string[] { "Alabama", "4,802,982", "7" };
                string[] row2 = new string[] { "Alaska", "721,523", "1" };
                string[] row3 = new string[] { "Arizona", "6,412,700", "6" };
                string[] row4 = new string[] { "Arkansas", "2,926,229", "4" };
                string[] row5 = new string[] { "California", "37,341,989", "52" };
                string[] row6 = new string[] { "Colorado", "5,044,930", "6" };
                string[] row7 = new string[] { "Connecticut", "3,581,628", "6" };
                string[] row8 = new string[] { "Delaware", "900,877", "1" };
                string[] row9 = new string[] { "Florida", "18,900,773", "23" };
                string[] row10 = new string[] { "Georgia", "9,727,566", "11" };
                string[] row11 = new string[] { "Hawaii", "1,366,862", "2" };
                string[] row12 = new string[] { "Idaho", "1,573,499", "2" };
                string[] row13 = new string[] { "Illinois", "12,864,380", "20" };
                string[] row14 = new string[] { "Indiana", "6,501,380", "10" };
                string[] row15 = new string[] { "Iowa", "3,053,787", "5" };
                string[] row16 = new string[] { "Kansas", "2,863,813", "4" };
                string[] row17 = new string[] { "Kentucky", "4,350,606", "6" };
                string[] row18 = new string[] { "Louisiana", "4,553,962", "7" };
                string[] row19 = new string[] { "Maine", "1,333,074", "2" };
                string[] row20 = new string[] { "Maryland", "5,789,929", "8" };
                string[] row21 = new string[] { "Massachusetts", "6,559,944", "10" };
                string[] row22 = new string[] { "Michigan", "9,911,626", "16" };
                string[] row23 = new string[] { "Minnesota", "5,314,879", "8" };
                string[] row24 = new string[] { "Mississippi", "2,978,240", "5" };
                string[] row25 = new string[] { "Missouri", "6,011,478", "9" };
                string[] row26 = new string[] { "Montana", "994,416", "1" };
                string[] row27 = new string[] { "Nebraska", "1,831,825", "3" };
                string[] row28 = new string[] { "Neveda", "2,709,432", "2" };
                string[] row29 = new string[] { "New Hampshire", "1,321,445", "2" };
                string[] row30 = new string[] { "New Jersey", "8,807,501", "13" };
                string[] row31 = new string[] { "New Mexico", "2,067,273", "3" };
                string[] row32 = new string[] { "New York", "19,421,055", "31" };
                string[] row33 = new string[] { "North Carolina", "9,565,781", "12" };
                string[] row34 = new string[] { "North Dakota", "675,905", "1" };
                string[] row35 = new string[] { "Ohio", "11,568,495", "19" };
                string[] row36 = new string[] { "Oklahoma", "3,764,882", "6" };
                string[] row37 = new string[] { "Oregon", "4,312,909", "5" };
                string[] row38 = new string[] { "Pennsylvania", "12,862,405", "21" };
                string[] row39 = new string[] { "Rhode Island", "1,052,567", "2" };
                string[] row40 = new string[] { "South Carolina", "5,217,530", "6" };
                string[] row41 = new string[] { "South Dakota", "6,375,431", "1" };
                string[] row42 = new string[] { "Tennessee", "25,268,418", "9" };
                string[] row43 = new string[] { "Texas", "2,770,765", "30" };
                string[] row44 = new string[] { "Utah", "630,337", "3" };
                string[] row45 = new string[] { "Vermont", "8,037,736", "1" };
                string[] row46 = new string[] { "Virginia", "6,753,369", "11" };
                string[] row47 = new string[] { "Washington", "1,859,815", "9" };
                string[] row48 = new string[] { "West Virginia", "5,698,230", "3" };
                string[] row49 = new string[] { "Wisconsin", "568,300", "9" };
                string[] row50 = new string[] { "Wyoming", "455,463", "1" };

                object[] rows = new object[] { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12,
               row13, row14, row15, row16, row17, row18, row19, row20, row21, row22, row23, row24,row25, row26, row27, row28, row29,
                    row30, row31, row32, row33, row34, row35, row36, row37, row38, row39, row40, row41, row42, row43, row44, row45, row46,
                    row47, row48, row49, row50};

                foreach (string[] rowArray in rows)
                {
                    dataGridView1.Rows.Add(rowArray);
                }


            }

        }//End of myGridSetup home to all the preset tables

    }//End of class
}//End of namespace