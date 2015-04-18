using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Learning.Genetic;

namespace Othello.Learning.Neural
{
    class NeuralNetwork
    {
        public const int NUM_INPUTS  = 72;
        public const int NUM_OUTPUTS = 36;
        public const int NUM_HIDDEN = 100;

        private Neuron[] hiddenLayer;
        public Neuron[] outputLayer;

        public NeuralNetwork(NeuronGenome[] hiddenGenomes)
        {
            hiddenLayer = new Neuron[NUM_HIDDEN];
            outputLayer = new Neuron[NUM_OUTPUTS];

            int hPos = 0;
            LinkedList<double>[] outWeights = new LinkedList<double>[NUM_OUTPUTS];
            foreach (NeuronGenome hidden in hiddenGenomes)
            {
                double[] inWeights = new double[NUM_INPUTS];
                foreach (NeuronGene gene in hidden.inputGenes)
                {
                    inWeights[gene.connectedIndex] = gene.weight;
                }
                hiddenLayer[hPos++] = new Neuron(inWeights);

                foreach (NeuronGene gene in hidden.outputGenes)
                {
                    if (outWeights[gene.connectedIndex] == null)
                    {
                        outWeights[gene.connectedIndex] = new LinkedList<double>();
                    }
                    outWeights[gene.connectedIndex].AddLast(gene.weight);
                }
            }

            int oPos = 0;
            foreach (LinkedList<double> weights in outWeights)
            {
                outputLayer[oPos++] = new Neuron(weights.ToArray());
            }
        }

        public double[] update(double[] input)
        {
            double[] hiddenOutput = updateHelper(input, hiddenLayer);
            return updateHelper(hiddenOutput, outputLayer);
        }

        private double[] updateHelper(double[] input, Neuron[] output)
        {
            double[] outputVals = new double[output.Length];
         
            for(int i = 0; i < outputVals.Length; i++)
            {
                outputVals[i] = output[i].evaluateInput(input);
            }

            return outputVals;
        }
    }

    struct Neuron
    {
        public double[] weights;

        public Neuron(double[] weights)
        {
            this.weights = weights;
        }

        public double evaluateInput(double[] input)
        {
            double sum = 0;
            int pos = 0;
            foreach(double weight in weights)
            {
                sum += weight*input[pos++];
            }
            return logisitc(sum);
        }

        private double logisitc (double x)
        {
            return 1.0/(1 + Math.Exp(-x));
        }
    }
}