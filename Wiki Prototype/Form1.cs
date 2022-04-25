using System.Runtime.Serialization.Formatters.Binary;

namespace Wiki_Prototype
{
    public partial class Wiki_Prototype : Form
    {
        public Wiki_Prototype()
        {
            InitializeComponent();
        }
        static int rowSize = 12;        
        static int coloumSize = 4;
        static int item = 0;
        int name = 0, category = 1, structure = 2, definition = 3;
        string[,] wikiArray = new string[rowSize, coloumSize];
        string currentFileName = "definition.bin";

        #region Buttons
        private void buttonAdd_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrEmpty(textBoxCategory.Text) && !string.IsNullOrEmpty(textBoxStructure.Text) && !string.IsNullOrEmpty(textBoxDefinition.Text))
            {
                Add(textBoxName.Text, textBoxCategory.Text, textBoxStructure.Text, textBoxDefinition.Text);
                
            }
            else
            {
                MessageBox.Show("Please fill all the textboxes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
             
            try
            {
                using (Stream stream = File.Open(currentFileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int i = 0; i < rowSize; i++)
                    {
                        string[] temp = new string[4];
                        for (int j = 0; j < coloumSize; j++)
                        {
                            wikiArray[i, j] = (string)bin.Deserialize(stream);
                           // temp[j] = (string)bin.Deserialize(stream);
                            // Add(temp[0], temp[1], temp[2], temp[3]);                        
                        }
                        item++;
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
            string fileName = currentFileName;

            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Create))
                {
                    BinaryWriter bwriter = new BinaryWriter(stream);
                   for (int i=0 ; i < item ; i++)
                    {
                        bwriter.Write(wikiArray[i,name]);
                        bwriter.Write(wikiArray[i,category]);
                        bwriter.Write(wikiArray[i,structure]);
                        bwriter.Write(wikiArray[i,definition]);
                    }
                }
                MessageBox.Show("Array saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void buttonAutofill_Click(object sender, EventArgs e)
        {
            Tofill();
            Display();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (arrayBox.SelectedIndex != -1)
            {

                int selected = arrayBox.SelectedIndex;
                wikiArray[selected, name] = "";
                wikiArray[selected, category] = "";
                wikiArray[selected, structure] = "";
                wikiArray[selected, definition] = "";
                Clear();
                //Sort():
                Display();

                item--;
            }

            else
            {
                MessageBox.Show("Nothing is selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            textBoxSearch.Focus();
        }

        private void arrayBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arrayBox.SelectedIndex != -1)
            {
                int element = arrayBox.SelectedIndex;
                DisplayBox(element);
                textBoxSearch.Focus();

            }
            else
            {
                MessageBox.Show("Please select from the array Box", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Sort();

            if (!string.IsNullOrEmpty(textBoxSearch.Text))
            {
                string target = textBoxSearch.Text;
                int min = 0;
                int max = item-1;
                int mid = 0;
                bool found = false;

                while (min <= max)
                {
                    mid = (min + max) / 2;
                    if (string.Compare(wikiArray[mid,name], target) == 0)
                    {
                        found = true;
                        break;
                    }
                    else if (string.Compare(wikiArray[mid, name], target) > 0)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                if (found)
                {
                    MessageBox.Show("The target was found at element[" + mid + "]", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBoxSearch.Clear();
                    DisplayBox(mid);

                }
                else
                {
                    MessageBox.Show("The target was Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // The program must generate an error message if the search is not successful. 
                }
            }

            else{ 
                MessageBox.Show("Search Box is Empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            Sort();
        }


        #endregion

        #region Methods
        private void Load()
        {
            string fileName = currentFileName;
            try
            {
                using (Stream stream = File.Open(fileName, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int i = 0; i < 12; i++)
                    {
                        string[] temp = new string[4];
                        for (int j = 0; j < 4; j++)
                        {
                            temp[j] = (string)bin.Deserialize(stream);
                        }
                        Add(temp[0], temp[1], temp[2], temp[3]);
                        item++;
                    }
                }

                Display();
            }
            catch (IOException)
            {
                MessageBox.Show("File counld not be openned");
            }
        }

        private void Display()
        {
            arrayBox.Items.Clear();
            for (int x = 0; x < rowSize; x++)
            {
                string Name = "";
                string Category = "";

                Name = wikiArray[x, name];
                Category = wikiArray[x, category];
                String rowAdd = Name + "                             " + Category;
                arrayBox.Items.Add(rowAdd);

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

        private void Tofill()
        {

            wikiArray[0, 0] = "hi";
            wikiArray[0, 1] = "hi";
            wikiArray[0, 2] = "hi";
            wikiArray[0, 3] = "hi";
            item++;
        }

      

        private void DisplayBox(int x)
        {
            textBoxName.Text = wikiArray[x, name];
            textBoxCategory.Text = wikiArray[x, category];
            textBoxStructure.Text = wikiArray[x, structure];
            textBoxDefinition.Text = wikiArray[x, definition];

        }


        private void Sort()
        {
            string[] temp = new string[4];

            //char.ToLower(wikiArray[j, name][0] <;
            for (int i = 0; i < item ; i++)
            {                 
                
                for (int j = 0; j < item - 1; j++)
                {

                    if (char.ToLower(wikiArray[i, name][0]) < char.ToLower(wikiArray[j, name][0])){
                       
                        temp[0] = wikiArray[j, 0];
                        temp[1] = wikiArray[j, 1];
                        temp[2] = wikiArray[j, 2];
                        temp[3] = wikiArray[j, 3];

                        wikiArray[j, 0] = wikiArray[j + 1, 0];
                        wikiArray[j, 1] = wikiArray[j + 1, 1];
                        wikiArray[j, 2] = wikiArray[j + 1, 2];
                        wikiArray[j, 3] = wikiArray[j + 1, 3];

                        wikiArray[j + 1, 0] = temp[0];
                        wikiArray[j + 1, 1] = temp[1];
                        wikiArray[j + 1, 2] = temp[2];
                        wikiArray[j + 1, 3] = temp[3];

                    }
                }
            }
            Display();
        }

       

        private void Add(string s1, string s2, string s3, string s4)
        {
           
                wikiArray[item, name] = s1;
                wikiArray[item, category] = s2;
                wikiArray[item, structure] = s3;
                wikiArray[item, definition] = s4;
                item++;
                Display();
                Clear();
          
        }

        
        #endregion

        #region Mouseclick
        private void textBoxName_DoubleClick(object sender, EventArgs e)
        {
            textBoxName.Clear();
        }

        private void textBoxCategory_DoubleClick(object sender, EventArgs e)
        {
            textBoxCategory.Clear();
           
        }
        private void textBoxStructure_DoubleClick(object sender, EventArgs e)
        {
            
            textBoxStructure.Clear();
           
        }
        private void textBoxDefinition_DoubleClick(object sender, EventArgs e)
        {
            textBoxDefinition.Clear();            
        }
        #endregion
    }
}