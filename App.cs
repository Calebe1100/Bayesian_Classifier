using Bayesian_Classifier.Entities;
using Bayesian_Classifier.Services;
using Bayesian_Classifier.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bayesian_Classifier
{
    public class App
    {
        public static void Main()
        {

            var dataReading = DataReading.ReadingAndGenerateInputText("../../Files/iris.data");

            FormatIrisData generateFormatted = new FormatIrisData(dataReading);

            Perceptron execPerceptron = new Perceptron(generateFormatted.SampleListTestFormattedZero, generateFormatted.SampleListTestFormattedOne, generateFormatted.SampleListTestFormattedTwo,
                generateFormatted.SampleListTrainerFormattedZero, generateFormatted.SampleListTrainerFormattedOne, generateFormatted.SampleListTrainerFormattedTwo);


            execPerceptron.SetCurrencyBase(generateFormatted.SampleTrainingFullListFormatted);
            int trainingBaseClassificationError = 0;
            foreach (var item in generateFormatted.SampleTrainingFullListFormatted)
            {
                int classification = execPerceptron.ExecuteClassification(item);

                if (classification != generateFormatted.GetOutputFormatted(item))
                {
                    trainingBaseClassificationError++;
                }

               
            }

            execPerceptron.SetCurrencyBase(generateFormatted.SampleTestFullListFormatted);
            int testBaseClassificationError = 0;
            foreach (var item in generateFormatted.SampleTestFullListFormatted)
            {
                int classification = execPerceptron.ExecuteClassification(item);

                if (classification != generateFormatted.GetOutputFormatted(item))
                {
                    testBaseClassificationError++;
                }
            }

            Console.WriteLine("Erro de classificação base treino: "+ trainingBaseClassificationError);
            Console.WriteLine("Erro de classificação base teste: "+ testBaseClassificationError);

        }
    }
}
