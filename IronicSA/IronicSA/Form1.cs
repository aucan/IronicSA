using SVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IronicSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRegression_Click(object sender, EventArgs e)
        {
            indexing inx = new indexing(@"C:\Users\Alaattin\Source\Repos\IronicSA\IronicSA\IronicSA\data\tweetdata.txt");
            inx.IndexingData();
            string datafolder = inx.getMatrixes(5);
            double total = 0;
            for (int i = 0; i < 5; i++)
            {
                total += TrainAndTest(datafolder, i);
            }
        }

        private double TrainAndTest(string datafolder, int i)
        {
              Parameter parameters = new Parameter();
              parameters.SvmType = SvmType.NU_SVR;
              Problem train = Problem.Read(datafolder + "Train" + i.ToString() + ".txt");
              Model model = Training.Train(train, parameters);
              Problem test = Problem.Read(datafolder + "Test" + i.ToString() + ".txt");
            return Prediction.Predict(test, datafolder + "result" + i.ToString() + ".txt", model, false);
        }
    }
}
