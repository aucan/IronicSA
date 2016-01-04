﻿using SVM;
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
            string results = dataset.Replace("dataset", "results");
            TrainAndTest(dataset, dataset, results);            

            string datafolder = inx.getMatrixes(5);
            double total = 0;
            for (int i = 0; i < 5; i++)
            {
                total += TrainAndTestFold(datafolder, i);
            }
        }

        private double TrainAndTest(string trainSet,string testSet, string resultFile)
        {            
            Problem train = Problem.Read(trainSet);
            Problem test = Problem.Read(testSet);
            Parameter parameters = new Parameter();
            parameters.C = 5;
            parameters.Gamma = 2;
            parameters.SvmType = SvmType.NU_SVR;
            Model model = Training.Train(train, parameters);
            return Prediction.Predict(test, resultFile, model, false);
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
