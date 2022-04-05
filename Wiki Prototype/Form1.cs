using System.Runtime.Serialization.Formatters.Binary;

namespace Wiki_Prototype
{
    public partial class Wiki_Prototype : Form
    {
        public Wiki_Prototype()
        {
            InitializeComponent();
        }
        static int rowSize = 15, item = 0;
        static int coloumSize = 4;
        int name = 0, category = 1, structure = 2, definition = 3;
        string[,] wikiArray = new string[rowSize, coloumSize];
        string currentFileName = "definition_00.txt";
        
        #region Buttons
        private void buttonAdd_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrEmpty(textBoxCategory.Text) && !string.IsNullOrEmpty(textBoxStructure.Text) && !string.IsNullOrEmpty(textBoxDefinition.Text))
            { ;

                wikiArray[item, name] = textBoxName.Text;
                wikiArray[item, category] = textBoxCategory.Text;
                wikiArray[item, structure] = textBoxCategory.Text;
                wikiArray[item, definition] = textBoxCategory.Text;
                item++;
                Display();
                Clear();
            }
            else
            {
                MessageBox.Show("Please fill all the textboxes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string fileName = "definition_01.txt";
            OpenFileDialog OpenText = new OpenFileDialog();
            DialogResult sr = OpenText.ShowDialog();
            OpenText.Filter = "Text Files | *.txt";
            OpenText.DefaultExt = "txt";
            if (sr == DialogResult.OK)
            {
                fileName = OpenText.FileName;
            }
            currentFileName = fileName;
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int y = 0; y < coloumSize; y++)
                    {
                        for (int x = 0; x < rowSize; x++)
                        {
                            wikiArray[x, y] = Convert.ToString(value: bin.Deserialize(stream));
                        }
                    }
                }

                Display();
            }
            catch (IOException)
            {
                MessageBox.Show("File counld not be openned");
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string fileName = "definiton_01.txt";
            SaveFileDialog SaveText = new SaveFileDialog();
            DialogResult sr = SaveText.ShowDialog();
            SaveText.Filter = "Text Files | *.txt";
            SaveText.DefaultExt = "txt";
            if (sr == DialogResult.OK)
            {
                fileName = SaveText.FileName;

            }
            if (sr == DialogResult.Cancel)
            {
                SaveText.FileName = fileName;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(fileName, false))
                {
                    foreach (var definiton in wikiArray)
                    {
                        writer.WriteLine(definiton);
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File NOT saved");
            }
        }

        private void buttonAutofill_Click(object sender, EventArgs e)
        {
            Tofill();
            Display();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {

                listBox.SetSelected(listBox.SelectedIndex, true);
                listBox.Items.RemoveAt(listBox.SelectedIndex);
                wikiArray[listBox.SelectedIndex, 1] = wikiArray[item - 1, 1];
                item--;

                Clear();
                Display();


            }

            else
            {
                MessageBox.Show("Nothing is selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            textBoxSearch.Focus();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                string dataItem = listBox.SelectedItem.ToString();
                int dataItemIndex = listBox.FindString(dataItem);
                textBoxSearch.Text = wikiArray[dataItemIndex, 1].ToString();
                textBoxSearch.Focus();

            }
            else
            {
                MessageBox.Show("Please select from the List Box", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Methods
        private void Display()
        {
            listBox.Items.Clear();
            for (int x = 0; x < rowSize; x++)
            {
                string oneLine = "";
                for (int y = 0; y < 2; y++)
                {
                    oneLine = wikiArray[x, y] + "   " + wikiArray[x, y];
                }
                listBox.Items.Add(oneLine);
            }

        }
        private void Clear()
        {
            textBoxCategory.Clear();
            textBoxDefinition.Clear();
            textBoxName.Clear();
            textBoxStructure.Clear();
            textBoxSearch.Clear();
        }
        private void Focus()
        {
            textBoxCategory.Focus();
            textBoxDefinition.Focus();
            textBoxName.Focus();
            textBoxStructure.Focus();
            textBoxSearch.Focus();
        }
        private void Tofill()
        {

            wikiArray[0, 0] = "hi";
            wikiArray[0, 1] = "hi";
            wikiArray[0, 2] = "hi";
            wikiArray[0, 3] = "hi";
            item = 1;
        }
        #endregion

    }
}