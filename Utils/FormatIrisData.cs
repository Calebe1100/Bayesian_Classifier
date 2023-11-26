using Bayesian_Classifier.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Bayesian_Classifier.Utils
{
    public class FormatIrisData
    {
        private static readonly double[] CLASS_DEFAULT = new double[] { 0, 0, 0 };
        private static double[] CLASS_ONE = new double[] { 0, 0, 1 };
        private static readonly double[] CLASS_TWO = new double[] { 0, 1, 0 };
        private static readonly double[] CLASS_THREE = new double[] { 1, 0, 0 };
        private List<Sample> SampleListFormatted { get; set; }
        public List<Sample> SampleTrainingFullListFormatted { get; set; } = new List<Sample>();
        public List<Sample> SampleTestFullListFormatted { get; set; } = new List<Sample>();
        public List<Sample> SampleListTrainerFormattedZero { get; set; } = new List<Sample>();
        public List<Sample> SampleListTrainerFormattedOne { get; set; } = new List<Sample>();
        public List<Sample> SampleListTrainerFormattedTwo { get; set; } = new List<Sample>();
        public List<Sample> SampleListTestFormattedZero { get; set; } = new List<Sample>();
        public List<Sample> SampleListTestFormattedOne { get; set; } = new List<Sample>();
        public List<Sample> SampleListTestFormattedTwo { get; set; } = new List<Sample>();
        private List<Sample> ClassZero { get; set; }
        private List<Sample> ClassOne { get; set; }
        private List<Sample> ClassTwo { get; set; }

        public FormatIrisData(List<string[]> dataReading)
        {
            List<Sample> sampleList = Initialize(dataReading.Count, dataReading[0].Length);



            Sample sample;
            for (int i = 0; i < dataReading.Count; i++)
            {
                sample = new Sample(4, 1);
                for (int j = 0; j < dataReading[i].Length; j++)
                {
                    if (j <= 3)
                    {
                        sample.CordX[j] = Double.Parse(dataReading[i][j], CultureInfo.InvariantCulture);
                    }

                    if (j > 3)
                    {

                        sample.CordY = GetOutputValueByPlantType(dataReading[i][j]);
                    }
                }
                sampleList.Add(sample);
            }
            this.SampleListFormatted = sampleList;

            ClassZero = this.SetBaseClass(CLASS_ONE);
            ClassOne = this.SetBaseClass(CLASS_TWO);
            ClassTwo = this.SetBaseClass(CLASS_THREE);

            this.SetSampleListTrainerFormatted();
            this.SetSampleListTestFormatted();

        }

        private void SetSampleListTestFormatted()
        {
            this.SampleListTestFormattedZero.AddRange(this.ClassZero.GetRange((int)(this.ClassZero.Count * 0.7) + 1, this.ClassZero.Count - ((int)(this.ClassZero.Count * 0.7) + 1)));
            this.SampleListTestFormattedOne.AddRange(this.ClassOne.GetRange((int)(this.ClassOne.Count * 0.7) + 1, this.ClassOne.Count - ((int)(this.ClassZero.Count * 0.7) + 1)));
            this.SampleListTestFormattedTwo.AddRange(this.ClassTwo.GetRange((int)(this.ClassTwo.Count * 0.7) + 1, this.ClassTwo.Count - ((int)(this.ClassZero.Count * 0.7) + 1)));
            this.SampleTestFullListFormatted.AddRange(this.SampleListTestFormattedZero);
            this.SampleTestFullListFormatted.AddRange(this.SampleListTestFormattedOne);
            this.SampleTestFullListFormatted.AddRange(this.SampleListTestFormattedTwo);
        }

        private void SetSampleListTrainerFormatted()
        {
            this.SampleListTrainerFormattedZero.AddRange(this.ClassZero.GetRange(0, (int)(this.ClassZero.Count * 0.7)));
            this.SampleListTrainerFormattedOne.AddRange(this.ClassOne.GetRange(0, (int)(this.ClassOne.Count * 0.7)));
            this.SampleListTrainerFormattedTwo.AddRange(this.ClassTwo.GetRange(0, (int)(this.ClassTwo.Count * 0.7)));
            this.SampleTrainingFullListFormatted.AddRange(this.SampleListTrainerFormattedZero);
            this.SampleTrainingFullListFormatted.AddRange(this.SampleListTrainerFormattedOne);
            this.SampleTrainingFullListFormatted.AddRange(this.SampleListTrainerFormattedTwo);
        }

        private List<Sample> SetBaseClass(double[] cordY)
        {
            return this.SampleListFormatted.Where(s => s.CordY == cordY).ToList();
        }
        private double[] GetOutputValueByPlantType(string value)
        {
            switch (value)
            {
                case "Iris-setosa":
                    return CLASS_ONE;

                case "Iris-versicolor":
                    return CLASS_TWO;
                case "Iris-virginica":
                    return CLASS_THREE;
                default:
                    return CLASS_DEFAULT;
            }
        }

        private List<Sample> Initialize(int count, int length)
        {
            return new List<Sample>(count);
        }

        public int GetOutputFormatted(Sample sample)
        {
            if (sample.CordY == CLASS_ONE)
            {
                return 0;
            }
            else if (sample.CordY == CLASS_TWO)
            {
                return 1;
            }
            else
            {
                return 2;
            }

        }
    }
}

