using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileConverterJIO1ToJIO2
{
    public partial class Form1 : Form
    {
        string oldFormatText = "";
        Dictionary<string, string> OldFormatDict = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Open();
            SaveAs();
        }
        private void Open()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Journal I/O File (*.jour)|*.jour";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                oldFormatText = File.ReadAllText(openFileDialog1.FileName.ToString());
                OldFormatDict = formatJournalDict(File.ReadAllText(openFileDialog1.FileName.ToString()));
            }
        }
        public Dictionary<string, string> formatJournalDict(string unformatted)// to be turned into dictionary from file 
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            string pat = "{(.*?):(.*?)}";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(unformatted);
            while (m.Success)
            {
                string key = "";
                string value = "";
                for (int i = 1; i <= 2; i++)
                {
                    Group g = m.Groups[i];
                    if (i == 1)
                    {
                        key = g.Value;
                    }
                    else if (i == 2)
                    {
                        value = g.Value;
                    }
                }
                toReturn.Add(key, value);
                m = m.NextMatch();
            }
            return toReturn;
        }
        public string formatForSave()
        {
            string formatted = "";
            foreach (KeyValuePair<string, string> kvp in OldFormatDict)
            {
                formatted += kvp.Key;
                formatted += " : ";
                formatted += kvp.Value;
                formatted += "\n";
            }

            return formatted;
        }
        public void SaveAs()
        {
            string Contents = formatForSave();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Journal I/O File (*.jour)|*.jour"; //txt files (*.txt)|*.txt|All files (*.*)|*.*
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {


                System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName.ToString());
                using (StringReader reader = new StringReader(Contents))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            file.WriteLine(line);
                        }

                    } while (line != null);
                }
                file.Close();
            }


        }
    }
}
