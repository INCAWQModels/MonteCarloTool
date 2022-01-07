﻿using System;
using System.IO;

namespace MC
{
    public static class GLUEAccounting
    {
        public static bool runningGLUE = false;
        public static bool saveForGLUE = false;
        static long runCounter = 0;

        public static int saveGLUEResults(parameterSet p)
        {
            int returnVal;
            if (saveForGLUE)
            {
                string fileName = "GLUE_" + runCounter.ToString() + ".par";
                returnVal = p.write(fileName);
                using (StreamWriter w = File.AppendText("GLUEPerformance.csv"))
                {
                    w.WriteLine(runCounter.ToString() + ", "+p.modelPerformance.ToString());
                }
                runCounter++;
            }
            else returnVal = 0;
            saveForGLUE = false;


            return returnVal;
        }
        public static void GLUERun(string commandLine, long runNumber)
        {
            double testModelPerformance;
            parameterSet g = new parameterSet(MCParameters.MCParFile, MCParameters.minParFile(), MCParameters.maxParFile());
            g.randomizeValues();
            g.write(MCParameters.MCParFile);
            InteractWithModel.runModel(commandLine);
            string targetFileName = "GLUE_ParameterSet_" + runNumber.ToString() + ".par";
            File.Copy(MCParameters.MCParFile, targetFileName);
            testModelPerformance = InteractWithModel.evaluatePerformanceStatistics();
            Console.WriteLine(runNumber.ToString() + ", " + testModelPerformance.ToString());
            using (StreamWriter w = File.AppendText("GLUEPerformance.csv"))
            {
                w.WriteLine(runNumber.ToString() + ", " + testModelPerformance.ToString());
            }
        }
    }
}

