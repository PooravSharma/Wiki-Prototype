namespace Wiki_Prototype
{
    public partial class Wiki_Prototype : Form
    {
        public Wiki_Prototype()
        {
            InitializeComponent();
        }
        static int rowSize = 12, item = 0;
        static int coloumSize = 4;
        int name = 0, category = 1, structure = 2, definition = 3;        
        string[,] wikiArray = new string[rowSize,coloumSize];
        string currentFileName = "definition_00.txt";
        #region Buttons
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrEmpty(textBoxCategory.Text) && !string.IsNullOrEmpty(textBoxStructure.Text) && !string.IsNullOrEmpty(textBoxDefinition.Text))
            {

                wikiArray[item, name] = textBoxName.Text;
                wikiArray[item,category] = textBoxCategory.Text;
                wikiArray[item,structure] = textBoxCategory.Text;
                wikiArray[item,definition] = textBoxCategory.Text;
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
                Array.Clear(wikiArray);
                using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
                {
                    while (!reader.EndOfStream)
                    {
                        wikiArray.Add(reader.ReadLine());
                    }
                }
                Display();
            }
            catch (IOException)
            {
                MessageBox.Show("File counld not be openned");
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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


        #endregion


        #region Methods
        private void Display()
        {
            listBox.Items.Clear();
            for (int i = 0; i < wikiArray.Length; i++)
            {

                listBox.Items.Add(wikiArray[i,name] + "\t" + wikiArray[i,category]);
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


        #endregion

    }
}