using Bayesian_Classifier.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian_Classifier.Services
{
    public class Perceptron
    {
        private List<Sample> SampleListTrainerFormattedZero { get; set; }
        private List<Sample> SampleListTrainerFormattedOne { get; set; }
        private List<Sample> SampleListTrainerFormattedTwo { get; set; }
        private List<Sample> SampleListTestFormattedZero { get; set; }
        private List<Sample> SampleListTestFormattedOne { get; set; }
        private List<Sample> SampleListTestFormattedTwo { get; set; }
        public List<Sample> SampleListFormatted { get; set; }
        private List<List<double>> AverageList { get; set; }
        private List<List<double>> StandardDeviationList { get; set; }


        public Random Random { get; set; } = new Random();
        public Perceptron(List<Sample> sampleTrainerListZero, List<Sample> sampleTestListZero, List<Sample> sampleTrainerListOne,
            List<Sample> sampleTestListOne, List<Sample> sampleTrainerListTwo, List<Sample> sampleTestListTwo)
        {
            this.SampleListTrainerFormattedZero = sampleTrainerListZero;
            this.SampleListTrainerFormattedOne = sampleTrainerListOne;
            this.SampleListTrainerFormattedTwo = sampleTrainerListTwo;

            this.SampleListTestFormattedZero = sampleTestListZero;
            this.SampleListTestFormattedOne = sampleTestListOne;
            this.SampleListTestFormattedTwo = sampleTestListTwo;

            this.AverageList = new List<List<double>>();
            this.StandardDeviationList = new List<List<double>>();

            this.SetAverageStandardDeviationList();
        }

        private void SetAverageStandardDeviationList()
        {
            SetAverageAndStandardDeviationByClass(0, this.SampleListTestFormattedZero);
            SetAverageAndStandardDeviationByClass(1, this.SampleListTestFormattedOne);
            SetAverageAndStandardDeviationByClass(2, this.SampleListTestFormattedTwo);

        }

        private void SetAverageAndStandardDeviationByClass(int sampleClass, List<Sample> sampleTestListClass)
        {
            int inputLength = sampleTestListClass[sampleClass].CordX.Length;
            double sampleStandardDeviation = 0;
            double sampleAverage = 0;
            this.AverageList.Add(new List<double>());
            this.StandardDeviationList.Add(new List<double>());

            for (int i = 0; i < inputLength; i++)
            {
                foreach (var sample in sampleTestListClass)
                {
                    sampleAverage += sample.CordX[i];
                }

                sampleAverage /= inputLength;

                foreach (var sample in sampleTestListClass)
                {
                    sampleStandardDeviation += (Math.Pow(sample.CordX[i] - sampleAverage, 2) / inputLength);
                }
                this.AverageList[sampleClass].Add(sampleAverage);
                this.StandardDeviationList[sampleClass].Add(Math.Sqrt(sampleStandardDeviation));
            }
        }

        public int ExecuteClassification(Sample sample)
        {
            double class1 = CalculateProbability(sample, 0);
            double class2 = CalculateProbability(sample, 1);
            double class3 = CalculateProbability(sample, 2);

            if (class1 > class2 && class1 > class3)
                return 1;
            else if (class2 > class3)
                return 2;
            else
                return 3;
        }

        private double CalculateProbability(Sample sample, int classType)
        {
            double r = GaussProbabilityDensityDistribution(sample, classType) * GetP(classType);

            return r;
        }

        private double GetP(int classType)
        {

            double baseSize = this.GetTrainingBaseSize();
            double filteredBaseSize = this.GetTrainingBaseByClass(classType).Count;

            return filteredBaseSize / baseSize;
        }

        private double GetTrainingBaseSize()
        {
            return this.SampleListFormatted.Count;
        }

        private double GaussProbabilityDensityDistribution(Sample sample, int classType)
        {
            double r = 1;
            classType -= 1;

            if (classType < 0)
                return r;

            for (int i = 0; i < sample.CordX.Length; i++)
            {
                double mean = this.AverageList[classType][i];
                double sd = this.StandardDeviationList[classType][i];
                r *= GaussProbabilityDensity(sample.CordX[i], mean, sd);
            }

            return r;
        }

        private double GaussProbabilityDensity(double value, double mean, double sd)
        {
            double r = 1 / Math.Sqrt(2 * Math.PI * sd);
            r *= Math.Exp(-(Math.Pow(value - mean, 2) / (2 * Math.Pow(sd, 2))));
            return r;
        }

        public List<Sample> GetTrainingBaseByClass(int classType)
        {
            switch (classType)
            {
                case 0:
                    return SampleListTrainerFormattedZero;
                case 1:
                    return SampleListTrainerFormattedOne;
                case 2:
                    return SampleListTrainerFormattedTwo;
                default:
                    return SampleListTrainerFormattedZero;
            }
        }

        public void SetCurrencyBase(List<Sample> sampleTestFullListFormatted)
        {
            this.SampleListFormatted = sampleTestFullListFormatted;
        }
    }

}