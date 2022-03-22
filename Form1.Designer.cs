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

namespace Apportionment
{
    partial class Apportionment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Apportionment));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Participants = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Population = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quota = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Apportioned = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Items = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Instructions = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Snow;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Participants,
            this.Population,
            this.Quota,
            this.Apportioned});
            this.dataGridView1.Location = new System.Drawing.Point(379, 22);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(553, 416);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            // 
            // Participants
            // 
            this.Participants.HeaderText = "Participants";
            this.Participants.MinimumWidth = 6;
            this.Participants.Name = "Participants";
            this.Participants.ReadOnly = true;
            this.Participants.Width = 125;
            // 
            // Population
            // 
            this.Population.HeaderText = "Population";
            this.Population.MinimumWidth = 6;
            this.Population.Name = "Population";
            this.Population.ReadOnly = true;
            this.Population.Width = 125;
            // 
            // Quota
            // 
            this.Quota.HeaderText = "Quota";
            this.Quota.MinimumWidth = 6;
            this.Quota.Name = "Quota";
            this.Quota.ReadOnly = true;
            this.Quota.Width = 125;
            // 
            // Apportioned
            // 
            this.Apportioned.HeaderText = "Apportioned";
            this.Apportioned.MinimumWidth = 6;
            this.Apportioned.Name = "Apportioned";
            this.Apportioned.ReadOnly = true;
            this.Apportioned.Width = 125;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Custom",
            "Representitives Apportioned 2020",
            "Representitives Apportioned 2010",
            "Representitives Apportioned 2000",
            "Representitives Apportioned 1990"});
            this.comboBox1.Location = new System.Drawing.Point(12, 22);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(245, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(282, 22);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(91, 24);
            this.UpdateButton.TabIndex = 4;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Number of Items to be apportioned:";
            // 
            // Items
            // 
            this.Items.Location = new System.Drawing.Point(282, 54);
            this.Items.MaxLength = 10;
            this.Items.Name = "Items";
            this.Items.Size = new System.Drawing.Size(91, 22);
            this.Items.TabIndex = 7;
            this.Items.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Items_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(199, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Number of seats:               N/A";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(167, 116);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(90, 30);
            this.AddButton.TabIndex = 9;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(282, 116);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(90, 30);
            this.RemoveButton.TabIndex = 10;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(61, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 30);
            this.label3.TabIndex = 11;
            this.label3.Text = "Rows : ";
            // 
            // Instructions
            // 
            this.Instructions.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Instructions.Location = new System.Drawing.Point(12, 174);
            this.Instructions.Name = "Instructions";
            this.Instructions.Size = new System.Drawing.Size(360, 264);
            this.Instructions.TabIndex = 13;
            this.Instructions.Text = resources.GetString("Instructions.Text");
            // 
            // Apportionment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 455);
            this.Controls.Add(this.Instructions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Items);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Apportionment";
            this.Text = "Hamilton\'s Method of Apportionment ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private ComboBox comboBox1;
        private Button UpdateButton;
        private Label label1;
        private TextBox Items;
        private Label label2;
        private Button AddButton;
        private Button RemoveButton;
        private DataGridViewTextBoxColumn Participants;
        private DataGridViewTextBoxColumn Population;
        private DataGridViewTextBoxColumn Quota;
        private DataGridViewTextBoxColumn Apportioned;
        private Label label3;
        private Label Instructions;
    }
}

