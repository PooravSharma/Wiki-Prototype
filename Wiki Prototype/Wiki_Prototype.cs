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
        static int coloumSize = 12;
        static int item = 0;
        int name = 0, category = 1, structure = 2, definition = 3;

        //8.1	Create a global 2D string array, use static variables for the dimensions(row, column),
        string[,] wikiArray = new string[rowSize, coloumSize];
        string currentFileName = "definition.dat";

        #region Buttons
        //8.2	Create an ADD button that will store the information from the 4 text boxes into the 2D array,
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

        //8.9	Create a LOAD button that will read the information from a binary file called definitions.dat into the 2D array
        private void buttonLoad_Click(object sender, EventArgs e)
        {// Open the text file using a stream reader.

            string fileName = currentFileName;
            OpenFileDialog OpenText = new OpenFileDialog();
            DialogResult sr = OpenText.ShowDialog();
            fileName = OpenText.FileName;
            autoLoad(fileName);


        }
        //8.8Create a SAVE button so the information from the 2D array can be written into a binary file called definitions.dat which is sorted by Name
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Sort();
            string fileName = currentFileName;
            SaveFileDialog SaveText = new SaveFileDialog();
            DialogResult sr = SaveText.ShowDialog();
            SaveText.Filter = "Binary Files | *.dat";
            SaveText.DefaultExt = "dat";
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
                using (Stream stream = File.Open(currentFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    for (int y = 0; y < item; y++)
                    {
                        for (int x = 0; x < 4; x++)
                        {
                            bin.Serialize(stream, wikiArray[y, x]);
                        }
                    }
                }
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
                while (selected < item - 1)
                {
                    wikiArray[selected, name] = wikiArray[selected + 1, name];
                    wikiArray[selected, category] = wikiArray[selected + 1, category];
                    wikiArray[selected, structure] = wikiArray[selected + 1, structure];
                    wikiArray[selected, definition] = wikiArray[selected + 1, definition];
                    selected++;
                }

                wikiArray[selected, name] = "";
                wikiArray[selected, category] = "";
                wikiArray[selected, structure] = "";
                wikiArray[selected, definition] = "";
                Clear();

                Display();

                item--;
            }

            else
            {
                MessageBox.Show("Nothing is selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            textBoxSearch.Focus();
            Sort();
        }

        //8.7Create a method so the user can select a definition (Name) from the Listbox and all the information is displayed in the appropriate Textboxes
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
        //8.5	Write the code for a Binary Search for the Name in the 2D array and display the information in the other textboxes when found, add suitable feedback if the search in not successful and clear the search textbox (do not use any built-in array methods)
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Sort();

            if (!string.IsNullOrEmpty(textBoxSearch.Text))
            {
                string target = textBoxSearch.Text;
                int min = 0;
                int max = item - 1;
                int mid = 0;
                bool found = false;

                while (min <= max)
                {
                    mid = (min + max) / 2;
                    if (string.Compare(wikiArray[mid, name], target) == 0)
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
                    arrayBox.SelectedIndex = mid;
                    MessageBox.Show("The target was found at element[" + mid + "]", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    DisplayBox(mid);


                }
                else
                {
                    MessageBox.Show("The target was Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // The program must generate an error message if the search is not successful. 
                }
                textBoxSearch.Clear();
            }

            else
            {
                MessageBox.Show("Search Box is Empty", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            Sort();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxName.Text) && !string.IsNullOrEmpty(textBoxCategory.Text) && !string.IsNullOrEmpty(textBoxStructure.Text) && !string.IsNullOrEmpty(textBoxDefinition.Text))
            {
                if (arrayBox.SelectedItem != null)
                {

                    int element = arrayBox.SelectedIndex;

                    wikiArray[element, name] = textBoxName.Text;
                    wikiArray[element, category] = textBoxCategory.Text;
                    wikiArray[element, structure] = textBoxStructure.Text;
                    wikiArray[element, definition] = textBoxDefinition.Text;
                    MessageBox.Show("Edit complete", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Select from array box", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Display();
            Clear();
            textBoxSearch.Focus();
        }


        private void buttonReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < item; i++)
            {
                wikiArray[i, name] = "";
                wikiArray[i, category] = "";
                wikiArray[i, structure] = "";
                wikiArray[i, definition] = "";
            }
            Clear();

            Display();

            item--;
        }

        private void Wiki_Prototype_Load(object sender, EventArgs e)
        {
            autoLoad("definition.dat");
        }

        #endregion

        #region Methods
        private void autoLoad(string fileName)
        {
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
                    }


                }

                Display();
            }
            catch (IOException)
            {
                MessageBox.Show("File counld not be openned");
            }
        }

        //8.6 Create a display method that will show the following information in a List box: Name and Category
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

            wikiArray[0, name] = "Array";
            wikiArray[0, category] = "Array";
            wikiArray[0, structure] = "Linear";
            wikiArray[0, definition] = "A list of finite numbers of elements stored in the memory. In a linear array, we can store only homogeneous data elements. Elements of the array form a sequence or linear list, that can have the same type of data. Each element of the array is referred by an index set.";

            wikiArray[1, name] = "Two Dimension Array";
            wikiArray[1, category] = "Array";
            wikiArray[1, structure] = "Linear";
            wikiArray[1, definition] = "A two-dimensional array could be considered to have “rows” and “columns”. The declaration of a two- dimensional array is extension of the declaration for a 1-D (linear) array. The first dimension is the “row” and the second is the “column”.";

            wikiArray[2, name] = "List";
            wikiArray[2, category] = "List";
            wikiArray[2, structure] = "Linear";
            wikiArray[2, definition] = "A list is an abstract data type that represents a finite number of ordered values, where the same value may occur more than once. An instance of a list is a computer representation of the mathematical concept of a tuple or finite sequence; the infinite analogy of a list is a stream.";

            wikiArray[3, name] = "Linked list";
            wikiArray[3, category] = "List";
            wikiArray[3, structure] = "Linear";
            wikiArray[3, definition] = "A linked list is a linear collection of data elements whose order is not given by their physical placement in memory. Instead, each element points to the next. It is a data structure consisting of a collection of nodes which together represent a sequence.";

            wikiArray[4, name] = "Self-Balance Tree";
            wikiArray[4, category] = "Tree";
            wikiArray[4, structure] = "Non-Linear";
            wikiArray[4, definition] = "A self-balancing binary search tree (BST) is any node-based tree that automatically keeps its height (maximal number of levels below the root) small in the face of arbitrary item insertions and deletions.";

            wikiArray[5, name] = "Heap";
            wikiArray[5, category] = "Tree";
            wikiArray[5, structure] = "Non-Linear";
            wikiArray[5, definition] = "A heap is a specialized tree-based data structure which is essentially an almost complete tree that satisfies the heap property: in a max heap, for any given node C, if P is a parent node of C, then the key (the value) of P is greater than or equal to the key to C.";

            wikiArray[6, name] = "Binary Search Tree";
            wikiArray[6, category] = "Tree";
            wikiArray[6, structure] = "Non-Linear";
            wikiArray[6, definition] = "Is a rooted binary tree data structure whose internal nodes each store a key greater than all the keys in the node's left subtree and less than those in its right subtree.";

            wikiArray[7, name] = "Graph";
            wikiArray[7, category] = "Graph";
            wikiArray[7, structure] = "Non-Linear";
            wikiArray[7, definition] = "A graph is a pictorial representation of a set of objects where some pairs of objects are connected by links. The interconnected objects are represented by points termed as vertices, and the links that connect the vertices are called edges.";

            wikiArray[8, name] = "Set";
            wikiArray[8, category] = "Abstract";
            wikiArray[8, structure] = "Non-Linear";
            wikiArray[8, definition] = "A set is a data structure that can store any number of unique values in any order you so wish. Sets are different from arrays in the sense that they only allow non-repeated, unique values within them.";

            wikiArray[9, name] = "Queue";
            wikiArray[9, category] = "Abstract";
            wikiArray[9, structure] = "Linear";
            wikiArray[9, definition] = "A collection of items in which only the earliest added item may be accessed. Basic operations are added (to the tail) or enqueue and delete (from the head) or dequeue.";

            wikiArray[10, name] = "Stack";
            wikiArray[10, category] = "Abstract";
            wikiArray[10, structure] = "Linear";
            wikiArray[10, definition] = "A stack is a linear data structure that follows the principle of Last in First Out (LIFO). This means the last element inserted inside the stack is removed first. You can think of the stack data structure as the pile of plates on top of another. Stack representation like a pile of plate.";

            wikiArray[11, name] = "Hash Table";
            wikiArray[11, category] = "Hash";
            wikiArray[11, structure] = "Non-Linear";
            wikiArray[11, definition] = "Hash Table is a data structure which stores data in an associative manner. In a hash table, data is stored in an array format, where each data value has its own unique index value. Access of data becomes very fast if we know the index of the desired data.";

            item = 12;
        }



        private void DisplayBox(int x)
        {
            textBoxName.Text = wikiArray[x, name];
            textBoxCategory.Text = wikiArray[x, category];
            textBoxStructure.Text = wikiArray[x, structure];
            textBoxDefinition.Text = wikiArray[x, definition];

        }

        //8.4 Write the code for a Bubble Sort method to sort the 2D array by Name ascending
        private void Sort()
        {
            try
            {



                for (int i = 0; i < item; i++)
                {

                    for (int j = 1; j < item - i; j++)
                    {

                        if (char.ToLower(wikiArray[j - 1, name][0]) > char.ToLower(wikiArray[j, name][0]))
                        {
                            bubbleSwap(j);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Array not filled");
            }



            Display();
        }

        //8.4 Ensure you use a separate swap method that passes (by reference) the array element to be swapped (do not use any built-in array methods)
        private void bubbleSwap(int j)
        {
            string[] temp = new string[4];

            temp[0] = wikiArray[j - 1, 0];
            temp[1] = wikiArray[j - 1, 1];
            temp[2] = wikiArray[j - 1, 2];
            temp[3] = wikiArray[j - 1, 3];



            wikiArray[j - 1, 0] = wikiArray[j, 0];
            wikiArray[j - 1, 1] = wikiArray[j, 1];
            wikiArray[j - 1, 2] = wikiArray[j, 2];
            wikiArray[j - 1, 3] = wikiArray[j, 3];

            wikiArray[j, 0] = temp[0];
            wikiArray[j, 1] = temp[1];
            wikiArray[j, 2] = temp[2];
            wikiArray[j, 3] = temp[3];

        }

        private void Add(string s1, string s2, string s3, string s4)
        {
            try
            {
                wikiArray[item, name] = s1;
                wikiArray[item, category] = s2;
                wikiArray[item, structure] = s3;
                wikiArray[item, definition] = s4;
                item++;
                Display();
                Clear();
            }
            catch
            {
                MessageBox.Show("Array Box is full");
            }

        }


        #endregion

        //8.3	Create a CLEAR method to clear the four text boxes so a new definition can be added
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