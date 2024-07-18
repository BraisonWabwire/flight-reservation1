using System;
using System.Windows.Forms;

namespace TravelessReservationSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            welcomeLabel.Text = "Welcome to Traveless";
            welcomeDescription.Text = "Welcome to our new app.";
        }

        private void flightsButton_Click(object sender, EventArgs e)
        {
            // Open FlightBookingForm when flightsButton is clicked
            FlightBookingForm flightBookingForm = new FlightBookingForm();
            flightBookingForm.Show();
        }

        private void reservationsButton_Click(object sender, EventArgs e)
        {
            welcomeLabel.Text = "Reservations";
            welcomeDescription.Text = "Manage your reservations here.";

            ReservationForm reservationForm = new ReservationForm();
            reservationForm.Show();
        }
    }
}
