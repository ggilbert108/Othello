using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Othello.Auxillary;
using Othello.Learning.Neural;

namespace Othello.Learning.Genetic
{
    static class GeneticSimulator
    {
        private static NeuronGenome[] neuronPopulation;
        private static BlueprintGenome[] blueprintPopulation;

        static GeneticSimulator()
        {
            neuronPopulation = new NeuronGenome[1000];
            blueprintPopulation = new BlueprintGenome[100];
        }
    }

    struct NeuronGenome
    {
        public LinkedList<NeuronGene> inputGenes;
        public LinkedList<NeuronGene> outputGenes;

        public NeuronGenome(int numInputs, int numOutputs)
        {
            inputGenes = new LinkedList<NeuronGene>();
            outputGenes = new LinkedList<NeuronGene>();
            initGenes(numInputs, inputGenes);
            initGenes(numOutputs, outputGenes);
        }

        public NeuronGenome(NeuronGene[] allGenes, int numInputs, int numOutputs)
        {
            inputGenes = new LinkedList<NeuronGene>();
            outputGenes = new LinkedList<NeuronGene>();
            int pos = 0;
            for (int i = 0; i < numInputs; i++)
            {
                inputGenes.AddLast(allGenes[pos++]);
            }
            for (int i = 0; i < numOutputs; i++)
            {
                outputGenes.AddLast(allGenes[pos++]);
            }
        }

        public static List<NeuronGenome> getCrossover(NeuronGenome a, NeuronGenome b)
        {
            LinkedList<NeuronGene> acg = a.getCompleteGenome();
            LinkedList<NeuronGene> bcg = b.getCompleteGenome();
            int crossoverPoint = Util.random.Next(acg.Count);

            NeuronGene[] c1Genes = new NeuronGene[acg.Count];
            NeuronGene[] c2Genes = new NeuronGene[acg.Count];
            acg.CopyTo(c1Genes, 0);
            bcg.CopyTo(c1Genes, crossoverPoint);
            bcg.CopyTo(c2Genes, 0);
            acg.CopyTo(c2Genes, crossoverPoint);
            NeuronGenome childOne = new NeuronGenome(c1Genes, a.inputGenes.Count, a.outputGenes.Count);
            NeuronGenome childTwo = new NeuronGenome(c1Genes, a.inputGenes.Count, a.outputGenes.Count);
            return new List<NeuronGenome>()
            {
                childOne, childTwo
            };
        }

        private void initGenes(int n, LinkedList<NeuronGene> genes)
        {
            LinkedList<int> available = Util.getShuffledConsecutive(n);
            while(available.Count >= 0)
            {
                NeuronGene gene = new NeuronGene()
                {
                    connectedIndex = available.First(),
                    weight = SimpleRNG.GetNormal(0, 1)
                };
                available.RemoveFirst();
                genes.AddLast(gene);
            }
        }

        public LinkedList<NeuronGene> getCompleteGenome()
        {
            LinkedList<NeuronGene> complete = new LinkedList<NeuronGene>();
            foreach (NeuronGene gene in inputGenes)
            {
                complete.AddLast(gene);
            }
            foreach (NeuronGene gene in outputGenes)
            {
                complete.AddLast(gene);
            }
            return complete;
        }
    }

    struct NeuronGene
    {
        public int connectedIndex;
        public double weight;
    }

    struct BlueprintGenome
    {
        public int[] neuronIndexes;
 
        public BlueprintGenome(int dummy)
        {
            neuronIndexes = new int[NeuralNetwork.NUM_HIDDEN];
        }
    }
}