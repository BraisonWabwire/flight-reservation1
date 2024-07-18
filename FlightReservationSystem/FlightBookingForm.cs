using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;


namespace TravelessReservationSystem
{
    public partial class FlightBookingForm : Form
    {
        private Label fromLabel;
        private Label toLabel;
        private Label dayLabel;
        private ComboBox fromComboBox;
        private ComboBox toComboBox;
        private ComboBox dayComboBox; // Changed from DateTimePicker to ComboBox
        private Button findFlightsButton;
        private ListView flightsListView;
        private Label nameLabel;
        private TextBox nameTextBox;
        private Label citizenshipLabel;
        private TextBox citizenshipTextBox;
        private Label flightCodeLabel;
        private TextBox flightCodeTextBox;
        private Label airlineLabel;
        private TextBox airlineTextBox;
        private Label fromFlightLabel;
        private TextBox fromTextBox;
        private Label toFlightLabel;
        private TextBox toTextBox;
        private Label dayFlightLabel;
        private TextBox dayTextBox;
        private Label timeLabel;
        private TextBox timeTextBox;
        private Label durationLabel;
        private TextBox durationTextBox;
        private Label costLabel;
        private TextBox costTextBox;
        private Button reserveButton;

        public FlightBookingForm()
        {
            InitializeComponent();
            Dictionary<string, string> airportCodesToNames = LoadAirportCodesToNames(@"C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/data/airports.csv");
            LoadAirports(airportCodesToNames);  // Load airports into combo boxes
            InitializeReserveButton();
        }

        private void InitializeReserveButton()
        {
            this.reserveButton = new Button();
            this.reserveButton.Text = "Reserve";
            this.reserveButton.Size = new Size(100, 50); // Adjust size as needed
            this.reserveButton.Location = new Point(325, 450); // Adjust x and y coordinates
            this.reserveButton.TabIndex = 29; // Adjust tab index if needed
            this.reserveButton.UseVisualStyleBackColor = false; // Set this to false to use the custom color
            this.reserveButton.BackColor = System.Drawing.Color.Blue;
            this.reserveButton.Font = new System.Drawing.Font(this.reserveButton.Font.FontFamily, 12);
            this.reserveButton.Click += new EventHandler(reserveButton_Click);
            this.Controls.Add(this.reserveButton);
        }
       private void reserveButton_Click(object sender, EventArgs e)
{
    // Collect reservation details from the form
    string flightCode = this.flightCodeTextBox.Text;
    string airline = this.airlineTextBox.Text;
    string from = this.fromTextBox.Text;
    string to = this.toTextBox.Text;
    string day = this.dayTextBox.Text;
    string time = this.timeTextBox.Text;
    string duration = this.durationTextBox.Text;
    string cost = this.costTextBox.Text;
    string passengerName = this.nameTextBox.Text;
    string citizenship = this.citizenshipTextBox.Text;

    // Check if required fields are empty
    if (string.IsNullOrEmpty(passengerName))
    {
        MessageBox.Show("Passenger Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return; // Exit the method without saving
    }

    if (string.IsNullOrEmpty(citizenship))
    {
        MessageBox.Show("Citizenship cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return; // Exit the method without saving
    }

    // Format the reservation details as comma-separated values
    string reservationLine = $"{flightCode},{airline},{from},{to},{day},{time},{duration},{cost},{passengerName},{citizenship}";

    // Define the file path for the .txt file
    string filePath = @"C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/data/reservations.txt";

    // Append the reservation details to the .txt file
    try
    {
        // Check if the file exists, create if it doesn't
        if (!File.Exists(filePath))
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(reservationLine); // Write the first line
            }
        }
        else
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(reservationLine); // Append a new line
            }
        }

        MessageBox.Show("Reservation saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error saving reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}


        private void findFlightsButton_Click(object sender, EventArgs e)
        {
            string fromAirportName = fromComboBox.SelectedItem?.ToString();
            string toAirportName = toComboBox.SelectedItem?.ToString();
            string day = dayComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(fromAirportName) || string.IsNullOrEmpty(toAirportName) || string.IsNullOrEmpty(day))
            {
                MessageBox.Show("Please select From, To, and Day to find flights.");
                return;
            }

            string flightsCsvFilePath = @"C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/data/flights.csv";
            string airportsCsvFilePath = @"C:/Users/braisonW/Desktop/G/flight reservation2/FlightReservationSystem/data/airports.csv";

            try
            {
                List<Flight> flights = LoadFlightsFromCsv(flightsCsvFilePath);
                Dictionary<string, string> airportCodesToNames = LoadAirportCodesToNames(airportsCsvFilePath);

                // Find airport codes for selected airport names
                string fromCode = airportCodesToNames.FirstOrDefault(x => x.Value == fromAirportName).Key;
                string toCode = airportCodesToNames.FirstOrDefault(x => x.Value == toAirportName).Key;

                if (fromCode == null || toCode == null)
                {
                    MessageBox.Show("Invalid airport selection. Please try again.");
                    return;
                }

                var matchingFlights = flights.Where(f =>
                    f.From.Equals(fromCode, StringComparison.OrdinalIgnoreCase) &&
                    f.To.Equals(toCode, StringComparison.OrdinalIgnoreCase) &&
                    f.Day.Equals(day, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                flightsListView.Items.Clear();

                if (matchingFlights.Any())
                {
                    flightsListView.BeginUpdate();

                    foreach (var flight in matchingFlights)
                    {
                        // Display airport names instead of codes
                        string fromName = airportCodesToNames.ContainsKey(flight.From) ? airportCodesToNames[flight.From] : flight.From;
                        string toName = airportCodesToNames.ContainsKey(flight.To) ? airportCodesToNames[flight.To] : flight.To;

                        var item = new ListViewItem(new[]
                        {
                    flight.FlightCode, flight.Airline, fromName, toName,
                    flight.Day, flight.Time, flight.Duration.ToString(), flight.Cost.ToString("C")
                });
                        flightsListView.Items.Add(item);
                    }

                    flightsListView.EndUpdate();

                    // Display details of the first matching flight
                    var firstFlight = matchingFlights.First();
                    flightCodeTextBox.Text = firstFlight.FlightCode;
                    airlineTextBox.Text = firstFlight.Airline;
                    fromTextBox.Text = airportCodesToNames.ContainsKey(firstFlight.From) ? airportCodesToNames[firstFlight.From] : firstFlight.From;
                    toTextBox.Text = airportCodesToNames.ContainsKey(firstFlight.To) ? airportCodesToNames[firstFlight.To] : firstFlight.To;
                    dayTextBox.Text = firstFlight.Day;
                    timeTextBox.Text = firstFlight.Time;
                    durationTextBox.Text = firstFlight.Duration.ToString();
                    costTextBox.Text = firstFlight.Cost.ToString("C");
                }
                else
                {
                    MessageBox.Show("No matching flights found.");

                    // Clear flight details text boxes if no flights found
                    flightCodeTextBox.Text = "";
                    airlineTextBox.Text = "";
                    fromTextBox.Text = "";
                    toTextBox.Text = "";
                    dayTextBox.Text = "";
                    timeTextBox.Text = "";
                    durationTextBox.Text = "";
                    costTextBox.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding flights: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private List<Flight> LoadFlightsFromCsv(string filePath)
        {
            var flights = new List<Flight>();

            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines.Skip(1)) // Skip the header
            {
                var parts = line.Split(',');

                if (parts.Length == 8)
                {
                    flights.Add(new Flight
                    {
                        FlightCode = parts[0].Trim(),
                        Airline = parts[1].Trim(),
                        From = parts[2].Trim(),
                        To = parts[3].Trim(),
                        Day = parts[4].Trim(),
                        Time = parts[5].Trim(),
                        Duration = int.Parse(parts[6].Trim()),
                        Cost = decimal.Parse(parts[7].Trim())
                    });
                }
            }

            return flights;
        }

        private Dictionary<string, string> LoadAirportCodesToNames(string airportsCsvFilePath)
        {
            var airportCodesToNames = new Dictionary<string, string>();

            var lines = File.ReadAllLines(airportsCsvFilePath);
            foreach (var line in lines.Skip(1)) // Skip the header
            {
                var parts = line.Split(',');

                if (parts.Length >= 2)
                {
                    string code = parts[0].Trim();
                    string name = parts[1].Trim();

                    if (!airportCodesToNames.ContainsKey(code))
                    {
                        airportCodesToNames.Add(code, name);
                    }
                }
            }

            return airportCodesToNames;
        }

        private void LoadAirports(Dictionary<string, string> airportCodesToNames)
        {
            foreach (var airportName in airportCodesToNames.Values)
            {
                fromComboBox.Items.Add(airportName);
                toComboBox.Items.Add(airportName);
            }
        }


        private void InitializeComponent()
        {
            this.fromLabel = new System.Windows.Forms.Label();
            this.toLabel = new System.Windows.Forms.Label();
            this.dayLabel = new System.Windows.Forms.Label();
            this.fromComboBox = new System.Windows.Forms.ComboBox();
            this.toComboBox = new System.Windows.Forms.ComboBox();
            this.dayComboBox = new System.Windows.Forms.ComboBox(); // Changed from DateTimePicker to ComboBox
            this.findFlightsButton = new System.Windows.Forms.Button();
            this.flightsListView = new System.Windows.Forms.ListView();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.citizenshipLabel = new System.Windows.Forms.Label();
            this.citizenshipTextBox = new System.Windows.Forms.TextBox();
            this.flightCodeLabel = new System.Windows.Forms.Label();
            this.flightCodeTextBox = new System.Windows.Forms.TextBox();
            this.airlineLabel = new System.Windows.Forms.Label();
            this.airlineTextBox = new System.Windows.Forms.TextBox();
            this.fromFlightLabel = new System.Windows.Forms.Label();
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.toFlightLabel = new System.Windows.Forms.Label();
            this.toTextBox = new System.Windows.Forms.TextBox();
            this.dayFlightLabel = new System.Windows.Forms.Label();
            this.dayTextBox = new System.Windows.Forms.TextBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.durationLabel = new System.Windows.Forms.Label();
            this.durationTextBox = new System.Windows.Forms.TextBox();
            this.costLabel = new System.Windows.Forms.Label();
            this.costTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(12, 15);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(30, 13);
            this.fromLabel.TabIndex = 0;
            this.fromLabel.Text = "From";
            // 
            // toLabel
            // 
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(20, 13);
            this.toLabel.TabIndex = 1;
            this.toLabel.Text = "To";
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Location = new System.Drawing.Point(400, 15);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(26, 13);
            this.dayLabel.TabIndex = 2;
            this.dayLabel.Text = "Day";
            // 
            // fromComboBox
            // 
            this.fromComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fromComboBox.FormattingEnabled = true;
            this.fromComboBox.Location = new System.Drawing.Point(50, 12);
            this.fromComboBox.Name = "fromComboBox";
            this.fromComboBox.Size = new System.Drawing.Size(121, 21);
            this.fromComboBox.TabIndex = 3;
            // 
            // toComboBox
            // 
            this.toComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toComboBox.FormattingEnabled = true;
            this.toComboBox.Location = new System.Drawing.Point(250, 12);
            this.toComboBox.Name = "toComboBox";
            this.toComboBox.Size = new System.Drawing.Size(121, 21);
            this.toComboBox.TabIndex = 4;
            // 
            // dayComboBox
            // 
            this.dayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dayComboBox.FormattingEnabled = true;
            this.dayComboBox.Location = new System.Drawing.Point(430, 12);
            this.dayComboBox.Name = "dayComboBox";
            this.dayComboBox.Size = new System.Drawing.Size(121, 21);
            this.dayComboBox.TabIndex = 5;
            this.dayComboBox.Items.AddRange(new object[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            // 
            // findFlightsButton
            // 
            this.findFlightsButton.Location = new System.Drawing.Point(575, 10);
            this.findFlightsButton.Name = "findFlightsButton";
            this.findFlightsButton.Size = new System.Drawing.Size(75, 23);
            this.findFlightsButton.TabIndex = 6;
            this.findFlightsButton.Text = "Find Flights";
            this.findFlightsButton.UseVisualStyleBackColor = true;
            this.findFlightsButton.Click += new System.EventHandler(this.findFlightsButton_Click);
            // 
            // flightsListView
            // 
            this.flightsListView.Location = new System.Drawing.Point(15, 50);
            this.flightsListView.Name = "flightsListView";
            this.flightsListView.Size = new System.Drawing.Size(635, 150);
            this.flightsListView.TabIndex = 7;
            this.flightsListView.UseCompatibleStateImageBehavior = false;
            this.flightsListView.View = System.Windows.Forms.View.Details;
            this.flightsListView.Columns.Add("Flight Code", 70);
            this.flightsListView.Columns.Add("Airline", 70);
            this.flightsListView.Columns.Add("From", 70);
            this.flightsListView.Columns.Add("To", 70);
            this.flightsListView.Columns.Add("Day", 70);
            this.flightsListView.Columns.Add("Time", 70);
            this.flightsListView.Columns.Add("Duration", 70);
            this.flightsListView.Columns.Add("Cost", 70);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(12, 220);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(35, 13);
            this.nameLabel.TabIndex = 8;
            this.nameLabel.Text = "Name";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(70, 217);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(121, 20);
            this.nameTextBox.TabIndex = 9;
            // 
            // citizenshipLabel
            // 
            this.citizenshipLabel.AutoSize = true;
            this.citizenshipLabel.Location = new System.Drawing.Point(215, 220);
            this.citizenshipLabel.Name = "citizenshipLabel";
            this.citizenshipLabel.Size = new System.Drawing.Size(58, 13);
            this.citizenshipLabel.TabIndex = 10;
            this.citizenshipLabel.Text = "Citizenship";
            // 
            // citizenshipTextBox
            // 
            this.citizenshipTextBox.Location = new System.Drawing.Point(290, 217);
            this.citizenshipTextBox.Name = "citizenshipTextBox";
            this.citizenshipTextBox.Size = new System.Drawing.Size(121, 20);
            this.citizenshipTextBox.TabIndex = 11;
            // 
            // flightCodeLabel
            // 
            this.flightCodeLabel.AutoSize = true;
            this.flightCodeLabel.Location = new System.Drawing.Point(12, 250);
            this.flightCodeLabel.Name = "flightCodeLabel";
            this.flightCodeLabel.Size = new System.Drawing.Size(60, 13);
            this.flightCodeLabel.TabIndex = 12;
            this.flightCodeLabel.Text = "Flight Code";
            // 
            // flightCodeTextBox
            // 
            this.flightCodeTextBox.Location = new System.Drawing.Point(70, 247);
            this.flightCodeTextBox.Name = "flightCodeTextBox";
            this.flightCodeTextBox.Size = new System.Drawing.Size(121, 20);
            this.flightCodeTextBox.TabIndex = 13;
            // 
            // airlineLabel
            // 
            this.airlineLabel.AutoSize = true;
            this.airlineLabel.Location = new System.Drawing.Point(215, 250);
            this.airlineLabel.Name = "airlineLabel";
            this.airlineLabel.Size = new System.Drawing.Size(35, 13);
            this.airlineLabel.TabIndex = 14;
            this.airlineLabel.Text = "Airline";
            // 
            // airlineTextBox
            // 
            this.airlineTextBox.Location = new System.Drawing.Point(290, 247);
            this.airlineTextBox.Name = "airlineTextBox";
            this.airlineTextBox.Size = new System.Drawing.Size(121, 20);
            this.airlineTextBox.TabIndex = 15;
            // 
            // fromFlightLabel
            // 
            this.fromFlightLabel.AutoSize = true;
            this.fromFlightLabel.Location = new System.Drawing.Point(12, 280);
            this.fromFlightLabel.Name = "fromFlightLabel";
            this.fromFlightLabel.Size = new System.Drawing.Size(30, 13);
            this.fromFlightLabel.TabIndex = 16;
            this.fromFlightLabel.Text = "From";
            // 
            // fromTextBox
            // 
            this.fromTextBox.Location = new System.Drawing.Point(70, 277);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(121, 20);
            this.fromTextBox.TabIndex = 17;
            // 
            // toFlightLabel
            // 
            this.toFlightLabel.AutoSize = true;
            this.toFlightLabel.Location = new System.Drawing.Point(215, 280);
            this.toFlightLabel.Name = "toFlightLabel";
            this.toFlightLabel.Size = new System.Drawing.Size(20, 13);
            this.toFlightLabel.TabIndex = 18;
            this.toFlightLabel.Text = "To";
            // 
            // toTextBox
            // 
            this.toTextBox.Location = new System.Drawing.Point(290, 277);
            this.toTextBox.Name = "toTextBox";
            this.toTextBox.Size = new System.Drawing.Size(121, 20);
            this.toTextBox.TabIndex = 19;
            // 
            // dayFlightLabel
            // 
            this.dayFlightLabel.AutoSize = true;
            this.dayFlightLabel.Location = new System.Drawing.Point(12, 310);
            this.dayFlightLabel.Name = "dayFlightLabel";
            this.dayFlightLabel.Size = new System.Drawing.Size(26, 13);
            this.dayFlightLabel.TabIndex = 20;
            this.dayFlightLabel.Text = "Day";
            // 
            // dayTextBox
            // 
            this.dayTextBox.Location = new System.Drawing.Point(70, 307);
            this.dayTextBox.Name = "dayTextBox";
            this.dayTextBox.Size = new System.Drawing.Size(121, 20);
            this.dayTextBox.TabIndex = 21;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(215, 310);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(30, 13);
            this.timeLabel.TabIndex = 22;
            this.timeLabel.Text = "Time";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(290, 307);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.Size = new System.Drawing.Size(121, 20);
            this.timeTextBox.TabIndex = 23;
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new System.Drawing.Point(12, 340);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(47, 13);
            this.durationLabel.TabIndex = 24;
            this.durationLabel.Text = "Duration";
            // 
            // durationTextBox
            // 
            this.durationTextBox.Location = new System.Drawing.Point(70, 337);
            this.durationTextBox.Name = "durationTextBox";
            this.durationTextBox.Size = new System.Drawing.Size(121, 20);
            this.durationTextBox.TabIndex = 25;
            // 
            // costLabel
            // 
            this.costLabel.AutoSize = true;
            this.costLabel.Location = new System.Drawing.Point(215, 340);
            this.costLabel.Name = "costLabel";
            this.costLabel.Size = new System.Drawing.Size(28, 13);
            this.costLabel.TabIndex = 26;
            this.costLabel.Text = "Cost";
            // 
            // costTextBox
            // 
            this.costTextBox.Location = new System.Drawing.Point(290, 337);
            this.costTextBox.Name = "costTextBox";
            this.costTextBox.Size = new System.Drawing.Size(121, 20);
            this.costTextBox.TabIndex = 27;
            // 
            // FlightBookingForm
            // 
            this.ClientSize = new System.Drawing.Size(684, 511);
            this.Controls.Add(this.costTextBox);
            this.Controls.Add(this.costLabel);
            this.Controls.Add(this.durationTextBox);
            this.Controls.Add(this.durationLabel);
            this.Controls.Add(this.timeTextBox);
            this.Controls.Add(this.timeLabel);
            this.Controls.Add(this.dayTextBox);
            this.Controls.Add(this.dayFlightLabel);
            this.Controls.Add(this.toTextBox);
            this.Controls.Add(this.toFlightLabel);
            this.Controls.Add(this.fromTextBox);
            this.Controls.Add(this.fromFlightLabel);
            this.Controls.Add(this.airlineTextBox);
            this.Controls.Add(this.airlineLabel);
            this.Controls.Add(this.flightCodeTextBox);
            this.Controls.Add(this.flightCodeLabel);
            this.Controls.Add(this.citizenshipTextBox);
            this.Controls.Add(this.citizenshipLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.flightsListView);
            this.Controls.Add(this.findFlightsButton);
            this.Controls.Add(this.dayComboBox);
            this.Controls.Add(this.toComboBox);
            this.Controls.Add(this.fromComboBox);
            this.Controls.Add(this.dayLabel);
            this.Controls.Add(this.toLabel);
            this.Controls.Add(this.fromLabel);
            this.Name = "FlightBookingForm";
            this.Text = "Flight Booking Form";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }

    public class Flight
    {
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int Duration { get; set; }
        public decimal Cost { get; set; }

        public override string ToString()
        {
            return $"{FlightCode} - {Airline} from {From} to {To} on {Day} at {Time}, Duration: {Duration} minutes, Cost: {Cost:C}";
        }
    }
}


