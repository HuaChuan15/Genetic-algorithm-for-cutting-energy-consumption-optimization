using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 功率能耗计算模块
{
    class Population
    {
        private int POPSIZE;
        private int MAXGENS;
        private double PXOVER;
        private double PMUTATION;

        private const int NVARS = 4;
        private int generation;
        private int curBest;
        private Chrosome[] myChrosome;
        private Chrosome[] newChrosome;
        private Random myRandom;
        private bool isFirstWrite;
        private string readerPath;
        private const string writePath =
         //  @"C:\Users\Chrome\Desktop\myCWindowsProgram\myCWindowsProgram\Write.txt";
         @"Write.txt";
        private const string resultPath =
          //  @"C:\Users\Chrome\Desktop\myCWindowsProgram\myCWindowsProgram\Result.txt";
          @"Result.txt";

        /*
        private const string writePath =
        @"myCWindowsProgram\Write.txt";
        private const string resultPath =
            @"myCWindowsProgram\Result.txt";
        */
        private MyWindows myWindows;
        private double[] lbound;
        private double[] ubound;

        /*********************************************************
       * 参数：无
       * 返回值：无
       * 功能：读入待处理数据集
       * *********************************************************/
        public void ReadDataSet()
        {
            int i = 0;
            lbound = new double[NVARS];
            ubound = new double[NVARS];
            using (StreamReader sr = new StreamReader(readerPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] strcol = line.Split(new char[] { ' ' });
                    lbound[i] = Convert.ToDouble(strcol[0]);
                    ubound[i] = Convert.ToDouble(strcol[1]);
                    i++;
                }
                sr.Close();
            }
        }

        /*********************************************************
       * 参数：分别别表示种群规模、迭代次数、种群交叉率、基因变异率、窗口句柄
       * 返回值：无
       * 功能：初始化种群
       * *********************************************************/
        public Population(int tempPOPSIZE, int tempMAXGENS, double tempPXOVER,
            double tempPMUTATION, string tempReaderPath, MyWindows tempMyWindows)
        {
            POPSIZE = tempPOPSIZE;
            MAXGENS = tempMAXGENS;
            PXOVER = tempPXOVER;
            PMUTATION = tempPMUTATION;
            readerPath = tempReaderPath;
            myWindows = tempMyWindows;
            isFirstWrite = true;
            myRandom = new Random(unchecked((int)DateTime.Now.Ticks));
            myChrosome = new Chrosome[POPSIZE + 1];
            newChrosome = new Chrosome[POPSIZE + 1];
            ReadDataSet();
            for (int i = 0; i <= POPSIZE; i++)
            {
                myChrosome[i] = new Chrosome(PMUTATION);
                newChrosome[i] = new Chrosome(PMUTATION);
            }
            for (int i = 0; i < NVARS; i++)
            {
                for (int j = 0; j < POPSIZE; j++)
                {
                    myChrosome[j].Fitness = 0;
                    myChrosome[j].RFitness = 0;
                    myChrosome[j].CFitness = 0;
                    myChrosome[j].SetIOneLower(i, lbound[i]);
                    myChrosome[j].SetIOneUpper(i, ubound[i]);
                    myChrosome[j].SetIOneGen(i, myChrosome[j].RandValue(
                    myChrosome[j].GetIOneLower(i), myChrosome[j].GetIOneUpper(i)));
                }
            }

        }

        /*********************************************************
       * 参数：无
       * 返回值：无
       * 功能：评价当前种群每个个体的适应度
       * *********************************************************/
        public void PopulationEvaluate()
        {
            for (int i = 0; i < POPSIZE; i++)
                myChrosome[i].ChrosomeEvaluate();
        }

        /*********************************************************
        * 参数：无
        * 返回值：无
        * 功能：精英主义（选择当前种群适应度最好个体存活下来）
        * *********************************************************/
        public void PopulationKeepTheBest()
        {
            curBest = 0;
            for (int i = 0; i < POPSIZE; i++)
            {
                if (myChrosome[i].Fitness > myChrosome[POPSIZE].Fitness)
                {
                    curBest = i;
                    myChrosome[POPSIZE].Fitness = myChrosome[i].Fitness;
                }
            }
            myChrosome[POPSIZE].CopyAllGene(myChrosome[curBest]);
        }

        /*********************************************************
       * 参数：无
       * 返回值：无
       * 功能：遗传算子-精英主义操作（保留最好个体，淘汰最坏个体）
       * *********************************************************/
        public void PopulationElitist()
        {
            double best, worst;
            int bestMem = 0, worstMem = 0;
            best = myChrosome[0].Fitness;
            worst = myChrosome[0].Fitness;
            for (int i = 0; i < POPSIZE - 1; ++i)
            {
                if (myChrosome[i].Fitness < myChrosome[i + 1].Fitness)
                {
                    if (myChrosome[i].Fitness <= best)
                    {
                        best = myChrosome[i].Fitness;
                        bestMem = i;
                    }
                    if (myChrosome[i + 1].Fitness >= worst)
                    {
                        worst = myChrosome[i + 1].Fitness;
                        worstMem = i + 1;
                    }
                }
                else
                {
                    if (myChrosome[i].Fitness >= worst)
                    {
                        worst = myChrosome[i].Fitness;
                        worstMem = i;
                    }
                    if (myChrosome[i + 1].Fitness <= best)
                    {
                        best = myChrosome[i + 1].Fitness;
                        bestMem = i + 1;
                    }
                }
            }

            if (best <= myChrosome[POPSIZE].Fitness)
            {
                myChrosome[POPSIZE].CopyAllGene(myChrosome[bestMem]);
                myChrosome[POPSIZE].Fitness = myChrosome[bestMem].Fitness;
            }
            else
            {
                myChrosome[worstMem].CopyAllGene(myChrosome[POPSIZE]);
                myChrosome[worstMem].Fitness = myChrosome[POPSIZE].Fitness;
            }
        }

        /*********************************************************
         * 参数：无
         * 返回值：无
         * 功能：遗传算子-选择操作（采用轮盘赌方法选择适应度较高个体）
         * *********************************************************/
        public void PopulationSelect()
        {
            double sum = 0;
            for (int i = 0; i < POPSIZE; i++)
            {
                sum += myChrosome[i].Fitness;
            }
            for (int i = 0; i < POPSIZE; i++)
            {
                myChrosome[i].RFitness = myChrosome[i].Fitness / sum;
            }
            myChrosome[0].CFitness = myChrosome[0].RFitness;
            for (int i = 1; i < POPSIZE; i++)
            {
                myChrosome[i].CFitness = myChrosome[i - 1].CFitness + myChrosome[i].RFitness;
            }
            for (int i = 0; i < POPSIZE; i++)
            {
                int val = (int)myRandom.Next(0, 1000);
                double p = val % 1000 / 1000.0;
                if (p < myChrosome[0].CFitness)
                    newChrosome[i].CopyChrosome(myChrosome[0]);
                else
                {
                    for (int j = 0; j < POPSIZE; j++)
                        if (p >= myChrosome[j].CFitness && p < myChrosome[j + 1].CFitness)
                            newChrosome[i].CopyChrosome(myChrosome[j + 1]);
                }
            }
            for (int i = 0; i < POPSIZE; i++)
                myChrosome[i].CopyChrosome(newChrosome[i]);
        }

        /*********************************************************
         * 参数：one，two代表两个带交叉的染色体
         * 返回值：无
         * 功能：交叉子操作（交换两染色体部分基因）
         * *********************************************************/
        public void PopulationXover(int one, int two)
        {
            int point;
            if (NVARS > 1)
            {

                if (NVARS == 2)
                    point = 1;
                else
                    point = ((int)myRandom.Next(0, NVARS - 1)) + 1;
                for (int i = 0; i < point; i++)
                {
                    double temp = myChrosome[one].GetIOneGen(i);
                    myChrosome[one].SetIOneGen(i, myChrosome[two].GetIOneGen(i));
                    myChrosome[two].SetIOneGen(i, temp);
                }

            }
        }

        /*********************************************************
        * 参数：无
        * 返回值：无
        * 功能：遗传算子-交叉操作（依概率选择染色体杂交）
        * *********************************************************/
        public void PopulationCrossover()
        {
            int mem, one = -1;
            int first = 0;
            double x;
            for (mem = 0; mem < POPSIZE; ++mem)
            {
                x = myRandom.Next(0, 1000) / 1000.0;
                if (x < PXOVER)
                {
                    ++first;
                    if (first % 2 == 0)
                        PopulationXover(one, mem);
                    else
                        one = mem;
                }
            }
        }

        /*********************************************************
        * 参数：无
        * 返回值：无
        * 功能：遗传算子-变异操作（基因变异）
        * *********************************************************/
        public void PopulationMutate()
        {
            for (int i = 0; i < POPSIZE; i++)
            {
                myChrosome[i].ChrosomeMution();
            }
        }

        /*********************************************************
        * 参数：无
        * 返回值：无
        * 功能：打印当前种群适应度信息
        * *********************************************************/
        public void PopulationReport()
        {
            double bestVal, avg, stddev, sumSquare, squareSum, sum;
            sum = 0.0;
            sumSquare = 0.0;
            for (int i = 0; i < POPSIZE; i++)
            {
                sum += myChrosome[i].Fitness;
                sumSquare += myChrosome[i].Fitness * myChrosome[i].Fitness;
            }
            avg = sum / (double)POPSIZE;
            squareSum = avg * avg * POPSIZE;
            stddev = Math.Sqrt((sumSquare - squareSum) / (double)(POPSIZE - 1));
            bestVal = myChrosome[POPSIZE].Fitness;
            try
            {
                //保证在第一次读写时覆盖上个数据集的处理结果，同时保证当前结果累加输到文件末尾
                if (isFirstWrite)
                    //保证当前结果累加输到文件末尾
                    using (StreamWriter br = new StreamWriter(writePath, false))
                    {
                        
                        br.WriteLine("generation            best          average          standard");
                        br.WriteLine("number               value          fitness          deviation");
                        br.WriteLine("{0},       {1},      {2},       {3}", generation, bestVal, avg, stddev);
                        br.Close();
                    }
                else
                    //保证在第一次读写时覆盖上个数据集的处理结果
                    using (StreamWriter br = new StreamWriter(writePath, true))
                    {
                        
                        br.WriteLine("{0},       {1},      {2},       {3}", generation, bestVal, avg, stddev);
                        br.Close();
                    }

                //double bestVal = (5.1175 + 7.7875 * x[1] / R / 60 + 478.797 / R);
                this.myWindows.setList(bestVal, 2);
                this.myWindows.setList(avg, 3);
                this.myWindows.setList(sumSquare, 4);
                this.myWindows.setList(squareSum, 5);
                this.myWindows.setList(stddev, 6);
                this.myWindows.setList(myChrosome[POPSIZE].GetIOneGen(0), 7);
                this.myWindows.setList(myChrosome[POPSIZE].GetIOneGen(1), 8);
                this.myWindows.setList(myChrosome[POPSIZE].GetIOneGen(2), 9);
                this.myWindows.setList(myChrosome[POPSIZE].GetIOneGen(3), 10);
                if (isFirstWrite)
                    isFirstWrite = false;
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("已经读到末尾");
            }
        }


        /*********************************************************
       * 参数：无
       * 返回值：无
       * 功能：遗传算法主框架程序
       * *********************************************************/
        public void GeneticAlgorithm()
        {
            DateTime starttime = DateTime.Now;
            TimeSpan timespan;
            this.generation = 0;
            this.myWindows.setProgressBar(0, MAXGENS);
            this.myWindows.setList(1.0 * this.generation, 1);
            this.PopulationEvaluate();
            this.PopulationKeepTheBest();
            while (this.generation < MAXGENS)
            {
                this.generation++;
                this.PopulationSelect();
                this.PopulationCrossover();
                this.PopulationMutate();
                this.PopulationReport();
                this.PopulationEvaluate();
                this.PopulationElitist();
                this.myWindows.setProgressBar(this.generation, MAXGENS);
                this.myWindows.setList(1.0 * this.generation, 1);
            }
            //显示最优解
            try
            {
                using (StreamWriter br = new StreamWriter(writePath, true))
                {
                    br.WriteLine(); br.WriteLine(); br.WriteLine();
                    br.WriteLine("Simulation completed");
                    br.WriteLine("Best member: ");
                    for (int i = 0; i < NVARS; i++)
                    {
                        br.WriteLine("var({0}) ={1} ", i, myChrosome[POPSIZE].GetIOneGen(i));
                    }
                    br.WriteLine("Best fitness = {0}", myChrosome[POPSIZE].Fitness);
                    timespan = DateTime.Now.Subtract(starttime);//获取就是开始时间很结束时间差
                    string diff = timespan.TotalSeconds.ToString();
                    br.WriteLine("the total time of program :{0} Seconds", diff);
                    br.Close();
                }
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("已经读到末尾");
            }

            //显示计算时间
            try
            {
                using (StreamWriter br = new StreamWriter(resultPath, true))
                {

                    br.WriteLine("geration={0}     Best fitness = {1}", MAXGENS, myChrosome[POPSIZE].Fitness);
                    timespan = DateTime.Now.Subtract(starttime);  //获取 开始和结束时间差
                    string diff = timespan.TotalSeconds.ToString();
                    br.WriteLine("the total time of program :{0} Seconds", diff);
                    br.WriteLine(); br.WriteLine(); br.WriteLine();
                    br.Close();
                }
            }
            catch (EndOfStreamException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("已经读到末尾");
            }
        }
    }
}
