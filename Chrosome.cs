using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 功率能耗计算模块
{
    class Chrosome
    {
        private const int NVARS = 4;
        private double PMUTATION;

        private double[] gene = new double[NVARS];
        private double[] upper = new double[NVARS];
        private double[] lower = new double[NVARS];
        private double fitness;
        private double rFitness;
        private double cFitness;
        private Random myRandom;

        public Chrosome(double tempPMUTATION)
        {
            PMUTATION = tempPMUTATION;
            myRandom = new Random(unchecked((int)DateTime.Now.Ticks));
        }

        public double Fitness
        {
            get { return fitness; }
            set { fitness = value; }
        }

        public double RFitness
        {
            get { return rFitness; }
            set { rFitness = value; }
        }

        public double CFitness
        {
            get { return cFitness; }
            set { cFitness = value; }
        }

        public double GetIOneGen(int id)
        {
            return this.gene[id];
        }
        public void SetIOneGen(int id, double tempGen)
        {
            this.gene[id] = tempGen;
        }

        public double GetIOneLower(int id)
        {
            return this.lower[id];
        }
        public void SetIOneLower(int id, double tempLower)
        {
            this.lower[id] = tempLower;
        }

        public double GetIOneUpper(int id)
        {
            return this.upper[id];
        }
        public void SetIOneUpper(int id, double tempUpper)
        {
            this.upper[id] = tempUpper;
        }

        /*********************************************************
      * 参数：分别表示上下界
      * 返回值：doule
      * 功能：产生介于[low,high)的随机数
      * *********************************************************/
        public double RandValue(double low, double high)
        {
            double val;
            val = (double)myRandom.Next(0, 1000);
            val = val / 1000.0 * (high - low) + low;
            return val;
        }

        /*********************************************************
      * 参数：无
      * 返回值：无
      * 功能：单个染色体变异
      * *********************************************************/
        public void ChrosomeMution()
        {
            double lbound, hbound, x;
            for (int i = 0; i < NVARS; i++)
            {
                x = myRandom.Next(0, 1000) / 1000.0;
                if (x < PMUTATION)
                {
                    lbound = this.lower[i];
                    hbound = this.upper[i];
                    this.SetIOneGen(i, RandValue(lbound, hbound));
                }
            }
        }

        /*********************************************************
     * 参数：无
     * 返回值：无
     * 功能：单个染色体评价适应度
     * *********************************************************/
        public void ChrosomeEvaluate()
        {
            double[] x = new double[NVARS + 1];
            for (int i = 0; i < NVARS; i++)
                x[i + 1] = this.GetIOneGen(i);
            double R = x[2] * x[3] * x[4] / 60;
            this.fitness = (5.1175 + 7.7875 * x[1] / R / 60 + 478.797 / R);
        }

        /*********************************************************
     * 参数：模板基因染色体
     * 返回值：无
     * 功能：复制染色体基因
     * *********************************************************/
        public void CopyAllGene(Chrosome tempChrosome)
        {
            for (int i = 0; i < NVARS; i++)
                this.gene[i] = tempChrosome.GetIOneGen(i);
        }

        /*********************************************************
     * 参数：模板基因染色体
     * 返回值：无
     * 功能：复制整个染色体
     * *********************************************************/
        public void CopyChrosome(Chrosome tempChrosome)
        {
            for (int i = 0; i < NVARS; i++)
                this.gene[i] = tempChrosome.GetIOneGen(i);
            for (int i = 0; i < NVARS; i++)
                this.upper[i] = tempChrosome.GetIOneUpper(i);
            for (int i = 0; i < NVARS; i++)
                this.lower[i] = tempChrosome.GetIOneLower(i);
            this.fitness = tempChrosome.Fitness;
            this.rFitness = tempChrosome.RFitness;
            this.cFitness = tempChrosome.CFitness;
        }
    }
}
