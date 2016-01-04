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

            string dataset = inx.getMatrixesFull();
            TrainAndTest(dataset, dataset, dataset.Replace("dataset","result"));

            /* 
            string datafolder = inx.getMatrixes(5);
            double total = 0;
            for (int i = 0; i < 5; i++)
            {
                total += TrainAndTestFold(datafolder, i);
            }
            */
        }

        private double TrainAndTest(string trainSet,string testSet, string resultFile)
        {
            Parameter parameters = new Parameter();
            parameters.SvmType = SvmType.NU_SVR;
            Problem train = Problem.Read(trainSet);
            Model model = Training.Train(train, parameters);
            Problem test = Problem.Read(testSet);
            return Prediction.Predict(test, resultFile, model, true);
        }

        private double TrainAndTestFold(string datafolder, int i)
        {
            string trainSet = datafolder + "Train" + i.ToString() + ".txt";
            string testSet = datafolder + "Test" + i.ToString() + ".txt";
            string resultFile = datafolder + "result" + i.ToString() + ".txt";
            return TrainAndTest(trainSet, testSet, resultFile); 
        }
    }
}
