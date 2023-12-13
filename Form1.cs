using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Web_Cam_App
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection capture;
        private VideoCaptureDevice videoSource;

        public Form1()
        {
            InitializeComponent();
            ApplyCustomStyles();
            InitializeCameraList();
        }

        private void ApplyCustomStyles()
        {
            ApplyButtonStyle(buttonStart);
            ApplyButtonStyle(buttonCapture);
            ApplyButtonStyle(buttonSave);
            ApplyButtonStyle(buttonExit);
        }

        private void ApplyButtonStyle(Button button)
        {
            button.BackColor = Color.FromArgb(0, 123, 255);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
        }

        private void InitializeCameraList()
        {
            capture = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo deviceList in capture)
            {
                comboBoxWebcamList.Items.Add(deviceList.Name);
            }
            comboBoxWebcamList.SelectedIndex = 0;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    StopCamera();
                }

                var selectedDevice = capture[comboBoxWebcamList.SelectedIndex];
                videoSource = new VideoCaptureDevice(selectedDevice.MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;

                StartCamera();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting the camera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartCamera()
        {
            SetStatusMessage("Starting camera...", true);

            videoSource.Start();

            SetStatusMessage("Camera started successfully!", false);
        }

        private void StopCamera()
        {
            SetStatusMessage("Stopping camera...", true);

            videoSource.SignalToStop();
            videoSource.WaitForStop();

            SetStatusMessage("Camera stopped successfully!", false);
        }

        private void SetStatusMessage(string message, bool isProcessing)
        {
            toolStripStatusLabel.Text = message;
            toolStripProgressBar.Visible = isProcessing;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs e)
        {
            pictureBox1.Image = (Bitmap)e.Frame.Clone();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Save Image As";
                saveFileDialog.Filter = "Image files (*.jpg, *.png) | *.jpg; *.png";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string ext = System.IO.Path.GetExtension(saveFileDialog.FileName);
                        ImageFormat imageFormat = (ext == ".jpg") ? ImageFormat.Jpeg : ImageFormat.Png;
                        pictureBox2.Image.Save(saveFileDialog.FileName, imageFormat);

                        MessageBox.Show("Image saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                StopCamera();
            }

            Application.Exit();
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Code for label1 Click event
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Code for pictureBox1 Click event
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Code for pictureBox2 Click event
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Code for Form1 Load event
        }
    }
}
