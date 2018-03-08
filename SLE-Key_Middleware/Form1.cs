using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SLE_Key_Middleware
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Solutions_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 NewForm = new Form1();
            NewForm.Show();
            this.Dispose(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            // Safety Check for plates and limit the size of the run!
            int NumberOfPlatesInRun = 0;
            List<string> AllIchipsUsed = new List<string>();
            string[] Plate1Ichips = { Plate1Chip1.Text, Plate1Chip2.Text, Plate1Chip3.Text, Plate1Chip4.Text };
            string[] Plate2Ichips = { Plate2Chip1.Text, Plate2Chip2.Text, Plate2Chip3.Text, Plate2Chip4.Text };
            string[] Plate3Ichips = { Plate3Chip1.Text, Plate3Chip2.Text, Plate3Chip3.Text, Plate3Chip4.Text };
            string[] Plate4Ichips = { Plate4Chip1.Text, Plate4Chip2.Text, Plate4Chip3.Text, Plate4Chip4.Text };

            // Is plate 4 all blank
            // See if slide values exist smartgit test
            IsPlateEmpty a = new IsPlateEmpty();
            string plate4exists = a.CheckForBlankPlate(Plate4Ichips);

            if (plate4exists == "True")
            {
                MessageBox.Show("Plate 4 is NOT Blank");

                // See if slide values are unique to plate
                if (Plate4Ichips.Distinct().Count() != Plate4Ichips.Count())
                {
                    MessageBox.Show("Slide ID has been entered twice in Plate 4, please review");
                    // quit program 
                    return;
                }
            }
            
            

            // Is plate 3 all blank
            // See if slide values exist
            IsPlateEmpty b = new IsPlateEmpty();
            string plate3exists = b.CheckForBlankPlate(Plate3Ichips);

            if (plate3exists == "True")
            {
                MessageBox.Show("Plate 3 is NOT Blank");
                // See if slide values are unique to plate
                if (Plate3Ichips.Distinct().Count() != Plate3Ichips.Count())
                {
                    MessageBox.Show("Slide ID has been entered twice in Plate 3, please review");
                    // quit program 
                    return;
                }
            }

            // Is plate 2 all blank
            // See if slide values exist
            IsPlateEmpty j = new IsPlateEmpty();
            string plate2exists = j.CheckForBlankPlate(Plate2Ichips);

            if (plate2exists == "True")
            {
                plate2exists = "True";
                MessageBox.Show("Plate 2 is NOT Blank");
                //see if slide values are unique to plate
                if (Plate2Ichips.Distinct().Count() != Plate2Ichips.Count())
                {
                    MessageBox.Show("Slide ID has been entered twice in Plate 2, please review");
                    // quit program 
                    return;
                }
            }
            
            // Is plate 1 all blank
            // See if slide values exist
            IsPlateEmpty d = new IsPlateEmpty();
            string plate1exists = d.CheckForBlankPlate(Plate1Ichips);

            if (plate1exists == "True")
            {
                MessageBox.Show("Plate 1 is NOT Blank");
                //see if slide values are unique to plate
                if (Plate1Ichips.Distinct().Count() != Plate1Ichips.Count())
                {
                    MessageBox.Show("Slide ID has been entered twice in Plate 1, please review");
                    // quit program 
                    return;
                }
            }
            

            // Cross check all samples for plate#exist="True"
            List<string> allslides = new List<string>();
            if (plate1exists == "True") 
            {
                foreach (string chip in Plate1Ichips){ allslides.Add(chip); }
            }
            if (plate2exists == "True")
            {
                foreach (string chip in Plate2Ichips) { allslides.Add(chip); }
            }
            if (plate3exists == "True")
            {
                foreach (string chip in Plate3Ichips) { allslides.Add(chip); }
            }
            if (plate4exists == "True")
            {
                foreach (string chip in Plate4Ichips) { allslides.Add(chip); }
            }
            if(allslides.Distinct().Count()!=allslides.Count())
            {
                MessageBox.Show("A Chip is used in more than one plate");
                return;
            }
            // load all values into a list , see if list.Distinct().Count()!=List.Count()

            // Check if plate 1 ichips exist, 
            if (String.IsNullOrEmpty(Plate1Chip1.Text)
                || String.IsNullOrEmpty(Plate1Chip2.Text)
                || String.IsNullOrEmpty(Plate1Chip3.Text)
                || String.IsNullOrEmpty(Plate1Chip4.Text))
            {
                // Message that says "Missing Ichip ID's, update form and retry"Read samples and make plate1
                MessageBox.Show("Missing Ichip ID's, update form and retry");
                // quit program
                return;
            }
            else
            {
                //Title line is constant!
                string TitleLine = "iChipLot,Slide_ID,Well_ID,Plate,TestSession,SampleID,Analyze,Run_Number,SampleClass,Aliqout_ID,Operator,Date";

                // see if every entry in the array of chips is unique
                if (Plate1Ichips.Distinct().Count() != Plate1Ichips.Count())
                {
                    MessageBox.Show("Slide ID has been entered twice, please review");
                    // quit program 
                    return;
                }

                //make array of the samples
                string[] Plate1Samples ={Plate1Sample1.Text,Plate1Sample2.Text,Plate1Sample3.Text,Plate1Sample4.Text,
                                       Plate1Sample5.Text,Plate1Sample6.Text,Plate1Sample7.Text,Plate1Sample8.Text};
                //Read samples and make plate1
                string IchipLot = null;
                string IchipSlideID = null;

                // Make Directory Structure
                string rootpath = @"c:\TestRun\" + RunNumber.Text;
                try { DirectoryInfo root = Directory.CreateDirectory(rootpath); }
                catch (Exception f) { MessageBox.Show("The process failed: {0}", f.ToString()); }
                string rawimagespath = rootpath + "\\Raw";
                try { DirectoryInfo raw = Directory.CreateDirectory(rawimagespath); }
                catch (Exception g) { MessageBox.Show("The process failed: {0}", g.ToString()); }
                string outputpath = rootpath + "\\Out";
                try { DirectoryInfo outdir = Directory.CreateDirectory(outputpath); }
                catch (Exception h) { MessageBox.Show("The process failed: {0}", h.ToString()); }
                // Start Metadata sheet
                string mdpath = rootpath + "\\" + RunNumber.Text + ".csv";
                if (!File.Exists(mdpath))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(mdpath))
                    {
                        sw.WriteLine(TitleLine);
                    }
                }

                //Plate = 1, TestSession =1, 

                string[] IchipLot_SlideID = null;
                foreach (string c in Plate1Ichips)
                {
                    //split ichipID apart on '_'
                    IchipLot_SlideID = c.Split('_');
                    IchipLot = IchipLot_SlideID[0];
                    IchipSlideID = IchipLot_SlideID[1];
                    int wellID = 0;

                    foreach (string s in Plate1Samples)
                    {
                        //get sample class
                        SampleClassEvalution n = new SampleClassEvalution();
                        string SC = n.EvaluateSampleID(s);
                        IsSampleIDNull o = new IsSampleIDNull();
                        string RunAnalysis = o.CheckForBlankSampleID(s);
                        wellID = wellID + 1;

                        //make string to add to metadatasheet
                        string newline =
                            IchipLot + "," //iChipLot
                            + IchipSlideID + "," //Slide_ID
                            + wellID.ToString() + "," //well_ID
                            + "1" + "," //Plate
                            + "1" + "," //TestSession
                            + s + "," //SampleID
                            + RunAnalysis + "," //Analyze 
                            + RunNumber.Text + "," //Run_Number
                            + SC + "," //SampleClass
                            + s + ',' //Aliqout_ID
                            + Operator.Text + ','
                            + dateTimePicker1.Value.ToString();
                        using (StreamWriter sw = File.AppendText(mdpath))
                        {
                            sw.WriteLine(newline);
                        }

                    }
                }
            }
        }

        class SampleClassEvalution
        {
            public string EvaluateSampleID(string SampleID)
            {
                string SampleClass = "1";
                if (SampleID.Trim().StartsWith("NC-"))
                {
                    return SampleClass = "5";
                }
                else if (SampleID.Trim().StartsWith("PC-"))
                {
                    return SampleClass = "3";
                }
                else
                {
                    return SampleClass;
                }
            }
        }
        class IsSampleIDNull
        {
            public string CheckForBlankSampleID(string SampleID)
            {
                string Analyze = "1";
                if (SampleID == "")
                {
                    return Analyze = "0";
                }
                else
                {
                    return Analyze;
                }
            }
        }
        class IsPlateEmpty
        {
            public string CheckForBlankPlate(string[] IchipsInPlate)
            {
                string exist = "False";

                if (!String.IsNullOrEmpty(IchipsInPlate[0])
                || !String.IsNullOrEmpty(IchipsInPlate[1])
                || !String.IsNullOrEmpty(IchipsInPlate[2])
                || !String.IsNullOrEmpty(IchipsInPlate[3]))
                {
                    return exist = "True";
                }
                else
                {
                    return exist;
                }
            }
        }
    }
}
