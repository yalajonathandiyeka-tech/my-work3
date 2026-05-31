using System;
using System.Drawing;
using System.Windows.Forms;
using ETS.Models;

namespace ETS.Forms
{
    public class AdminForm : Form
    {
        // Tab control
        private TabControl tabControl;
        private TabPage tabEvents, tabUsers;

        // Event tab controls
        private DataGridView dgvEvents;
        private TextBox txtEventName;
        private NumericUpDown nudPrice, nudTickets;
        private Button btnAddEvent, btnEditEvent, btnDeleteEvent, btnClearEvent;
        private Label lblEventName, lblPrice, lblTickets;

        // User tab controls
        private DataGridView dgvUsers;
        private TextBox txtUserName, txtUserEmail;
        private NumericUpDown nudBalance;
        private Button btnAddUser, btnClearUser;
        private Label lblUserName, lblUserEmail, lblBalance;

        // Header
        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnLogout;

        public AdminForm()
        {
            InitializeComponent();
            LoadEvents();
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "ETS – Administrator Panel";
            this.Size = new Size(820, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Header
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(30, 60, 120) };
            lblTitle = new Label
            {
                Text = "Administrator Panel",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnLogout = new Button
            {
                Text = "Logout",
                Width = 90, Height = 30,
                Top = 15, Left = 700,
                BackColor = Color.FromArgb(200, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => this.Close();
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnLogout);

            // Tab Control
            tabControl = new TabControl
            {
                Left = 10, Top = 70,
                Width = 780, Height = 510,
                Font = new Font("Segoe UI", 10)
            };

            tabEvents = new TabPage("Event Management");
            tabUsers = new TabPage("User Management");

            BuildEventTab();
            BuildUserTab();

            tabControl.TabPages.Add(tabEvents);
            tabControl.TabPages.Add(tabUsers);

            this.Controls.Add(pnlHeader);
            this.Controls.Add(tabControl);
        }

        // ─────────────── EVENT TAB ───────────────
        private void BuildEventTab()
        {
            tabEvents.BackColor = Color.FromArgb(248, 249, 252);

            // DataGridView
            dgvEvents = new DataGridView
            {
                Left = 10, Top = 10, Width = 740, Height = 260,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };
            dgvEvents.SelectionChanged += DgvEvents_SelectionChanged;

            // Input fields
            lblEventName = MakeLabel("Event Name:", 10, 285);
            txtEventName = MakeTextBox(120, 283, 240);

            lblPrice = MakeLabel("Price (R):", 380, 285);
            nudPrice = new NumericUpDown
            {
                Left = 460, Top = 283, Width = 110, Height = 28,
                Minimum = 0, Maximum = 100000, DecimalPlaces = 2,
                Font = new Font("Segoe UI", 9)
            };

            lblTickets = MakeLabel("Tickets:", 590, 285);
            nudTickets = new NumericUpDown
            {
                Left = 655, Top = 283, Width = 95, Height = 28,
                Minimum = 0, Maximum = 10000,
                Font = new Font("Segoe UI", 9)
            };

            // Buttons
            btnAddEvent = MakeButton("Add Event", 10, 325, Color.FromArgb(30, 120, 60));
            btnEditEvent = MakeButton("Save Edit", 170, 325, Color.FromArgb(30, 80, 160));
            btnDeleteEvent = MakeButton("Delete", 330, 325, Color.FromArgb(180, 40, 40));
            btnClearEvent = MakeButton("Clear", 490, 325, Color.FromArgb(100, 100, 120));

            btnAddEvent.Click += BtnAddEvent_Click;
            btnEditEvent.Click += BtnEditEvent_Click;
            btnDeleteEvent.Click += BtnDeleteEvent_Click;
            btnClearEvent.Click += (s, e) => ClearEventFields();

            tabEvents.Controls.AddRange(new Control[]
            {
                dgvEvents,
                lblEventName, txtEventName,
                lblPrice, nudPrice,
                lblTickets, nudTickets,
                btnAddEvent, btnEditEvent, btnDeleteEvent, btnClearEvent
            });
        }

        private void LoadEvents()
        {
            dgvEvents.DataSource = null;
            dgvEvents.DataSource = AppData.Events;
            dgvEvents.Columns["EventName"].HeaderText = "Event Name";
            dgvEvents.Columns["Price"].HeaderText = "Price (R)";
            dgvEvents.Columns["AvailableTickets"].HeaderText = "Available Tickets";
            StyleGrid(dgvEvents);
        }

        private void DgvEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEvents.SelectedRows.Count > 0)
            {
                var ev = (Event)dgvEvents.SelectedRows[0].DataBoundItem;
                txtEventName.Text = ev.EventName;
                nudPrice.Value = ev.Price;
                nudTickets.Value = ev.AvailableTickets;
            }
        }

        private void BtnAddEvent_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEventName.Text))
                    throw new Exception("Event name cannot be empty.");

                var ev = new Event(txtEventName.Text.Trim(), nudPrice.Value, (int)nudTickets.Value);
                AppData.Events.Add(ev);
                LoadEvents();
                ClearEventFields();
                MessageBox.Show("Event added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditEvent_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEvents.SelectedRows.Count == 0)
                    throw new Exception("Please select an event to edit.");
                if (string.IsNullOrWhiteSpace(txtEventName.Text))
                    throw new Exception("Event name cannot be empty.");

                var ev = (Event)dgvEvents.SelectedRows[0].DataBoundItem;
                ev.EventName = txtEventName.Text.Trim();
                ev.Price = nudPrice.Value;
                ev.AvailableTickets = (int)nudTickets.Value;
                LoadEvents();
                ClearEventFields();
                MessageBox.Show("Event updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDeleteEvent_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEvents.SelectedRows.Count == 0)
                    throw new Exception("Please select an event to delete.");

                var ev = (Event)dgvEvents.SelectedRows[0].DataBoundItem;
                var confirm = MessageBox.Show($"Delete '{ev.EventName}'?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    AppData.Events.Remove(ev);
                    LoadEvents();
                    ClearEventFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearEventFields()
        {
            txtEventName.Clear();
            nudPrice.Value = 0;
            nudTickets.Value = 0;
            dgvEvents.ClearSelection();
        }

        // ─────────────── USER TAB ───────────────
        private void BuildUserTab()
        {
            tabUsers.BackColor = Color.FromArgb(248, 249, 252);

            dgvUsers = new DataGridView
            {
                Left = 10, Top = 10, Width = 740, Height = 280,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };

            lblUserName = MakeLabel("Name:", 10, 305);
            txtUserName = MakeTextBox(80, 303, 200);

            lblUserEmail = MakeLabel("Email:", 300, 305);
            txtUserEmail = MakeTextBox(360, 303, 200);

            lblBalance = MakeLabel("Balance (R):", 580, 305);
            nudBalance = new NumericUpDown
            {
                Left = 668, Top = 303, Width = 82, Height = 28,
                Minimum = 0, Maximum = 1000000, DecimalPlaces = 2,
                Font = new Font("Segoe UI", 9)
            };

            btnAddUser = MakeButton("Add User", 10, 345, Color.FromArgb(30, 120, 60));
            btnClearUser = MakeButton("Clear", 170, 345, Color.FromArgb(100, 100, 120));

            btnAddUser.Click += BtnAddUser_Click;
            btnClearUser.Click += (s, e) => ClearUserFields();

            tabUsers.Controls.AddRange(new Control[]
            {
                dgvUsers,
                lblUserName, txtUserName,
                lblUserEmail, txtUserEmail,
                lblBalance, nudBalance,
                btnAddUser, btnClearUser
            });
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = AppData.Members;
            dgvUsers.Columns["Name"].HeaderText = "Full Name";
            dgvUsers.Columns["Email"].HeaderText = "Email";
            dgvUsers.Columns["Balance"].HeaderText = "Balance (R)";
            StyleGrid(dgvUsers);
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text))
                    throw new Exception("Name cannot be empty.");
                if (string.IsNullOrWhiteSpace(txtUserEmail.Text))
                    throw new Exception("Email cannot be empty.");

                var member = new Member(txtUserName.Text.Trim(), txtUserEmail.Text.Trim(), nudBalance.Value);
                AppData.Members.Add(member);
                LoadUsers();
                ClearUserFields();
                MessageBox.Show("Member added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUserFields()
        {
            txtUserName.Clear();
            txtUserEmail.Clear();
            nudBalance.Value = 0;
        }

        // ─────────────── HELPERS ───────────────
        private Label MakeLabel(string text, int left, int top)
        {
            return new Label
            {
                Text = text, Left = left, Top = top,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 80)
            };
        }

        private TextBox MakeTextBox(int left, int top, int width)
        {
            return new TextBox
            {
                Left = left, Top = top, Width = width, Height = 28,
                Font = new Font("Segoe UI", 9)
            };
        }

        private Button MakeButton(string text, int left, int top, Color back)
        {
            var btn = new Button
            {
                Text = text, Left = left, Top = top,
                Width = 145, Height = 38,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 120);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 255);
            dgv.RowTemplate.Height = 28;
        }
    }
}
