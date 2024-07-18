using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;


namespace TravelessReservationSystem
{
    public partial class ReservationForm : Form
    {
        private Label codeLabel;
        private Label airlineLabel;
        private Label nameLabel;
        private Label reservationCodeLabel;
        private Label flightCodeLabel;
        private Label airlineReserveLabel;
        private Label dayLabel;
        private Label costLabel;
        private Label timeLabel;
        private Label nameReserveLabel;
        private Label citizenshipLabel;
        private Label statusLabel;

        private TextBox codeTextBox;
        private TextBox airlineTextBox;
        private TextBox nameTextBox;
        private TextBox reservationCodeTextBox;
        private TextBox flightCodeTextBox;
        private TextBox airlineReserveTextBox;
        private TextBox dayTextBox;
        private TextBox costTextBox;
        private TextBox timeTextBox;
        private TextBox nameReserveTextBox;
        private TextBox citizenshipTextBox;
        private TextBox statusTextBox;

        private Button findReservationsButton;
        private Button saveButton;
        private ListView reservationsListView;

        public ReservationForm()
        {
            InitializeComponent();
        }

        private void findReservationsButton_Click(object sender, EventArgs e)
{
    string code = codeTextBox.Text.Trim();
    string airline = airlineTextBox.Text.Trim();
    string name = nameTextBox.Text.Trim();

    // Define the file path for reservations.txt
    string filePath = @"C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/data/reservations.txt";

    // Check if the file exists
    if (!File.Exists(filePath))
    {
        MessageBox.Show("No reservations found. The reservations file does not exist or could not be found.", "No Reservations", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
    }

    // Read all lines from reservations.txt
    List<string> reservationLines = new List<string>();
    try
    {
        string[] lines = File.ReadAllLines(filePath);
        reservationLines.AddRange(lines);
    }
    catch (IOException ex)
    {
        MessageBox.Show($"Error loading reservation data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }

    // Search for reservations matching any of the criteria
    bool reservationFound = false;
    foreach (string line in reservationLines)
    {
        string[] parts = line.Split(',');

        // Check if the line has enough parts to match the expected format
        if (parts.Length >= 10) // Adjust if needed based on your reservation format
        {
            string flightCode = parts[0].Trim();
            string flightAirline = parts[1].Trim();
            string passengerName = parts[9].Trim(); // Index 9 for passenger name

            // Check if the reservation matches any of the search criteria
            if (flightCode.Equals(code, StringComparison.OrdinalIgnoreCase) ||
                flightAirline.Equals(airline, StringComparison.OrdinalIgnoreCase) ||
                passengerName.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                // Fill text boxes with reservation details
                reservationCodeTextBox.Text = GenerateReservationCode();
                flightCodeTextBox.Text = flightCode;
                airlineReserveTextBox.Text = flightAirline;
                dayTextBox.Text = parts[4].Trim(); // Day of week
                timeTextBox.Text = parts[5].Trim(); // Time
                costTextBox.Text = parts[7].Trim(); // Cost
                nameReserveTextBox.Text = passengerName;
                citizenshipTextBox.Text = parts[10].Trim(); // Citizenship

                // Optionally update other fields like statusTextBox, etc.
                statusTextBox.Text = "Reservation found."; // Update status or other relevant fields

                reservationFound = true;
                break; // Exit loop since we found a matching reservation
            }
        }
    }

    if (!reservationFound)
    {
        // Clear text boxes and update status if no reservation found
        reservationCodeTextBox.Text = "";
        flightCodeTextBox.Text = "";
        airlineReserveTextBox.Text = "";
        dayTextBox.Text = "";
        timeTextBox.Text = "";
        costTextBox.Text = "";
        nameReserveTextBox.Text = "";
        citizenshipTextBox.Text = "";
        statusTextBox.Text = "No reservations found matching the criteria.";
    }
}
 
 private string GenerateReservationCode()
{
    // Logic to generate a unique reservation code in the format LL-DDDD
    Random random = new Random();
    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    string digits = "0123456789";

    string code = $"{letters[random.Next(0, letters.Length)]}{letters[random.Next(0, letters.Length)]}-{digits[random.Next(0, digits.Length)]}{digits[random.Next(0, digits.Length)]}{digits[random.Next(0, digits.Length)]}{digits[random.Next(0, digits.Length)]}";

    return code;
}



        private void saveButton_Click(object sender, EventArgs e)
        {
            // Gather data from text boxes
            string reservationCode = reservationCodeTextBox.Text.Trim();
            string flightCode = flightCodeTextBox.Text.Trim();
            string airline = airlineReserveTextBox.Text.Trim();
            string day = dayTextBox.Text.Trim();
            string time = timeTextBox.Text.Trim();
            string cost = costTextBox.Text.Trim();
            string name = nameReserveTextBox.Text.Trim();
            string citizenship = citizenshipTextBox.Text.Trim();

            // Construct the line to save in CSV format
            string reservationData = $"{reservationCode},{flightCode},{airline},{day},{time},{cost},{name},{citizenship}";

            // Specify the path to your reservations CSV file
            string filePath = "C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/reservations.txt";

            try
            {
                // Write reservation data to the file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(reservationData);
                }

                // Show success message
                MessageBox.Show("Reservation saved successfully.");
            }
            catch (IOException ex)
            {
                // Handle file IO exception
                MessageBox.Show($"Error saving reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InitializeComponent()
        {
            this.codeLabel = new System.Windows.Forms.Label();
            this.airlineLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.reservationCodeLabel = new System.Windows.Forms.Label();
            this.flightCodeLabel = new System.Windows.Forms.Label();
            this.airlineReserveLabel = new System.Windows.Forms.Label();
            this.dayLabel = new System.Windows.Forms.Label();
            this.costLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.nameReserveLabel = new System.Windows.Forms.Label();
            this.citizenshipLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();

            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.airlineTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.reservationCodeTextBox = new System.Windows.Forms.TextBox();
            this.flightCodeTextBox = new System.Windows.Forms.TextBox();
            this.airlineReserveTextBox = new System.Windows.Forms.TextBox();
            this.dayTextBox = new System.Windows.Forms.TextBox();
            this.costTextBox = new System.Windows.Forms.TextBox();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.nameReserveTextBox = new System.Windows.Forms.TextBox();
            this.citizenshipTextBox = new System.Windows.Forms.TextBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();

            this.findReservationsButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.reservationsListView = new System.Windows.Forms.ListView();

            this.SuspendLayout();

            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(12, 15);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(32, 13);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Code";
            // 
            // airlineLabel
            // 
            this.airlineLabel.AutoSize = true;
            this.airlineLabel.Location = new System.Drawing.Point(185, 15);
            this.airlineLabel.Name = "airlineLabel";
            this.airlineLabel.Size = new System.Drawing.Size(36, 13);
            this.airlineLabel.TabIndex = 1;
            this.airlineLabel.Text = "Airline";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(358, 15);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 2;
            this.nameLabel.Text = "Name";
            // 
            // reservationCodeLabel
            // 
            this.reservationCodeLabel.AutoSize = true;
            this.reservationCodeLabel.Location = new System.Drawing.Point(12, 120);
            this.reservationCodeLabel.Name = "reservationCodeLabel";
            this.reservationCodeLabel.Size = new System.Drawing.Size(90, 13);
            this.reservationCodeLabel.TabIndex = 8;
            this.reservationCodeLabel.Text = "Reservation Code";
            // 
            // flightCodeLabel
            // 
            this.flightCodeLabel.AutoSize = true;
            this.flightCodeLabel.Location = new System.Drawing.Point(12, 160);
            this.flightCodeLabel.Name = "flightCodeLabel";
            this.flightCodeLabel.Size = new System.Drawing.Size(60, 13);
            this.flightCodeLabel.TabIndex = 9;
            this.flightCodeLabel.Text = "Flight Code";
            // 
            // airlineReserveLabel
            // 
            this.airlineReserveLabel.AutoSize = true;
            this.airlineReserveLabel.Location = new System.Drawing.Point(12, 200);
            this.airlineReserveLabel.Name = "airlineReserveLabel";
            this.airlineReserveLabel.Size = new System.Drawing.Size(36, 13);
            this.airlineReserveLabel.TabIndex = 10;
            this.airlineReserveLabel.Text = "Airline";
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Location = new System.Drawing.Point(12, 240);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(28, 13);
            this.dayLabel.TabIndex = 11;
            this.dayLabel.Text = "Day";
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(12, 280);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(28, 13);
            this.costLabel.TabIndex = 12;
            this.costLabel.Text = "Cost";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(12, 320);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(30, 13);
            this.timeLabel.TabIndex = 13;
            this.timeLabel.Text = "Time";
            // 
            // nameReserveLabel
            // 
            this.nameReserveLabel.AutoSize = true;
            this.nameReserveLabel.Location = new System.Drawing.Point(12, 360);
            this.nameReserveLabel.Name = "nameReserveLabel";
            this.nameReserveLabel.Size = new System.Drawing.Size(35, 13);
            this.nameReserveLabel.TabIndex = 14;
            this.nameReserveLabel.Text = "Name";
            // 
            // citizenshipLabel
            // 
            this.citizenshipLabel.AutoSize = true;
            this.citizenshipLabel.Location = new System.Drawing.Point(12, 400);
            this.citizenshipLabel.Name = "citizenshipLabel";
            this.citizenshipLabel.Size = new System.Drawing.Size(58, 13);
            this.citizenshipLabel.TabIndex = 15;
            this.citizenshipLabel.Text = "Citizenship";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 440);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(37, 13);
            this.statusLabel.TabIndex = 16;
            this.statusLabel.Text = "Status";
            // 

            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(50, 12);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(121, 20);
            this.codeTextBox.TabIndex = 3;
            // 
            // airlineTextBox
            // 
            this.airlineTextBox.Location = new System.Drawing.Point(227, 12);
            this.airlineTextBox.Name = "airlineTextBox";
            this.airlineTextBox.Size = new System.Drawing.Size(121, 20);
            this.airlineTextBox.TabIndex = 4;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(399, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(121, 20);
            this.nameTextBox.TabIndex = 5;
            // 

            // reservationCodeTextBox
            // 
            this.reservationCodeTextBox.Location = new System.Drawing.Point(15, 140);
            this.reservationCodeTextBox.Name = "reservationCodeTextBox";
            this.reservationCodeTextBox.Size = new System.Drawing.Size(760, 20);
            this.reservationCodeTextBox.TabIndex = 9;
            // 

            // flightCodeTextBox
            // 
            this.flightCodeTextBox.Location = new System.Drawing.Point(15, 180);
            this.flightCodeTextBox.Name = "flightCodeTextBox";
            this.flightCodeTextBox.Size = new System.Drawing.Size(760, 20);
            this.flightCodeTextBox.TabIndex = 10;
            // 

            // airlineReserveTextBox
            // 
            this.airlineReserveTextBox.Location = new System.Drawing.Point(15, 220);
            this.airlineReserveTextBox.Name = "airlineReserveTextBox";
            this.airlineReserveTextBox.Size = new System.Drawing.Size(760, 20);
            this.airlineReserveTextBox.TabIndex = 11;
            // 

            // dayTextBox
            // 
            this.dayTextBox.Location = new System.Drawing.Point(15, 260);
            this.dayTextBox.Name = "dayTextBox";
            this.dayTextBox.Size = new System.Drawing.Size(760, 20);
            this.dayTextBox.TabIndex = 12;
            // 

            // costTextBox
            // 
            this.costTextBox.Location = new System.Drawing.Point(15, 300);
            this.costTextBox.Name = "costTextBox";
            this.costTextBox.Size = new System.Drawing.Size(760, 20);
            this.costTextBox.TabIndex = 13;
            // 

            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(15, 340);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.Size = new System.Drawing.Size(760, 20);
            this.timeTextBox.TabIndex = 14;
            // 

            // nameReserveTextBox
            // 
            this.nameReserveTextBox.Location = new System.Drawing.Point(15, 380);
            this.nameReserveTextBox.Name = "nameReserveTextBox";
            this.nameReserveTextBox.Size = new System.Drawing.Size(760, 20);
            this.nameReserveTextBox.TabIndex = 15;
            // 

            // citizenshipTextBox
            // 
            this.citizenshipTextBox.Location = new System.Drawing.Point(15, 420);
            this.citizenshipTextBox.Name = "citizenshipTextBox";
            this.citizenshipTextBox.Size = new System.Drawing.Size(760, 20);
            this.citizenshipTextBox.TabIndex = 16;
            // 

            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(15, 460);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(760, 20);
            this.statusTextBox.TabIndex = 17;
            // 

            // findReservationsButton
            // 
            this.findReservationsButton.BackColor = System.Drawing.Color.Blue;
            this.findReservationsButton.ForeColor = System.Drawing.Color.White;
            this.findReservationsButton.Location = new System.Drawing.Point(550, 10);
            this.findReservationsButton.Name = "findReservationsButton";
            this.findReservationsButton.Size = new System.Drawing.Size(120, 23);
            this.findReservationsButton.TabIndex = 6;
            this.findReservationsButton.Text = "Find Reservations";
            this.findReservationsButton.UseVisualStyleBackColor = false;
            this.findReservationsButton.Click += new System.EventHandler(this.findReservationsButton_Click);
            // 

            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.Blue;
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(325, 490); // Centered horizontally
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 50);
            this.saveButton.TabIndex = 18;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 

            // reservationsListView
            // 
            this.reservationsListView.Location = new System.Drawing.Point(15, 50);
            this.reservationsListView.Name = "reservationsListView";
            this.reservationsListView.Size = new System.Drawing.Size(760, 60);
            this.reservationsListView.TabIndex = 7;
            this.reservationsListView.View = System.Windows.Forms.View.List;
            // 

            // ReservationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.citizenshipTextBox);
            this.Controls.Add(this.nameReserveTextBox);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.costTextBox);
            this.Controls.Add(this.dayTextBox);
            this.Controls.Add(this.airlineReserveTextBox);
            this.Controls.Add(this.flightCodeTextBox);
            this.Controls.Add(this.reservationCodeTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.airlineLabel);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.findReservationsButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.citizenshipLabel);
            this.Controls.Add(this.nameReserveLabel);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.dayLabel);
            this.Controls.Add(this.airlineReserveLabel);
            this.Controls.Add(this.flightCodeLabel);
            this.Controls.Add(this.reservationCodeLabel);
            this.Controls.Add(this.reservationsListView);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.airlineTextBox);
            this.Controls.Add(this.nameTextBox);
            this.Name = "ReservationForm";
            this.Text = "Reservation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
