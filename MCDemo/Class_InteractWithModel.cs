using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MC
{
    public class InteractWithModel
    {
        public struct modelStruct
        {
            public int id;
            public string name;
            public string version;

            public modelStruct(int idToUse, string nameToUse, string versionToUse)
            {
                id = idToUse;
                name = nameToUse;
                version = versionToUse;
            }

        };

        public static List<modelStruct> availableModels;
       

        public static void whatModel()
        {
            string r;
            int s;

            addAvailableModels();

            Console.WriteLine();
            Console.WriteLine("**************************************");
            foreach(modelStruct item in availableModels)
            {
                Console.WriteLine("Enter {0} for {1} version {2} : ", item.id, item.name, item.version);
            }
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.model = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, I'm running PERSiST 1.4");
            }
        }

        public static void runModel(string commandString)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + commandString);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                //
                // not really pretty but should tag the problem
                //
                if (result.Length > 1)
                {
                    Console.WriteLine(result);
                    File.WriteAllText("error.lst", result);
                    Environment.Exit(-1);
                }
            }
            catch
            {
                Console.WriteLine("Could not run model");
            }
        }

        protected static double returnNS(string s)
        {
            //note that things may behave in a very odd way if NS crosses 0.
            //Maximizing NS-1 will prevent this from happenning
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = val-1;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

        static double returnLogNS(string s)
        {
            //note that things may behave in a very odd way if NS crosses 0.
            //Maximizing NS-1 will prevent this from happenning
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = val - 1;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

        static double returnKGE(string s)
        {
            //note that things may behave in a very odd way if KGE crosses 0.
            //Maximizing KGE-1 will prevent this from happenning
            //Console.WriteLine($"{s}");
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = val - 1;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }
        static double returnR2(string s)
        {
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = val - 1.0;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

        static double returnAD(string s)
        {
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = -1.0 * Math.Abs(val);
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }
        
        static double returnRMSE(string s)
        {
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = -1.0 * val;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

        static double returnRE(string s)
        {
            double val;
            try
            {
                val = Convert.ToDouble(s);
                val = -1.0 * Math.Abs(val);
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

        static double returnVR(string s)
        {
            double val;
            try
            {
                val = Convert.ToDouble(s);
                if (val > 1.0) { val = 1.0 / val; }
                val = val - 1.0;
            }
            catch
            {
                val = 0.0;
            }
            return val;

        }

        static double returnCat(string s)
        {
            double val;
            
            try
            {
                // we still have some "-999" codes in the CatB and CatC output
                if (s.Equals("-999"))
                    val = 0.0;
                else
                    val = Convert.ToDouble(s)-1.0;
            }
            catch
            {
                val = 0.0;
            }
            return val;
        }

public static void writePerformanceStatistics(string rowID, string statisticFileName, string summaryFileName)
        {

        }
        
          public static double evaluatePerformanceStatistics()
        {
            string fileName = MCParameters.coefficientsFile;
            {
                double performanceStatistic = 0.0;
                int i = 0;

                try
                {
                    foreach (string line in File.ReadLines(fileName))
                    {

                        string trimLine = line.Trim();
                        string[] values = trimLine.Split(',');

                        try
                        {
                            switch (MCParameters.model)
                            {
                                case 1:     //running PERSiST 1.4
                                case 8:     //running PERSiST 1.6
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[3] * returnLogNS(values[3]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[6] * returnAD(values[6]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnVR(values[7]);
                                    break;
                                case 10:    //running PERSiST 2.0
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[3] * returnLogNS(values[3]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[6] * returnAD(values[6]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnVR(values[7]);
                                    //need to check position of KGE
                                    //performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[11] * returnKGE(values[11]);
                                    break;
                                case 2:  // running INCA-C
                                case 11: //running INCA-C
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    break;
                                case 3: // running INCA-PECo
                                    //needs further refinement
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[3] * returnRMSE(values[3]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[4] * returnRE(values[4]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[5] * returnVR(values[5]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[6] * returnCat(values[6]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnCat(values[7]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[8] * returnCat(values[8]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[9] * returnCat(values[9]);

                                    //Console.WriteLine("i {0} Performance statistic {1:F} Series weight {2} Coefficient Weight {3} NS {4:F}", i,performanceStatistic, MCParameters.seriesWeights[i], MCParameters.coefficientsWeights[2], returnNS(values[2]));
                                    break;
                                case 4: // running INCA-P
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[5] * returnVR(values[5]);
                                    break;
                                case 5: // running INCA-Tox
                                        // need NS + Cat-B + Cat-C
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[6] * returnCat(values[6]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnCat(values[7]);
                                    break;
                                case 6: // running INCA-Path
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[3] * returnLogNS(values[3]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnVR(values[7]);
                                    break;
                                case 7: //running INCA-Hg
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    break;
                                case 9: // running INCA_ON(THE)
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[3] * returnLogNS(values[3]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[6] * returnAD(values[6]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[7] * returnVR(values[7]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[11] * returnKGE(values[11]);
                                    break;
                                case 12: // running INCA-N
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[1] * returnR2(values[1]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[2] * returnNS(values[2]);
                                    performanceStatistic += MCParameters.seriesWeights[i] * MCParameters.coefficientsWeights[5] * returnVR(values[5]);
                                    break;
                                default:
                                    Console.WriteLine("This should not happen - invalid model ID code in performance statistic retrieval");
                                    break;
                            }
                        }
                        catch
                        {
                            performanceStatistic += 0.0;
                        }
                        i++;
                        //Console.WriteLine(performanceStatistic);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); };
               
                return performanceStatistic;
            }
        }

        static double getWeight(string testStatistic)
        {
            double weight = 0.0;
            Console.Write("Please enter a weight to use for {0} : ", testStatistic);
            try { weight = Convert.ToDouble(Console.ReadLine()); }
            catch { Console.WriteLine("Invalid weight, setting value to 0.0"); }
            return weight;
        }

        static void setPERSiSTCoefficientWeights()
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
            MCParameters.coefficientsWeights[3] = getWeight("logNS (log(Nash Sutcliffe))");
            MCParameters.coefficientsWeights[6] = getWeight("AD (Absolute Difference)");
            MCParameters.coefficientsWeights[7] = getWeight("VR (Variance Ratio)");
        }

        static void setINCA_PCoefficientWeights()
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
            MCParameters.coefficientsWeights[5] = getWeight("VR (Variance Ratio)");
        }

        static void setINCA_NCoefficientWeights()
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
            MCParameters.coefficientsWeights[5] = getWeight("VR (Variance Ratio)");
        }
        static void setINCA_PEcoCoefficientWeights() //INCA_PEco
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
            MCParameters.coefficientsWeights[3] = getWeight("RMSE (Root mean square error)");
            MCParameters.coefficientsWeights[4] = getWeight("RE (Relative error)");
            MCParameters.coefficientsWeights[5] = getWeight("VR (Variance Ratio)");
            MCParameters.coefficientsWeights[7] = getWeight("CatB");
            MCParameters.coefficientsWeights[8] = getWeight("CatC");
            MCParameters.coefficientsWeights[9] = getWeight("CatCa");
            MCParameters.coefficientsWeights[10] = getWeight("CatCb");
        }

        static void setINCA_ONTHECoefficientWeights()
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
            MCParameters.coefficientsWeights[3] = getWeight("log(NS) (log Nash Sutcliffe)");
            MCParameters.coefficientsWeights[6] = getWeight("AD (Absolute Difference)");
            MCParameters.coefficientsWeights[7] = getWeight("VR (Variance Ratio)");
            MCParameters.coefficientsWeights[11] = getWeight("KGE (Kling Gupta Efficiency)");
        }

        static void setINCA_CCoefficientWeights()
        {
            MCParameters.coefficientsWeights[1] = getWeight("R2 (Pearson Correlation)");
            MCParameters.coefficientsWeights[2] = getWeight("NS (Nash Sutcliffe)");
        }

        public static void setCoefficientWeights()
        {
            //fix this later, currently placeholder code to add coefficeint weights
            MCParameters.coefficientsWeights = new double[20];
            for (int i = 0; i < 20; i++)
                MCParameters.coefficientsWeights[i] = 1.0;

            Console.WriteLine("Model Number {0}", MCParameters.model);

            switch(MCParameters.model)
            {
                case 1:  //PERSiST 1.4
                case 8:  //PERSiST 1.6
                case 10: //PERSiST 2.0
                    setPERSiSTCoefficientWeights();
                    break;
                case 2: //INCA-C
                    setINCA_CCoefficientWeights();
                    break;
                case 3: //INCA-PECo
                    setINCA_PEcoCoefficientWeights();
                    break;
                case 4: //INCA-P
                    setINCA_PCoefficientWeights();
                    break;
                case 9: //INCA(ON)THE
                    setINCA_ONTHECoefficientWeights();
                    break;
                case 12: // INCA-N Classic
                    setINCA_NCoefficientWeights();
                    break;
                default:
                    break;
            }
        }

        public static void writeCoefficientWeights()
        {
            switch (MCParameters.model)
            {
                case 1:  //PERSiST 1.4
                case 8:  //PERSiST 1.6
                case 10: //PERSiST 2.0
                    writePERSiSTCoefficientWeights();
                    break;
                case 2:
                    writeINCA_CCoefficientWeights();
                    break;
                case 3: //INCA-PEco
                    writeINCA_PEcoCoefficientWeights();
                    break;
                case 4: //INCA-P
                    writeINCA_PCoefficientWeights();
                    break;
                case 9: //INCA(ON)THE
                    writeINCA_ONTHECoefficientWeights();
                    break;
                case 12:
                    writeINCA_NCoefficientWeights();
                    break;
                default:
                    break;
            }
        }

        static void writePERSiSTCoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile,false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("logNS (log(Nash Sutcliffe)), {0}", MCParameters.coefficientsWeights[3]);
                cwf.WriteLine("AD (Absolute Difference), {0}", MCParameters.coefficientsWeights[6]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[7]);
            }
        }

        static void writeINCA_PCoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile, false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[5]);
            }
        }

        static void writeINCA_NCoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile, false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[5]);
            }
        }

        static void writeINCA_CCoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile, false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[5]);
            }
        }
        static void writeINCA_ONTHECoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile, false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("logNS (log(Nash Sutcliffe)), {0}", MCParameters.coefficientsWeights[3]);
                cwf.WriteLine("AD (Absolute Difference), {0}", MCParameters.coefficientsWeights[6]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[7]);
                cwf.WriteLine("KGE (Kling Gupta Efficiency), {0}", MCParameters.coefficientsWeights[11]);
            }
        }


        //will need some editing
        static void writeINCA_PEcoCoefficientWeights()
        {
            //overwrite existing file
            using (StreamWriter cwf = new StreamWriter(MCParameters.coefficientsWeightFile, false))
            {
                cwf.WriteLine("R2 (Pearson Correlation), {0}", MCParameters.coefficientsWeights[1]);
                cwf.WriteLine("NS (Nash Sutcliffe), {0}", MCParameters.coefficientsWeights[2]);
                cwf.WriteLine("RMSE (Root Mean Square Error), {0}", MCParameters.coefficientsWeights[3]);
                cwf.WriteLine("RE (Relative Error), {0}", MCParameters.coefficientsWeights[4]);
                cwf.WriteLine("VR (Variance Ratio), {0}", MCParameters.coefficientsWeights[5]);
                cwf.WriteLine("CatB (CatB), {0}", MCParameters.coefficientsWeights[6]);
                cwf.WriteLine("CatC (CatC), {0}", MCParameters.coefficientsWeights[7]);
                cwf.WriteLine("CatCa (CatCa), {0}", MCParameters.coefficientsWeights[8]);
                cwf.WriteLine("CatCb (CatCb), {0}", MCParameters.coefficientsWeights[9]);
            }
        }

        static void _setSeriesWeights(string fileName)
        {
            ArrayList l = new ArrayList();
            long count = 0;

            using (StreamReader r = new StreamReader(fileName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    l.Add(line);
                    count++;
                }
            }
            MCParameters.seriesWeights = new double[count];
            long n = 0;

            foreach (string m in l)
            {
                Console.Write("{0} : ", m);
                string k = Console.ReadLine();
                try     { MCParameters.seriesWeights[n] = Convert.ToDouble(k); }
                catch   { MCParameters.seriesWeights[n] = 0.0; }
                n++;
            }
        }

        public static int setSeriesWeights()
        {
            Console.WriteLine();
            Console.WriteLine("*********************************");
            Console.WriteLine();
            try
            {
                //debug again
                _setSeriesWeights(MCParameters.coefficientsFile);
                return 0;
            }
            catch { return -1; }
        }

        public static void setNumberOfContaminants()
        {
            string r;
            int s;
            
            Console.WriteLine("**********************************\n\n");
            Console.Write("Please enter the number of contaminants : ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.numberOfContaminants = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of contaminants has been set to {0}", MCParameters.numberOfContaminants);
            }
        }

        public static void setLandUseAndReaches()
        {
            string r;
            int s;

            Console.WriteLine("**********************************\n\n");
            Console.Write("Please enter the number of land uses (6 for standard INCA) : ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.numberOfLandUses = s;
            }
            catch 
            {
                Console.WriteLine("You did not enter a valid number, the number of land uses has been set to {0}", MCParameters.numberOfLandUses);
            }
            Console.Write("Please enter the nunber of reaches : ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.numberOfReaches = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of reaches has been set to {0}", MCParameters.numberOfReaches);
            }
            Console.Write("Please enter the number of buckets (use a value of 3 for INCA) : ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.numberOfBoxes = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of buckets has been set to {0}", MCParameters.numberOfBoxes);
            }
            if (MCParameters.model == 3 || MCParameters.model==5 )  // running PECo or Tox
            {
                Console.Write("Please enter the number of sediment size classes : ");
                r = Console.ReadLine();
                try
                {
                    s = Convert.ToInt32(r);
                    MCParameters.numberOfSedimentClasses = s;
                }
                catch
                {
                    Console.WriteLine("You did not enter a valid number, the number of size classes has been set to {0}", MCParameters.numberOfSedimentClasses);
                }
            }
        }

        public static void setRunMCParameters()
        {
            string r;
            int s;

            Console.Write("Please enter the number of parameter sets you would like in the final ensemble: ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.maxTries = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of parameter sets has been set to {0}", MCParameters.maxTries);
            }

            Console.Write("Please enter the number of runs used for the identification of each candidate parameter set: ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.maxJumps = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of runs has been set to {0}", MCParameters.maxJumps);
            }
            Console.Write("Please enter the maximum number of unsuccessful jumps: ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.maxUnsuccessfulJumps = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of unsuccessful jumps has been set to {0}", MCParameters.maxUnsuccessfulJumps);
            }
            Console.Write("Please enter the number of files to use for model output: ");
            r = Console.ReadLine();
            try
            {
                s = Convert.ToInt32(r);
                MCParameters.splitsToUse = s;
            }
            catch
            {
                Console.WriteLine("You did not enter a valid number, the number of files for model output has been set to {0}", MCParameters.splitsToUse);
            }
        }

        public static void addAvailableModels()
        {
            availableModels = new List<modelStruct>();
            availableModels.Add(new modelStruct(1, "PERSiST_1.4", "1.4.x"));
            availableModels.Add(new modelStruct(2, "INCA-C", "1.7"));
            availableModels.Add(new modelStruct(3, "INCA-PEco", "All"));
            availableModels.Add(new modelStruct(4, "INCA-P", "1.4.x"));
            availableModels.Add(new modelStruct(5, "INCA-Contaminants (Including MP)", "All"));
            availableModels.Add(new modelStruct(6, "INCA-Path", "1.0"));
            availableModels.Add(new modelStruct(7, "INCA-Hg", "1.4"));
            availableModels.Add(new modelStruct(8, "PERSiST_1.6", "1.6.x"));
            availableModels.Add(new modelStruct(9, "INCA(ON)THE", "1.0"));
            availableModels.Add(new modelStruct(10, "PERSiST_2", "2.0"));
            availableModels.Add(new modelStruct(11, "INCA_C_v2", "2.0"));
            availableModels.Add(new modelStruct(12, "INCA_N_v1", "1.0"));
        }
    }
}
